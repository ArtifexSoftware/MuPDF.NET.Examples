using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.Watermark;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 26-Watermark");
        Watermark();
    }

    /// <summary>
    /// Insert a logo image as a watermark on every page.
    /// </summary>
    static void Watermark()
    {
        string input = ExamplePaths.MuPdfNetInput("sample.pdf");
        string logo = ExamplePaths.MuPdfNetInput("logo.png");
        string output = ExamplePaths.Output("MuPDF.NET", "26-Watermark", "watermarked.pdf");
        var check = new ResultCheck("MuPDF.NET", "26-Watermark");

        using var doc = Document.Open(input);
        // Insert once as a reusable xref, then place on each page (docs watermark tip).
        int imageXref = -1;
        for (int i = 0; i < doc.PageCount; i++)
        {
            using Page page = doc[i];
            Rect bounds = page.Rect;
            // Centered watermark rectangle (~40% of page width).
            float w = bounds.Width * 0.4f;
            float h = bounds.Height * 0.4f;
            float x0 = bounds.X0 + (bounds.Width - w) / 2f;
            float y0 = bounds.Y0 + (bounds.Height - h) / 2f;
            var rect = new Rect(x0, y0, x0 + w, y0 + h);

            if (imageXref < 0)
                imageXref = page.InsertImage(rect, filename: logo, keepProportion: true, overlay: "true");
            else
                page.InsertImage(rect, xref: imageXref, keepProportion: true, overlay: "true");
        }

        ConsoleEx.Info($"Watermark xref={imageXref} on {doc.PageCount} page(s)");
        doc.Save(output);

        var props = PdfFingerprint.FromFile(output);
        props["watermark"] = Path.GetFileName(logo);
        check.Properties(props, "watermarked.summary.txt");
        check.Finish();
    }
}
