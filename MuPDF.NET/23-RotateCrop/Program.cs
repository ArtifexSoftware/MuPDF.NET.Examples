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

    /// <summary>Rotate page 1 and set a CropBox, then save</summary>
    static void RotateCrop()
    {
        string input = "sample.pdf";
        string rotated = "rotated.pdf";
        string cropped = "cropped.pdf";

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        {

            // Rotate clockwise 90 degrees (PDF /Rotate).
            page.SetRotation(90);
            doc.Save(rotated);
        }

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        {
            // Crop to the upper-left quadrant of the media box.
            Rect media = page.Rect;
            var crop = new Rect(
                media.X0,
                media.Y0,
                media.X0 + media.Width / 2f,
                media.Y0 + media.Height / 2f);
            page.SetCropBox(crop);
            doc.Save(cropped);
        }
    }

    static string Fmt(Rect r) =>
        r == null ? "null" : $"{r.X0:0.###},{r.Y0:0.###},{r.X1:0.###},{r.Y1:0.###}";
}
