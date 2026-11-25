using System.Globalization;
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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void CompressSave()
    {
        string input = ExamplePaths.MuPdfNetInput("compress-save.pdf");
        string plain = ExamplePaths.Output("MuPDF.NET", "25-CompressSave", "plain.pdf");
        string compressed = ExamplePaths.Output("MuPDF.NET", "25-CompressSave", "compressed.pdf");
        string plainPng = ExamplePaths.Output("MuPDF.NET", "25-CompressSave", "plain.png");
        string compressedPng = ExamplePaths.Output("MuPDF.NET", "25-CompressSave", "compressed.png");
        var check = new ResultCheck("MuPDF.NET", "25-CompressSave");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                CompressSave(input, plain, compressed);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        using (var doc = Document.Open(plain))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(plainPng);

        using (var doc = Document.Open(compressed))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(compressedPng);

        check.Text(captured, "compress.txt");
        check.Properties(PdfFingerprint.FromFile(plain), "plain.summary.txt");
        check.Properties(PdfFingerprint.FromFile(compressed), "compressed.summary.txt");
        check.FileSha256(plainPng, "plain.png.sha256");
        check.FileSha256(compressedPng, "compressed.png.sha256");
        check.Finish();
    }

    // doc-start:CompressSave
    /// <summary>Compare a plain save vs a compressed save (garbage + deflate + object streams)</summary>
    static void CompressSave(string inputPath, string plainPath, string compressedPath)
    {
        using (var doc = Document.Open(inputPath))
            doc.Save(plainPath);

        using (var doc = Document.Open(inputPath))
        {
            // garbage:4 drops unused/duplicate objects; deflate compresses streams;
            // useObjstms packs object definitions into those streams.
            doc.Save(
                compressedPath,
                garbage: 4,
                deflate: 1,
                deflateImages: 1,
                deflateFonts: 1,
                useObjstms: 1);
        }

        long inputBytes = new System.IO.FileInfo(inputPath).Length;
        long plainBytes = new System.IO.FileInfo(plainPath).Length;
        long compressedBytes = new System.IO.FileInfo(compressedPath).Length;
        string fileSizePct = (100.0 * compressedBytes / plainBytes)
            .ToString("0.0", CultureInfo.InvariantCulture);
        Console.WriteLine($"inputBytes={inputBytes}");
        Console.WriteLine($"plainBytes={plainBytes}");
        Console.WriteLine($"compressedBytes={compressedBytes}");
        Console.WriteLine($"fileSizePct={fileSizePct}");
        Console.WriteLine($"smallerOrEqual={compressedBytes <= plainBytes}");
    }
    // doc-end:CompressSave
}
