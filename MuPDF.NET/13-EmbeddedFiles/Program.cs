using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.EmbeddedFiles;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 13-EmbeddedFiles");
        EmbeddedFiles();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void EmbeddedFiles()
    {
        string blank = ExamplePaths.MuPdfNetInput("Blank.pdf");
        string note = ExamplePaths.MuPdfNetInput("note.txt");
        string output = ExamplePaths.Output("MuPDF.NET", "13-EmbeddedFiles", "with-attachment.pdf");
        string png = ExamplePaths.Output("MuPDF.NET", "13-EmbeddedFiles", "with-attachment.png");
        var check = new ResultCheck("MuPDF.NET", "13-EmbeddedFiles");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {blank}");
        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                EmbeddedFiles(blank, note, output);
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
            pix.Save(png);

        check.Text(captured, "embedded.txt");
        check.Properties(PdfFingerprint.FromFile(output), "with-attachment.summary.txt");
        check.FileSha256(png, "with-attachment.png.sha256");
        check.Finish();
    }

    // doc-start:EmbeddedFiles
    /// <summary>Attach a file to a PDF and list embedded attachments</summary>
    static void EmbeddedFiles(string blankPath, string notePath, string outputPath)
    {
        byte[] payload = File.ReadAllBytes(notePath);
        const string embName = "note.txt";

        using (var doc = Document.Open(blankPath))
        {
            // Replace any prior attachment with the same logical name.
            if (doc.GetEmbeddedFileNames().Contains(embName))
                doc.DeleteEmbeddedFile(embName);

            // AddEmbeddedFile stores bytes in the PDF EmbeddedFiles name tree.
            doc.AddEmbeddedFile(
                name: embName,
                buffer: payload,
                filename: embName,
                uFileName: embName,
                desc: "Example attachment");

            using (Page page = doc[0])
            {
                var writer = new TextWriter(page.Rect);
                writer.FillTextbox(
                    new Rect(72, 72, 500, 120),
                    "Embedded file: note.txt",
                    new Font(fontName: "helv"),
                    fontSize: 14);
                writer.WriteText(page);
            }

            doc.Save(outputPath, garbage: 4, deflate: 1);
        }

        using (var doc = Document.Open(outputPath))
        {
            var names = doc.GetEmbeddedFileNames();
            Console.WriteLine($"embeddedCount={names.Count}");
            foreach (string name in names.OrderBy(n => n, StringComparer.Ordinal))
            {
                var info = doc.GetEmbeddedFileInfo(name);
                byte[] data = doc.GetEmbeddedFile(name);
                string sha = Convert.ToHexString(SHA256.HashData(data)).ToLowerInvariant();
                Console.WriteLine($"{name}\t{info["filename"]}\t{data.Length}\t{sha}");
            }
        }
    }
    // doc-end:EmbeddedFiles
}
