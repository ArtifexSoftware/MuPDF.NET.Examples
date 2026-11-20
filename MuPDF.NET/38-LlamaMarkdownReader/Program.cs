using System.Text;
using MuPDF.NET.Examples.Common;
using MuPDF.NET;
namespace MuPDF.NET.Examples.Llm.LlamaMarkdownReader;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 38-LlamaMarkdownReader");
        LlamaMarkdownReader();
    }

    /// <summary>Load LlamaIndex-style documents (one per page) via Utils.LlamaMarkdownReader</summary>
    static void LlamaMarkdownReader()
    {
        string input = "columns.pdf";
        string output = "llama-docs.txt";

        bool prior = Utils.LayoutEnabled;
        try
        {
            // (With layout available, UseLayout defaults to true and output changes.)
            Utils.UseLayout(false);

            // Reader wraps ToMarkdown per page and fills ExtraInfo (page, total_pages, metadata, …).
            var reader = Utils.LlamaMarkdownReader();
            var docs = reader.LoadData(input);

            for (int i = 0; i < docs.Count; i++)
            {
                var d = docs[i];
                object? page = null;
                d.ExtraInfo?.TryGetValue("page", out page);

            }

        }
        finally
        {
            Utils.UseLayout(prior);
        }
    }
}
