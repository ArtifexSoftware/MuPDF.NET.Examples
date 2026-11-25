using System.Text;
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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ImageToPdf()
    {
        string imagePath = ExamplePaths.MuPdfNetInput("image-to-pdf.png");
        string output = ExamplePaths.Output("MuPDF.NET", "27-ImageToPdf", "wrapped.pdf");
        string sourcePng = ExamplePaths.Output("MuPDF.NET", "27-ImageToPdf", "source.png");
        string pagePng = ExamplePaths.Output("MuPDF.NET", "27-ImageToPdf", "page.png");
        var check = new ResultCheck("MuPDF.NET", "27-ImageToPdf");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {imagePath}");
        File.Copy(imagePath, sourcePng, overwrite: true);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                ImageToPdf(imagePath, output);
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
            pix.Save(pagePng);

        check.Text(captured, "image-to-pdf.txt");
        check.Properties(PdfFingerprint.FromFile(output), "wrapped.summary.txt");
        check.FileSha256(sourcePng, "source.png.sha256");
        check.FileSha256(pagePng, "page.png.sha256");
        check.Finish();
    }

    // doc-start:ImageToPdf
    /// <summary>Create a one-page PDF sized to an image and place the image full-bleed</summary>
    static void ImageToPdf(string imagePath, string outputPath)
    {
        using var pixmap = new Pixmap(imagePath);
        // PDF page size in points: map 72 dpi → 1 point per pixel.
        float width = pixmap.Width;
        float height = pixmap.Height;

        using var doc = Document.Open();
        using Page page = doc.NewPage(width: width, height: height);
        int xref = page.InsertImage(page.Rect, filename: imagePath);
        Console.WriteLine($"width={pixmap.Width}");
        Console.WriteLine($"height={pixmap.Height}");
        Console.WriteLine($"n={pixmap.N}");
        Console.WriteLine($"pageCount={doc.PageCount}");
        Console.WriteLine($"xref={xref}");
        Console.WriteLine($"sizedToImage={page.Rect.Width == width && page.Rect.Height == height}");
        doc.Save(outputPath);
    }
    // doc-end:ImageToPdf
}
