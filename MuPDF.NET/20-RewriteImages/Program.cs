using System.Globalization;
using System.Text;
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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void RewriteImages()
    {
        string input = ExamplePaths.MuPdfNetInput("rewrite-images.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "20-RewriteImages", "rewritten.pdf");
        string beforePng = ExamplePaths.Output("MuPDF.NET", "20-RewriteImages", "before.png");
        string afterPng = ExamplePaths.Output("MuPDF.NET", "20-RewriteImages", "after.png");
        string rasterBefore = ExamplePaths.Output("MuPDF.NET", "20-RewriteImages", "raster-before.png");
        string rasterAfter = ExamplePaths.Output("MuPDF.NET", "20-RewriteImages", "raster-after.png");
        var check = new ResultCheck("MuPDF.NET", "20-RewriteImages");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        {
            using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
                pix.Save(beforePng);
            using var raster = new Pixmap(doc, page.GetImages(full: true)[0].Xref);
            raster.Save(rasterBefore);
        }

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                RewriteImages(input, output);
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
        {
            using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
                pix.Save(afterPng);
            using var raster = new Pixmap(doc, page.GetImages(full: true)[0].Xref);
            raster.Save(rasterAfter);
        }

        check.Text(captured, "rewritten.txt");
        check.Properties(PdfFingerprint.FromFile(output), "rewritten.summary.txt");
        check.FileSha256(beforePng, "before.png.sha256");
        check.FileSha256(afterPng, "after.png.sha256");
        check.FileSha256(rasterBefore, "raster-before.png.sha256");
        check.FileSha256(rasterAfter, "raster-after.png.sha256");
        check.Finish();
    }

    // doc-start:RewriteImages
    /// <summary>Downsample and recompress images in a PDF</summary>
    static void RewriteImages(string inputPath, string outputPath)
    {
        long size0 = new System.IO.FileInfo(inputPath).Length;
        int beforeW;

        using (var doc = Document.Open(inputPath))
        using (Page page = doc[0])
        {
            Console.WriteLine("---before---");
            Console.WriteLine($"page={page.Width:0}x{page.Height:0}");
            Console.WriteLine($"inputBytes={size0}");
            DumpImages(page);
            beforeW = (int)page.GetImages(full: true)[0].Width;

            // Subsample images above 100 DPI down to 72 DPI; JPEG quality 33.
            doc.RewriteImages(dpiThreshold: 100, dpiTarget: 72, quality: 33);
            doc.Save(outputPath, garbage: 3, deflate: 1);
        }

        long size1 = new System.IO.FileInfo(outputPath).Length;
        using (var doc = Document.Open(outputPath))
        using (Page page = doc[0])
        {
            int afterW = (int)page.GetImages(full: true)[0].Width;
            int pixelPct = (int)Math.Round(100.0 * afterW / beforeW);
            string fileSizePct = (100.0 * size1 / size0).ToString("0.0", CultureInfo.InvariantCulture);
            Console.WriteLine("---after---");
            Console.WriteLine($"page={page.Width:0}x{page.Height:0}");
            Console.WriteLine($"pixelPct={pixelPct}");
            Console.WriteLine($"fileSizePct={fileSizePct}");
            DumpImages(page);
        }

        static void DumpImages(Page page)
        {
            List<Entry> images = page.GetImages(full: true);
            Console.WriteLine($"imageCount={images.Count}");
            foreach (Entry img in images)
            {
                Rect bbox = page.GetImageBbox((object)img.Name);
                Console.WriteLine($"xref={img.Xref} name={img.Name} pixels={img.Width:0}x{img.Height:0} draw={bbox.Width:0}x{bbox.Height:0} bpc={img.Bpc} cs={img.CsName}");
            }
        }
    }
    // doc-end:RewriteImages
}
