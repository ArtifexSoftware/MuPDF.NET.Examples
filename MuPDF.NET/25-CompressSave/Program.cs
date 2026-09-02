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

    /// <summary>
    /// Compare a plain save vs a compressed save (garbage + deflate + object streams).
    /// </summary>
    static void CompressSave()
    {
        string input = ExamplePaths.MuPdfNetInput("sample.pdf");
        string plain = ExamplePaths.Output("MuPDF.NET", "25-CompressSave", "plain.pdf");
        string compressed = ExamplePaths.Output("MuPDF.NET", "25-CompressSave", "compressed.pdf");
        var check = new ResultCheck("MuPDF.NET", "25-CompressSave");
        var report = new StringBuilder();

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
        report.Append("smallerOrEqual=").Append(smallerOrEqual ? "true" : "false").Append('\n');
        report.Append("savedWith=garbage4+deflate+objstms\n");

        ConsoleEx.Info($"Plain: {plainBytes} bytes");
        ConsoleEx.Info($"Compressed: {compressedBytes} bytes");

        check.Equal(smallerOrEqual, true, "compressed <= plain");
        check.Text(report.ToString(), "compress.txt");
        check.Properties(PdfFingerprint.FromFile(plain), "plain.summary.txt");
        check.Properties(PdfFingerprint.FromFile(compressed), "compressed.summary.txt");
        check.Finish();
    }
}
