using System.Linq;
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

    /// <summary>
    /// List images on page 1 and write each embedded image stream to disk.
    /// </summary>
    static void ExtractImages()
    {
        string input = ExamplePaths.MuPdfNetInput("Color.pdf");
        string outDir = Path.GetDirectoryName(
            ExamplePaths.Output("MuPDF.NET", "21-ExtractImages", "_"))!;
        var check = new ResultCheck("MuPDF.NET", "21-ExtractImages");
        var report = new StringBuilder();

        using var doc = Document.Open(input);
        using Page page = doc[0];

        // GetImages(full: true) returns xref + colorspace / filter metadata.
        List<Entry> images = page.GetImages(full: true);
        report.Append("imageCount=").Append(images.Count).Append('\n');
        ConsoleEx.Info($"Page 1 images: {images.Count}");

        int index = 0;
        foreach (Entry entry in images.OrderBy(e => e.Xref))
        {
            ImageInfo extracted = doc.ExtractImage(entry.Xref);
            string ext = string.IsNullOrWhiteSpace(extracted.Ext) ? "bin" : extracted.Ext;
            string fileName = $"img-{index:D2}-xref{entry.Xref}.{ext}";
            string path = Path.Combine(outDir, fileName);
            File.WriteAllBytes(path, extracted.Image ?? Array.Empty<byte>());

            report.Append("xref=").Append(entry.Xref)
                .Append(" ext=").Append(ext)
                .Append(" w=").Append((int)extracted.Width)
                .Append(" h=").Append((int)extracted.Height)
                .Append(" n=").Append(extracted.ColorSpace)
                .Append(" bytes=").Append(extracted.Image?.Length ?? 0)
                .Append(" sha=").Append(ResultCheck.Sha256HexBytes(extracted.Image ?? Array.Empty<byte>()))
                .Append('\n');
            check.FileSha256(path, fileName + ".sha256");
            index++;
        }

        check.Text(report.ToString(), "extract-images.txt");
        check.Finish();
    }
}
