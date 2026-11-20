using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.CompressSave;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 25-CompressSave");
        CompressSave();
    }

    /// <summary>Compare a plain save vs a compressed save (garbage + deflate + object streams)</summary>
    static void CompressSave()
    {
        string input = "sample.pdf";
        string plain = "plain.pdf";
        string compressed = "compressed.pdf";

        using (var doc = Document.Open(input))
            doc.Save(plain);

        using (var doc = Document.Open(input))
        {
            // Docs: https://mupdfnet.readthedocs.io/en/latest/compressing-files.html
            doc.Save(
                compressed,
                garbage: 4,
                deflate: 1,
                deflateImages: 1,
                deflateFonts: 1,
                useObjstms: 1);
        }

        long plainBytes = new System.IO.FileInfo(plain).Length;
        long compressedBytes = new System.IO.FileInfo(compressed).Length;
        bool smallerOrEqual = compressedBytes <= plainBytes;
    }
}
