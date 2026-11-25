using System.Text;
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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void InsertImage()
    {
        string filename = ExamplePaths.MuPdfNetInput("logo.png");
        string output = ExamplePaths.Output("MuPDF.NET", "09-InsertImage", "with-logo.pdf");
        string png = ExamplePaths.Output("MuPDF.NET", "09-InsertImage", "with-logo.png");
        var check = new ResultCheck("MuPDF.NET", "09-InsertImage");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {filename}");
        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                InsertImage(filename, output);
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

        check.Text(captured, "insert-info.txt");
        check.Properties(PdfFingerprint.FromFile(output), "with-logo.summary.txt");
        check.FileSha256(png, "with-logo.png.sha256");
        check.Finish();
    }

    // doc-start:InsertImage
    /// <summary>Create a page and insert a PNG into a rectangle</summary>
    static void InsertImage(string imagePath, string outputPath)
    {
        // Empty document + custom page size (points).
        using (var doc = Document.Open())
        {
            using Page page = doc.NewPage(width: 400, height: 300);

            // Destination rectangle for the image on the page (100×100 pt).
            var rect = new Rect(40, 40, 140, 140);

            // Insert from file path; returns the image xref number.
            int xref = page.InsertImage(rect, filename: imagePath);
            Console.WriteLine($"xref={xref}");

            doc.Save(outputPath);
        }
    }
    // doc-end:InsertImage
}
