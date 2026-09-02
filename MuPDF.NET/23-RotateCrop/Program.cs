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

    /// <summary>
    /// Rotate page 1 and set a CropBox, then save.
    /// </summary>
    static void RotateCrop()
    {
        string input = ExamplePaths.MuPdfNetInput("sample.pdf");
        string rotated = ExamplePaths.Output("MuPDF.NET", "23-RotateCrop", "rotated.pdf");
        string cropped = ExamplePaths.Output("MuPDF.NET", "23-RotateCrop", "cropped.pdf");
        var check = new ResultCheck("MuPDF.NET", "23-RotateCrop");
        var report = new StringBuilder();

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        {
            report.Append("before.rotation=").Append(page.Rotation).Append('\n');
            report.Append("before.rect=").Append(Fmt(page.Rect)).Append('\n');
            report.Append("before.cropbox=").Append(Fmt(page.CropBox)).Append('\n');

            // Rotate clockwise 90 degrees (PDF /Rotate).
            page.SetRotation(90);
            doc.Save(rotated);
            report.Append("after.rotation=").Append(page.Rotation).Append('\n');
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
            report.Append("crop.rect=").Append(Fmt(crop)).Append('\n');
            report.Append("after.cropbox=").Append(Fmt(page.CropBox)).Append('\n');
        }

        check.Text(report.ToString(), "rotate-crop.txt");
        check.Properties(PdfFingerprint.FromFile(rotated), "rotated.summary.txt");
        check.Properties(PdfFingerprint.FromFile(cropped), "cropped.summary.txt");
        check.Finish();
    }

    static string Fmt(Rect r) =>
        r == null ? "null" : $"{r.X0:0.###},{r.Y0:0.###},{r.X1:0.###},{r.Y1:0.###}";
}
