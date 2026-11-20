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

    /// <summary>Rewrite Images</summary>
    static void RewriteImages()
    {
        string input = "test-rewrite-images.pdf";
        string output = "rewritten.pdf";

        long size0 = new System.IO.FileInfo(input).Length;

        using (var doc = Document.Open(input))
        {
            // Subsample images above 100 DPI down to 72 DPI; JPEG quality 33.
            doc.RewriteImages(dpiThreshold: 100, dpiTarget: 72, quality: 33);
            doc.Save(output, garbage: 3, deflate: 1);
        }

        long size1 = new System.IO.FileInfo(output).Length;
        double reduction = 1.0 - (size1 / (double)size0);

        // JPEG/deflate size can wobble by a byte across runs; pin percent, not outputBytes.
    }
}
