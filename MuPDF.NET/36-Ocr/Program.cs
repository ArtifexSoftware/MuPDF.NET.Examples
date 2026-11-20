using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.Ocr;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 36-Ocr");
        OcrCompare();
    }

    /// <summary>Compare Markdown with OCR on vs off (layout mode required for OCR)</summary>
    static void OcrCompare()
    {
        string input = "Ocr.pdf";
        string withOcr = "ocr-on.md";
        string withoutOcr = "ocr-off.md";

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

            // With OCR: Tesseract where the pipeline decides it helps (needs tessdata).
            string mdOn;
            using (Document docOn = new Document(input))
                mdOn = docOn.ToMarkdown(showProgress: false, useOcr: true) ?? "";

        }
        finally
        {
            Utils.UseLayout(prior);
        }
    }
}
