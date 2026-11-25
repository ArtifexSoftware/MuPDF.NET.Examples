using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.ExtractImages;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 21-ExtractImages");
        ExtractImages();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ExtractImages()
    {
        string input = ExamplePaths.MuPdfNetInput("extract-images.pdf");
        string outputDir = Path.GetDirectoryName(
            ExamplePaths.Output("MuPDF.NET", "21-ExtractImages", "_"))!;
        string pagePng = ExamplePaths.Output("MuPDF.NET", "21-ExtractImages", "extract-images.png");
        var check = new ResultCheck("MuPDF.NET", "21-ExtractImages");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(pagePng);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                ExtractImages(input, outputDir);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        check.Text(captured, "extract-images.txt");
        check.FileSha256(pagePng, "extract-images.png.sha256");
        foreach (string path in Directory.GetFiles(outputDir, "img-*.*")
            .OrderBy(p => p, StringComparer.Ordinal))
        {
            check.FileSha256(path, Path.GetFileName(path) + ".sha256");
        }
        check.Finish();
    }

    // doc-start:ExtractImages
    /// <summary>List images on page 1 and write each embedded image stream to disk</summary>
    static void ExtractImages(string inputPath, string outputDir)
    {
        using (var doc = Document.Open(inputPath))
        using (Page page = doc[0])
        {
            // GetImages(full: true) returns xref + colorspace / filter metadata.
            List<Entry> images = page.GetImages(full: true);
            Console.WriteLine($"imageCount={images.Count}");

            int index = 0;
            foreach (Entry entry in images.OrderBy(e => e.Xref))
            {
                ImageInfo extracted = doc.ExtractImage(entry.Xref);
                string ext = string.IsNullOrWhiteSpace(extracted.Ext) ? "bin" : extracted.Ext;
                byte[] data = extracted.Image ?? Array.Empty<byte>();
                string fileName = $"img-{index:D2}-xref{entry.Xref}.{ext}";
                File.WriteAllBytes(Path.Combine(outputDir, fileName), data);

                string sha = Convert.ToHexString(SHA256.HashData(data)).ToLowerInvariant();
                Console.WriteLine($"file={fileName} xref={entry.Xref} ext={ext} w={extracted.Width:0} h={extracted.Height:0} n={extracted.ColorSpace} bytes={data.Length} sha={sha}");
                index++;
            }
        }
    }
    // doc-end:ExtractImages
}
