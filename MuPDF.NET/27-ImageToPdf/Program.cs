using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.ImageToPdf;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 27-ImageToPdf");
        ImageToPdf();
    }

    /// <summary>
    /// Create a one-page PDF sized to an image and place the image full-bleed.
    /// </summary>
    static void ImageToPdf()
    {
        string imagePath = ExamplePaths.MuPdfNetInput("apple.png");
        string output = ExamplePaths.Output("MuPDF.NET", "27-ImageToPdf", "apple.pdf");
        var check = new ResultCheck("MuPDF.NET", "27-ImageToPdf");

        using var pixmap = new Pixmap(imagePath);
        // PDF page size in points: map 72 dpi → 1 point per pixel.
        float width = pixmap.Width;
        float height = pixmap.Height;

        using var doc = Document.Open();
        using Page page = doc.NewPage(width: width, height: height);
        page.InsertImage(page.Rect, filename: imagePath);
        doc.Save(output);

        ConsoleEx.Info($"Page {width}x{height} pt from {Path.GetFileName(imagePath)}");

        var props = PdfFingerprint.FromFile(output);
        props["source"] = Path.GetFileName(imagePath);
        props["width"] = ((int)width).ToString();
        props["height"] = ((int)height).ToString();
        check.Properties(props, "apple.summary.txt");
        check.Finish();
    }
}
