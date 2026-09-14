using System.Linq;
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

    /// <summary>
    /// Add text/rect annotations and apply a redaction.
    /// </summary>
    static void AnnotationsRedact()
    {
        string input = ExamplePaths.MuPdfNetInput("Blank.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "07-AnnotationsRedact", "annotated.pdf");
        var check = new ResultCheck("MuPDF.NET", "07-AnnotationsRedact");

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        {
            var rect = new Rect(72, 72, 300, 120);

            // Sticky-note style text annotation.
            Annot text = page.AddTextAnnot(new Point(rect.X0, rect.Y0), "Example note");
            text.SetInfo(
                content: "Hello from 07-AnnotationsRedact",
                title: "MuPDF.NET.Examples",
                creationDate: null,
                modDate: null,
                subject: null);
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
            ConsoleEx.Info($"Page annots remaining: {annotCount}");
            doc.Save(output);

            var props = PdfFingerprint.FromFile(output);
            props["annotCount"] = annotCount.ToString();
            check.Properties(props, "annotated.summary.txt");
        }

        check.Finish();
    }
}
