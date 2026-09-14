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

    /// <summary>
    /// Recolor page images/vectors to DeviceCMYK (4 components). Copy this method into your project.
    /// </summary>
    static void Recolor()
    {
        string input = ExamplePaths.MuPdfNetInput("Color.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "05-Recolor", "recolor.pdf");
        var check = new ResultCheck("MuPDF.NET", "05-Recolor");

        using (var doc = Document.Open(input))
        {
            List<Entry> before = doc.GetPageImages(0);
            if (before.Count == 0)
                throw new InvalidOperationException("No images on page 1.");

            // GetPageImages: CsName is the PDF /ColorSpace name (e.g. DeviceRGB).
            // AltCsName is only set for some alternate spaces — usually empty.
            string csBefore = before[0].CsName ?? "";
            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Page 1 image colorspace before: {csBefore}");

            // Recolor page 0 → 4 components (DeviceCMYK / ICC-based CMYK).
            doc.Recolor(0, 4);

            List<Entry> after = doc.GetPageImages(0);
            // Prefer CsName (not AltCsName). Use null/empty coalescing carefully:
            // AltCsName is often "" after recolor; `??` does NOT fall through empty strings.
            string csAfter = string.IsNullOrEmpty(after[0].CsName) ? "" : after[0].CsName;

            // ExtractImage gives component count + a richer cs-name (includes ICC profile).
            ImageInfo extracted = doc.ExtractImage(after[0].Xref);
            ConsoleEx.Info($"Page 1 image colorspace after: {csAfter}");
            ConsoleEx.Info($"ExtractImage: n={extracted.ColorSpace}, cs-name={extracted.CsName}");

            doc.Save(output);

            var props = PdfFingerprint.FromFile(output);
            props["csBefore"] = csBefore;
            props["csAfter"] = csAfter;
            props["extractCsName"] = extracted.CsName ?? "";
            props["extractComponents"] = extracted.ColorSpace.ToString();
            check.Properties(props, "recolor.summary.txt");
        }

        check.Finish();
    }
}
