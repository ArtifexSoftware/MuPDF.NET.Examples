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

    /// <summary>List images on page 1 and write each embedded image stream to disk</summary>
    static void ExtractImages()
    {
        string input = "Color.pdf";
        string outDir = Path.GetDirectoryName(
            "_")!;

        using var doc = Document.Open(input);
        using Page page = doc[0];

        // GetImages(full: true) returns xref + colorspace / filter metadata.
        List<Entry> images = page.GetImages(full: true);

        int index = 0;
        foreach (Entry entry in images.OrderBy(e => e.Xref))
        {
            ImageInfo extracted = doc.ExtractImage(entry.Xref);
            string ext = string.IsNullOrWhiteSpace(extracted.Ext) ? "bin" : extracted.Ext;
            string fileName = $"img-{index:D2}-xref{entry.Xref}.{ext}";
            string path = Path.Combine(outDir, fileName);
            File.WriteAllBytes(path, extracted.Image ?? Array.Empty<byte>());

            index++;
        }
    }
}
