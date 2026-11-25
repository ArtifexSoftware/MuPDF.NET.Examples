using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.FileAnnot;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 30-FileAnnot");
        FileAnnot();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void FileAnnot()
    {
        string input = ExamplePaths.MuPdfNetInput("file-annot.pdf");
        string note = ExamplePaths.MuPdfNetInput("spare-list.txt");
        string output = ExamplePaths.Output("MuPDF.NET", "30-FileAnnot", "with-file-annot.pdf");
        string beforePng = ExamplePaths.Output("MuPDF.NET", "30-FileAnnot", "before.png");
        string afterPng = ExamplePaths.Output("MuPDF.NET", "30-FileAnnot", "after.png");
        var check = new ResultCheck("MuPDF.NET", "30-FileAnnot");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(beforePng);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                FileAnnot(input, note, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        using (var doc = Document.Open(output))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(afterPng);

        check.Text(captured, "file-annot.txt");
        check.Properties(PdfFingerprint.FromFile(output), "with-file-annot.summary.txt");
        check.FileSha256(beforePng, "before.png.sha256");
        check.FileSha256(afterPng, "after.png.sha256");
        check.Finish();
    }

    // doc-start:FileAnnot
    /// <summary>Attach a file as a page annotation (paperclip), distinct from EmbeddedFiles</summary>
    static void FileAnnot(string inputPath, string notePath, string outputPath)
    {
        byte[] payload = File.ReadAllBytes(notePath);

        using (var doc = Document.Open(inputPath))
        using (Page page = doc[0])
        {
            // Page-level file attachment annotation (vs Document.AddEmbeddedFile).
            Annot annot = page.AddFileAnnot(
                point: new Point(448, 16),
                buffer_: payload,
                filename: "spare-list.txt",
                uFileName: "spare-list.txt",
                desc: "Harbor spare list",
                icon: "Paperclip");
            annot.Update();

            var fileInfo = annot.GetFileInfo();
            Console.WriteLine($"annotType={annot.TypeString}");
            Console.WriteLine($"filename={fileInfo["filename"]}");
            Console.WriteLine($"desc={fileInfo["desc"]}");
            Console.WriteLine($"size={payload.Length}");
            doc.Save(outputPath, garbage: 3, deflate: 1);
        }

        using (var doc = Document.Open(outputPath))
        using (Page page = doc[0])
        {
            var annots = page.Annots().ToList();
            Console.WriteLine($"annotCount={annots.Count}");
            Console.WriteLine($"type={annots[0].TypeString}");
            byte[] extracted = annots[0].GetFile();
            Console.WriteLine($"extractedSize={extracted.Length}");
            Console.WriteLine($"roundtrip={extracted.SequenceEqual(payload)}");
        }
    }
    // doc-end:FileAnnot
}
