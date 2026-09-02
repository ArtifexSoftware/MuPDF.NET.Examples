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

    /// <summary>
    /// Attach a file as a page annotation (paperclip), distinct from EmbeddedFiles.
    /// </summary>
    static void FileAnnot()
    {
        string blank = ExamplePaths.MuPdfNetInput("Blank.pdf");
        string note = ExamplePaths.MuPdfNetInput("note.txt");
        string output = ExamplePaths.Output("MuPDF.NET", "30-FileAnnot", "with-file-annot.pdf");
        var check = new ResultCheck("MuPDF.NET", "30-FileAnnot");
        var report = new StringBuilder();

        byte[] payload = File.ReadAllBytes(note);

        using (var doc = Document.Open(blank))
        using (Page page = doc[0])
        {
            // Page-level file attachment annotation (vs Document.AddEmbeddedFile).
            Annot annot = page.AddFileAnnot(
                point: new Point(72, 72),
                buffer_: payload,
                filename: "note.txt",
                uFileName: "note.txt",
                desc: "Example file annotation");
            annot.Update();

            report.Append("annotType=").Append(annot.TypeString).Append('\n');
            var fileInfo = annot.GetFileInfo();
            report.Append("filename=").Append(fileInfo.GetValueOrDefault("filename") ?? "").Append('\n');
            report.Append("size=").Append(fileInfo.GetValueOrDefault("size") ?? payload.Length).Append('\n');
            doc.Save(output, garbage: 3, deflate: 1);
        }

        using (var doc = Document.Open(output))
        using (Page page = doc[0])
        {
            var annots = page.Annots().ToList();
            report.Append("annotCount=").Append(annots.Count).Append('\n');
            foreach (Annot a in annots)
                report.Append("type=").Append(a.TypeString).Append('\n');
        }

        check.Text(report.ToString(), "file-annot.txt");
        check.Properties(PdfFingerprint.FromFile(output), "with-file-annot.summary.txt");
        check.Finish();
    }
}
