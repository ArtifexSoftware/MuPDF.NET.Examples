using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.AnnotationsRedact;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 07-AnnotationsRedact");
        AnnotationsRedact();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void AnnotationsRedact()
    {
        string input = ExamplePaths.MuPdfNetInput("Blank.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "07-AnnotationsRedact", "annotated.pdf");
        string png = ExamplePaths.Output("MuPDF.NET", "07-AnnotationsRedact", "annotated.png");
        var check = new ResultCheck("MuPDF.NET", "07-AnnotationsRedact");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                AnnotationsRedact(input, output);
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

        string info = string.Join(
            "\n",
            captured.Split('\n').Where(line => line.StartsWith("annotCount=", StringComparison.Ordinal)));
        check.Text(info + "\n", "annot-info.txt");
        check.Properties(PdfFingerprint.FromFile(output), "annotated.summary.txt");
        check.FileSha256(png, "annotated.png.sha256");
        check.Finish();
    }

    // doc-start:AnnotationsRedact
    /// <summary>Add text/rect annotations and apply a redaction</summary>
    static void AnnotationsRedact(string inputPath, string outputPath)
    {
        using (var doc = Document.Open(inputPath))
        using (Page page = doc[0])
        {
            var rect = new Rect(72, 72, 300, 120);

            // Sticky-note style text annotation.
            Annot text = page.AddTextAnnot(new Point(rect.X0, rect.Y0), "Example note");
            text.SetInfo(
                content: "Hello from 07-AnnotationsRedact",
                title: "MuPDF.NET.Examples");
            text.Update(); // rebuild appearance stream

            // Rectangle markup annotation (red stroke).
            Annot box = page.AddRectAnnot(rect);
            box.SetColors(stroke: new[] { 1f, 0f, 0f });
            box.Update();

            // Redaction: mark area, then ApplyRedactions() permanently removes content.
            Annot redact = page.AddRedactAnnot(new Rect(72, 200, 250, 230), text: "REDACTED");
            redact.Update();
            page.ApplyRedactions();

            int annotCount = page.Annots().Count();
            Console.WriteLine($"annotCount={annotCount}");
            doc.Save(outputPath);
        }
    }
    // doc-end:AnnotationsRedact
}
