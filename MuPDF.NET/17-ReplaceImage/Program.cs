using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.ReplaceImage;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 17-ReplaceImage");
        ReplaceImage();
    }

    /// <summary>
    /// Replace the first image on a page with the Artifex logo at its native aspect ratio.
    /// </summary>
    static void ReplaceImage()
    {
        string input = ExamplePaths.MuPdfNetInput("Color.pdf");
        string replacement = ExamplePaths.MuPdfNetInput("logo.png");
        string output = ExamplePaths.Output("MuPDF.NET", "17-ReplaceImage", "replaced.pdf");
        var check = new ResultCheck("MuPDF.NET", "17-ReplaceImage");

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        {
            // List images on the page (xref + colorspace, …).
            List<Entry> images = page.GetImages(full: true);
            if (images.Count == 0)
                throw new InvalidOperationException("No images found on page 1.");

            int xref = images[0].Xref;
            List<Box> places = page.GetImageRects(xref);
            if (places.Count == 0)
                throw new InvalidOperationException("Image xref is not drawn on page 1.");
            Rect place = places[0].Rect;

            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Replacing image xref={xref} with {Path.GetFileName(replacement)}");
            ConsoleEx.Info($"Placement: {place}");

            // Clear the old image, then insert into the same rect keeping the logo ratio.
            page.DeleteImage(xref);
            page.InsertImage(place, filename: replacement, keepProportion: true);
            doc.Save(output);
        }

        var props = PdfFingerprint.FromFile(output);
        props["replacement"] = Path.GetFileName(replacement);
        check.Properties(props, "replaced.summary.txt");
        check.Finish();
    }
}
