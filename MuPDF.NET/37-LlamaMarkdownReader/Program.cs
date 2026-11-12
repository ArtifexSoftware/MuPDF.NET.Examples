using System.Text;
using MuPDF.NET.Examples.Common;
using MuPDF.NET;
namespace MuPDF.NET.Examples.Llm.LlamaMarkdownReader;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 37-LlamaMarkdownReader");
        LlamaMarkdownReader();
    }

    /// <summary>
    /// Load LlamaIndex-style documents (one per page) via PDFMarkdownReader.
    /// </summary>
    static void LlamaMarkdownReader()
    {
        string input = ExamplePaths.MuPdfNetInput("columns.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "37-LlamaMarkdownReader", "llama-docs.txt");
        var check = new ResultCheck("MuPDF.NET", "37-LlamaMarkdownReader");

        bool prior = Utils.LayoutEnabled;
        try
        {
            // Pin classic RAG markdown so the golden Expected/ stays stable.
            // (With layout available, UseLayout defaults to true and output changes.)
            Utils.UseLayout(false);

            // Reader wraps ToMarkdown per page and fills ExtraInfo (page, total_pages, metadata, …).
            var reader = new PDFMarkdownReader();
            var docs = reader.LoadData(input);

            var sb = new StringBuilder();
            sb.Append("docCount=").Append(docs.Count).Append('\n');
            for (int i = 0; i < docs.Count; i++)
            {
                var d = docs[i];
                object? page = null;
                d.ExtraInfo?.TryGetValue("page", out page);

                string text = ResultCheck.NormalizeText(d.Text ?? "");
                sb.Append("--- page ").Append(page ?? (i + 1)).Append(" ---\n");
                sb.Append("chars=").Append(text.Length).Append('\n');
                sb.Append(text);
                if (!text.EndsWith('\n'))
                    sb.Append('\n');
            }

            string report = sb.ToString();
            File.WriteAllText(output, report);
            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Llama documents: {docs.Count}");
            check.Text(report, "llama-docs.txt");
        }
        finally
        {
            Utils.UseLayout(prior);
        }

        check.Finish();
    }
}
