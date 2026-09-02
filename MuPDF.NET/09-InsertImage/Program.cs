using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.InsertImage;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 09-InsertImage");
        InsertImage();
    }

    /// <summary>
    /// Create a page and insert a PNG into a rectangle.
    /// </summary>
    static void InsertImage()
    {
        string logo = ExamplePaths.MuPdfNetInput("logo.png");
        string output = ExamplePaths.Output("MuPDF.NET", "09-InsertImage", "with-logo.pdf");
        var check = new ResultCheck("MuPDF.NET", "09-InsertImage");

        // Empty document + custom page size (points).
        using (var doc = Document.Open())
        {
            using Page page = doc.NewPage(width: 400, height: 300);

            // Destination rectangle for the image on the page (100×100 pt).
            var rect = new Rect(40, 40, 140, 140);

            // Insert from file path; returns the image xref number.
            int xref = page.InsertImage(rect, filename: logo);
            ConsoleEx.Info($"Inserted image xref={xref} from {logo}");

            doc.Save(output);
        }

        var props = PdfFingerprint.FromFile(output);
        props["imageFile"] = Path.GetFileName(logo);
        check.Properties(props, "with-logo.summary.txt");
        check.Finish();
    }
}
