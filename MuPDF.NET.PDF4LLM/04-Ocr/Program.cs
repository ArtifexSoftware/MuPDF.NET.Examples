using MuPDF.NET.Examples.Common;
using MuPDF.NET.PDF4LLM;

namespace MuPDF.NET.Examples.PDF4LLM.Ocr;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.PDF4LLM / 04-Ocr");
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

        bool prior = MuPDF4LLM.UseLayout;
        try
        {
            // OCR is only implemented in the layout pipeline (DocumentLayout).
            // With UseLayout=false, useOcr is ignored and scanned pages stay empty.
            MuPDF4LLM.SetUseLayout(true);

            // Without OCR: only embedded text.
            string mdOff = MuPDF4LLM.ToMarkdown(input, showProgress: false, useOcr: false) ?? "";
            File.WriteAllText(withoutOcr, mdOff);

            // With OCR: Tesseract where the pipeline decides it helps (needs tessdata).
            string mdOn = MuPDF4LLM.ToMarkdown(input, showProgress: false, useOcr: true) ?? "";
            File.WriteAllText(withOcr, mdOn);

            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Markdown length useOcr=false: {mdOff.Length}");
            ConsoleEx.Info($"Markdown length useOcr=true:  {mdOn.Length}");

            check.Text(mdOff, "ocr-off.md");
            check.Text(mdOn, "ocr-on.md");
        }
        finally
        {
            MuPDF4LLM.SetUseLayout(prior);
        }

        check.Finish();
    }
}
