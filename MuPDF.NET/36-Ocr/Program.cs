using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.Ocr;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 36-Ocr");
        Ocr();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void Ocr()
    {
        string input = ExamplePaths.MuPdfNetInput("ocr-scan.pdf");
        string outputOff = ExamplePaths.Output("MuPDF.NET", "36-Ocr", "ocr-off.md");
        string outputOn = ExamplePaths.Output("MuPDF.NET", "36-Ocr", "ocr-on.md");
        string png = ExamplePaths.Output("MuPDF.NET", "36-Ocr", "page.png");
        var check = new ResultCheck("MuPDF.NET", "36-Ocr");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(png);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                Ocr(input, outputOff, outputOn);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        var dump = new StringBuilder();
        foreach (string line in captured.Split('\n'))
        {
            if (line.Contains("pymupdf-layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("pymupdf_layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("greatly improved page layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("Using RapidOCR", StringComparison.OrdinalIgnoreCase)
                || line.Contains("Using Tesseract", StringComparison.OrdinalIgnoreCase)
                || line.Contains("Using PaddleOCR", StringComparison.OrdinalIgnoreCase)
                || line.Contains("no OCR function is available", StringComparison.OrdinalIgnoreCase)
                || line.Contains("OCR on page.", StringComparison.OrdinalIgnoreCase))
                continue;
            dump.Append(line).Append('\n');
        }

        check.Text(dump.ToString(), "ocr.txt");
        if (File.Exists(outputOff))
            check.Text(File.ReadAllText(outputOff), "ocr-off.md");
        if (File.Exists(outputOn))
            check.Text(File.ReadAllText(outputOn), "ocr-on.md");
        check.FileSha256(png, "page.png.sha256");
        check.Finish();
    }

    // doc-start:Ocr
    /// <summary>Compare Markdown with OCR off vs on (requires layout worker and an OCR engine)</summary>
    static void Ocr(string inputPath, string outputOffPath, string outputOnPath)
    {
        // Remember prior layout flag so we restore it for other examples in the same process.
        bool prior = Utils.LayoutEnabled;
        try
        {
            // ToMarkdown OCR runs only in layout mode, and only when an engine is registered.
            if (!Utils.LayoutAvailable)
            {
                Console.WriteLine("LAYOUT_UNAVAILABLE");
                return;
            }
            if (!Utils.RapidOcrAvailable && string.IsNullOrEmpty(Utils.TESSDATA_PREFIX))
            {
                Console.WriteLine("OCR_UNAVAILABLE");
                return;
            }

            Utils.UseLayout(true);

            // Image-only page: useOcr false keeps the scan empty.
            using (var docOff = Document.Open(inputPath))
            {
                string mdOff = docOff.ToMarkdown(showProgress: false, useOcr: false) ?? "";
                File.WriteAllText(outputOffPath, mdOff, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                Console.WriteLine($"pageCount={docOff.PageCount}");
                Console.WriteLine($"layoutEnabled={Utils.LayoutEnabled}");
                Console.WriteLine($"ocrOffCharCount={mdOff.Length}");
                Console.WriteLine("=== useOcr false ===");
                Console.WriteLine(mdOff);
            }

            // Separate Document: OCR may write text spans onto the page it processes.
            using (var docOn = Document.Open(inputPath))
            {
                // forceOcr: a copier scan can fail the layout "needs OCR" heuristic.
                string mdOn = docOn.ToMarkdown(showProgress: false, useOcr: true, forceOcr: true) ?? "";
                File.WriteAllText(outputOnPath, mdOn, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                Console.WriteLine($"ocrOnCharCount={mdOn.Length}");
                Console.WriteLine("=== useOcr true ===");
                Console.WriteLine(mdOn);
            }
        }
        finally
        {
            Utils.UseLayout(prior);
        }
    }
    // doc-end:Ocr
}
