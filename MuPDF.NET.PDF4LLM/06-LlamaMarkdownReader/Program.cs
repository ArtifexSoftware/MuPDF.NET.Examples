using System.Text;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.PDF4LLM;
using MuPDF.NET.PDF4LLM.Llama;

namespace MuPDF.NET.Examples.PDF4LLM.LlamaMarkdownReader;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.PDF4LLM / 06-LlamaMarkdownReader");
        LlamaMarkdownReader();
    }

    /// <summary>
    /// Load LlamaIndex-style documents (one per page) via PDFMarkdownReader.
    /// </summary>
    static void LlamaMarkdownReader()
    {
        string input = ExamplePaths.Pdf4LlmInput("columns.pdf");
        string output = ExamplePaths.Output("MuPDF.NET.PDF4LLM", "06-LlamaMarkdownReader", "llama-docs.txt");
        var check = new ResultCheck("MuPDF.NET.PDF4LLM", "06-LlamaMarkdownReader");

        bool prior = MuPDF4LLM.UseLayout;
        try
        {
            // Pin classic RAG markdown so the golden Expected/ stays stable.
            // (With layout available, UseLayout defaults to true and output changes.)
            MuPDF4LLM.SetUseLayout(false);

            // Reader wraps ToMarkdown per page and fills ExtraInfo (page, total_pages, metadata, …).
            var reader = new PDFMarkdownReader();
            List<LlamaIndexDocument> docs = reader.LoadData(input);

            var sb = new StringBuilder();
            sb.Append("docCount=").Append(docs.Count).Append('\n');
            for (int i = 0; i < docs.Count; i++)
            {
                LlamaIndexDocument d = docs[i];
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
            MuPDF4LLM.SetUseLayout(prior);
        }

        check.Finish();
    }
}
