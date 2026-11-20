using MuPDF.NET.Examples.Common;
using MuPDF.NET;
namespace MuPDF.NET.Examples.Llm.MarkdownToPdf;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 40-MarkdownToPdf");
        MarkdownToPdf();
    }

    /// <summary>Render a Markdown file to PDF (Story)</summary>
    static void MarkdownToPdf()
    {
        string input = "sample.md";
        string output = "sample.pdf";

        // When outputPath is set, the PDF is written to disk (return value is null).
        Utils.MarkdownToPdf(input, outputPath: output);
    }
}
