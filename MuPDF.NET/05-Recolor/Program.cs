using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.Recolor;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 05-Recolor");
        Recolor();
    }

    /// <summary>Recolor</summary>
    static void Recolor()
    {
        string input = "Color.pdf";
        string output = "recolor.pdf";

        using (var doc = Document.Open(input))
        {
            List<Entry> before = doc.GetPageImages(0);
            if (before.Count == 0)
                throw new InvalidOperationException("No images on page 1.");

            // GetPageImages: CsName is the PDF /ColorSpace name (e.g. DeviceRGB).
            // AltCsName is only set for some alternate spaces — usually empty.
            string csBefore = before[0].CsName ?? "";

            // Recolor page 0 → 4 components (DeviceCMYK / ICC-based CMYK).
            doc.Recolor(0, 4);

            List<Entry> after = doc.GetPageImages(0);
            // Prefer CsName (not AltCsName). Use null/empty coalescing carefully:
            // AltCsName is often "" after recolor; `??` does NOT fall through empty strings.
            string csAfter = string.IsNullOrEmpty(after[0].CsName) ? "" : after[0].CsName;

            // ExtractImage gives component count + a richer cs-name (includes ICC profile).
            ImageInfo extracted = doc.ExtractImage(after[0].Xref);

            doc.Save(output);

        }
    }
}
