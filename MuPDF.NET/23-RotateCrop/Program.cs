using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.RotateCrop;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 23-RotateCrop");
        RotateCrop();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void RotateCrop()
    {
        string input = ExamplePaths.MuPdfNetInput("rotate-crop.pdf");
        string rotated = ExamplePaths.Output("MuPDF.NET", "23-RotateCrop", "rotated.pdf");
        string cropped = ExamplePaths.Output("MuPDF.NET", "23-RotateCrop", "cropped.pdf");
        string originalPng = ExamplePaths.Output("MuPDF.NET", "23-RotateCrop", "original.png");
        string rotatedPng = ExamplePaths.Output("MuPDF.NET", "23-RotateCrop", "rotated.png");
        string croppedPng = ExamplePaths.Output("MuPDF.NET", "23-RotateCrop", "cropped.png");
        var check = new ResultCheck("MuPDF.NET", "23-RotateCrop");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(originalPng);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                RotateCrop(input, rotated, cropped);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        using (var doc = Document.Open(rotated))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(rotatedPng);

        using (var doc = Document.Open(cropped))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(croppedPng);

        check.Text(captured, "rotate-crop.txt");
        check.Properties(PdfFingerprint.FromFile(rotated), "rotated.summary.txt");
        check.Properties(PdfFingerprint.FromFile(cropped), "cropped.summary.txt");
        check.FileSha256(originalPng, "original.png.sha256");
        check.FileSha256(rotatedPng, "rotated.png.sha256");
        check.FileSha256(croppedPng, "cropped.png.sha256");
        check.Finish();
    }

    // doc-start:RotateCrop
    /// <summary>Rotate page 1 and set a CropBox, then save</summary>
    static void RotateCrop(string inputPath, string rotatedPath, string croppedPath)
    {
        static string Box(Rect r) =>
            r == null ? "null" : $"{r.X0:0},{r.Y0:0},{r.X1:0},{r.Y1:0}";

        using (var doc = Document.Open(inputPath))
        using (Page page = doc[0])
        {
            Console.WriteLine($"before.rotation={page.Rotation}");
            Console.WriteLine($"before.rect={Box(page.Rect)}");
            Console.WriteLine($"before.cropbox={Box(page.CropBox)}");

            // Rotate clockwise 90 degrees (PDF /Rotate).
            page.SetRotation(90);
            Console.WriteLine($"after.rotation={page.Rotation}");
            Console.WriteLine($"after.rect={Box(page.Rect)}");
            doc.Save(rotatedPath);
        }

        using (var doc = Document.Open(inputPath))
        using (Page page = doc[0])
        {
            // CropBox is unrotated. Upper-left of page.Rect is the TL quadrant.
            Rect media = page.Rect;
            var crop = new Rect(
                media.X0,
                media.Y0,
                media.X0 + media.Width / 2f,
                media.Y0 + media.Height / 2f);
            Console.WriteLine($"crop.rect={Box(crop)}");
            page.SetCropBox(crop);
            doc.Save(croppedPath);
        }

        using (var doc = Document.Open(croppedPath))
        using (Page page = doc[0])
            Console.WriteLine($"after.cropbox={Box(page.CropBox)}");
    }
    // doc-end:RotateCrop
}
