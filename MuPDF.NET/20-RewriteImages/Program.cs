using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.RewriteImages;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 20-RewriteImages");
        RewriteImages();
    }

    /// <summary>
    /// Downsample / recompress embedded images (PDF only). Same as PyMuPDF Document.rewrite_images.
    /// </summary>
    static void RewriteImages()
    {
        string input = ExamplePaths.MuPdfNetInput("test-rewrite-images.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "20-RewriteImages", "rewritten.pdf");
        var check = new ResultCheck("MuPDF.NET", "20-RewriteImages");

        long size0 = new System.IO.FileInfo(input).Length;

        using (var doc = Document.Open(input))
        {
            // Subsample images above 100 DPI down to 72 DPI; JPEG quality 33.
            doc.RewriteImages(dpiThreshold: 100, dpiTarget: 72, quality: 33);
            doc.Save(output, garbage: 3, deflate: 1);
        }

        long size1 = new System.IO.FileInfo(output).Length;
        double reduction = 1.0 - (size1 / (double)size0);

        ConsoleEx.Info($"Opened: {input}");
        ConsoleEx.Info($"Input size:  {size0} bytes");
        ConsoleEx.Info($"Output size: {size1} bytes ({reduction:P1} smaller)");
        ConsoleEx.Info($"Wrote: {output}");

        var props = PdfFingerprint.FromFile(output);
        props["inputBytes"] = size0.ToString();
        props["outputBytes"] = size1.ToString();
        props["reductionPct"] = ((int)Math.Round(reduction * 100)).ToString();
        check.Properties(props, "rewritten.summary.txt");
        check.Finish();
    }
}
