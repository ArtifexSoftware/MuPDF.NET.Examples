using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.DrawShapes;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 16-DrawShapes");
        DrawShapes();
    }

    /// <summary>
    /// Draw lines, rectangles, and circles on a page.
    /// </summary>
    static void DrawShapes()
    {
        string output = ExamplePaths.Output("MuPDF.NET", "16-DrawShapes", "shapes.pdf");
        var check = new ResultCheck("MuPDF.NET", "16-DrawShapes");

        using (var doc = Document.Open())
        {
            using Page page = doc.NewPage();

            // Dashed horizontal lines (PDF dash pattern "[5] 0").
            page.DrawLine(new Point(72, 100), new Point(500, 100), width: 1f, dashes: "[5] 0");
            page.DrawLine(new Point(72, 130), new Point(500, 130), width: 2f, color: new[] { 1f, 0f, 0f });

            // Stroke rectangle (blue).
            page.DrawRect(new Rect(72, 180, 220, 280), color: new[] { 0f, 0f, 1f }, width: 1.5f);

            // Filled circle (green fill, dark stroke).
            page.DrawCircle(
                new Point(350, 230),
                radius: 40,
                color: new[] { 0f, 0f, 0f },
                fill: new[] { 0f, 0.6f, 0f },
                width: 1f);

            doc.Save(output);
            ConsoleEx.Info($"Wrote: {output}");
        }

        // Drawing-only PDF may have little extractable text — fingerprint still records pageCount.
        check.Properties(PdfFingerprint.FromFile(output), "shapes.summary.txt");
        check.Finish();
    }
}
