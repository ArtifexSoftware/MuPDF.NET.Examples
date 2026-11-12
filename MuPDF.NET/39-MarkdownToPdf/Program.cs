using MuPDF.NET.Examples.Common;
using MuPDF.NET;
namespace MuPDF.NET.Examples.Llm.MarkdownToPdf;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 39-MarkdownToPdf");
        MarkdownToPdf();
    }

    /// <summary>
    /// Render a Markdown file to PDF (Story).
    /// </summary>
    static void MarkdownToPdf()
    {
        string input = ExamplePaths.MuPdfNetInput("sample.md");
        string output = ExamplePaths.Output("MuPDF.NET", "39-MarkdownToPdf", "sample.pdf");
        var check = new ResultCheck("MuPDF.NET", "39-MarkdownToPdf");

        // When outputPath is set, the PDF is written to disk (return value is null).
        Utils.MarkdownToPdf(input, outputPath: output);

        ConsoleEx.Info($"Markdown: {input}");
        ConsoleEx.Info($"Wrote PDF: {output}");
        check.Properties(PdfFingerprint.FromFile(output), "sample.summary.txt");
        check.Finish();
    }
}
