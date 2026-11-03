using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.PDF4LLM.Ocr;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 04-Ocr");
        OcrCompare();
    }

    /// <summary>
    /// Compare Markdown with OCR on vs off (layout mode required for OCR).
    /// </summary>
    static void OcrCompare()
    {
        string input = ExamplePaths.Pdf4LlmInput("Ocr.pdf");
        string withOcr = ExamplePaths.Output("MuPDF.NET.PDF4LLM", "04-Ocr", "ocr-on.md");
        string withoutOcr = ExamplePaths.Output("MuPDF.NET.PDF4LLM", "04-Ocr", "ocr-off.md");
        var check = new ResultCheck("MuPDF.NET.PDF4LLM", "04-Ocr");

        bool prior = Utils.LayoutEnabled;
        try
        {
            // OCR is only implemented in the layout pipeline (DocumentLayout).
            // With UseLayout=false, useOcr is ignored and scanned pages stay empty.
            Utils.UseLayout(true);

            // Without OCR: only embedded text.
            string mdOff;
            using (Document docOff = new Document(input))
                mdOff = docOff.ToMarkdown(showProgress: false, useOcr: false) ?? "";
            File.WriteAllText(withoutOcr, mdOff);

            // With OCR: Tesseract where the pipeline decides it helps (needs tessdata).
            string mdOn;
            using (Document docOn = new Document(input))
                mdOn = docOn.ToMarkdown(showProgress: false, useOcr: true) ?? "";
            File.WriteAllText(withOcr, mdOn);

            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Markdown length useOcr=false: {mdOff.Length}");
            ConsoleEx.Info($"Markdown length useOcr=true:  {mdOn.Length}");

            check.Text(mdOff, "ocr-off.md");
            check.Text(mdOn, "ocr-on.md");
        }
        finally
        {
            Utils.UseLayout(prior);
        }

        check.Finish();
    }
}
