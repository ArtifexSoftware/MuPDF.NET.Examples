using MuPDF.NET.Examples.Common;
using MuPDF.NET.PDF4LLM;

namespace MuPDF.NET.Examples.PDF4LLM.MarkdownToPdf;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.PDF4LLM / 08-MarkdownToPdf");
        MarkdownToPdf();
    }

    /// <summary>
    /// Render a Markdown file to PDF (Story).
    /// </summary>
    static void MarkdownToPdf()
    {
        string input = ExamplePaths.Pdf4LlmInput("sample.md");
        string output = ExamplePaths.Output("MuPDF.NET.PDF4LLM", "08-MarkdownToPdf", "sample.pdf");
        var check = new ResultCheck("MuPDF.NET.PDF4LLM", "08-MarkdownToPdf");

        // When outputPath is set, the PDF is written to disk (return value is null).
        MuPDF4LLM.MarkdownToPdf(input, outputPath: output);

        ConsoleEx.Info($"Markdown: {input}");
        ConsoleEx.Info($"Wrote PDF: {output}");
        check.Properties(PdfFingerprint.FromFile(output), "sample.summary.txt");
        check.Finish();
    }
}
