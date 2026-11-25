using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.Barcodes;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 12-Barcodes");
        WriteAndReadBarcode();
    }

    // help:skip — resolves output paths, calls the documented overload, then checks the harness baseline.
    static void WriteAndReadBarcode()
    {
        string output = ExamplePaths.Output("MuPDF.NET", "12-Barcodes", "barcodes.pdf");
        string png = ExamplePaths.Output("MuPDF.NET", "12-Barcodes", "barcodes.png");
        var check = new ResultCheck("MuPDF.NET", "12-Barcodes");
        var sb = new StringBuilder();

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                WriteAndReadBarcode(output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        captured = string.Join(
            "\n",
            captured.Split('\n').Where(line =>
                line.StartsWith("barcodeCount=", StringComparison.Ordinal)
                || (line.Length > 0 && line.Contains('\t') && !line.StartsWith("up ", StringComparison.Ordinal))));
        Console.Write(captured);
        if (!captured.EndsWith("\n", StringComparison.Ordinal))
            Console.WriteLine();

        using (var doc = Document.Open(output))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(png);

        check.Text(captured.EndsWith("\n") ? captured : captured + "\n", "barcodes.txt");
        check.Properties(PdfFingerprint.FromFile(output), "barcodes.summary.txt");
        check.FileSha256(png, "barcodes.png.sha256");
        check.Finish();
    }

    // doc-start:WriteAndReadBarcode
    /// <summary>Write several barcode types (including QR) to a PDF, then read them back</summary>
    static void WriteAndReadBarcode(string outputPath)
    {
        // Clip size is encoding pixels (1pt = 1px). 1D codes need a short height (bar height).
        // PDF417 is last: its placed image is taller than the clip.
        var samples = new (BarcodeFormat format, string text, Rect rect, bool fit)[]
        {
            (BarcodeFormat.QR, "https://mupdf.com", new Rect(40, 36, 160, 156), true),
            (BarcodeFormat.AZTEC, "MuPDF.NET", new Rect(220, 36, 340, 156), true),
            (BarcodeFormat.CODE128, "Hello World!", new Rect(40, 188, 340, 228), false),
            (BarcodeFormat.CODE39, "CODE39", new Rect(40, 268, 340, 308), false),
            (BarcodeFormat.EAN8, "1234567", new Rect(40, 348, 240, 388), false),
            (BarcodeFormat.PDF417, "Hello World!", new Rect(40, 428, 340, 436), false),
        };

        using (var doc = Document.Open())
        {
            using Page page = doc.NewPage(width: 480, height: 560);
            var writer = new TextWriter(page.Rect);
            var font = new Font(fontName: "helv");

            foreach (var (format, text, rect, fit) in samples)
            {
                writer.FillTextbox(
                    new Rect(rect.X0, rect.Y0 - 16, rect.X1 + 120, rect.Y0),
                    format.ToString(),
                    font,
                    fontSize: 10);
                page.WriteBarcode(
                    rect,
                    text,
                    format,
                    forceFitToRect: fit,
                    pureBarcode: true,
                    marginLeft: 8,
                    marginTop: 8,
                    marginRight: 8,
                    marginBottom: 8);
            }

            writer.WriteText(page);
            doc.Save(outputPath);
        }

        using (var doc = Document.Open(outputPath))
        using (Page page = doc[0])
        {
            Console.WriteLine($"barcodeCount={samples.Length}");
            foreach (var (format, _, _, _) in samples)
            {
                // Scan the page for this format so clip size vs placed image size cannot miss.
                var found = page.ReadBarcodes(barcodeFormat: format)
                    .FirstOrDefault(b => b.BarcodeFormat == format);
                Console.WriteLine($"{format}\t{found?.Text}");
            }
        }
    }
    // doc-end:WriteAndReadBarcode
}
