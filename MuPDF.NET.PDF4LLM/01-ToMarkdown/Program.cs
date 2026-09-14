using MuPDF.NET.Examples.Common;
using MuPDF.NET.PDF4LLM;

namespace MuPDF.NET.Examples.PDF4LLM.ToMarkdown;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.PDF4LLM / 01-ToMarkdown");
        ToMarkdown();
    }

    /// <summary>
    /// Convert a PDF to Markdown (layout off = classic RAG path).
    /// </summary>
    static void ToMarkdown()
    {
        string input = ExamplePaths.Pdf4LlmInput("columns.pdf");
        string output = ExamplePaths.Output("MuPDF.NET.PDF4LLM", "01-ToMarkdown", "columns.md");
        var check = new ResultCheck("MuPDF.NET.PDF4LLM", "01-ToMarkdown");

        // Remember prior layout flag so we restore it for other examples in the same process.
        bool prior = MuPDF4LLM.UseLayout;
        try
        {
            // false = MuPdfRag markdown (no pymupdf-layout required).
            MuPDF4LLM.SetUseLayout(false);

            string markdown = MuPDF4LLM.ToMarkdown(input, showProgress: false) ?? "";
            File.WriteAllText(output, markdown);

            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Markdown length: {markdown.Length}");
            check.Text(markdown, "columns.md");
        }
        finally
        {
            MuPDF4LLM.SetUseLayout(prior);
        }

        check.Finish();
    }
}
