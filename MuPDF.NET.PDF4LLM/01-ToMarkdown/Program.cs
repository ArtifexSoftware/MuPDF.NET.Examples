using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.PDF4LLM.ToMarkdown;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 01-ToMarkdown");
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
        bool prior = Utils.LayoutEnabled;
        try
        {
            // false = MuPdfRag markdown (no pymupdf-layout required).
            Utils.UseLayout(false);

            using Document doc = new Document(input);
            string markdown = doc.ToMarkdown(showProgress: false) ?? "";
            File.WriteAllText(output, markdown);

            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Markdown length: {markdown.Length}");
            check.Text(markdown, "columns.md");
        }
        finally
        {
            Utils.UseLayout(prior);
        }

        check.Finish();
    }
}
