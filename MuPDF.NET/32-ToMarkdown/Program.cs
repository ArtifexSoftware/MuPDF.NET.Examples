using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.ToMarkdown;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 32-ToMarkdown");
        ToMarkdown();
    }

    /// <summary>Convert a PDF to Markdown (layout off = classic RAG path)</summary>
    static void ToMarkdown()
    {
        string input = "columns.pdf";
        string output = "columns.md";

        // Remember prior layout flag so we restore it for other examples in the same process.
        bool prior = Utils.LayoutEnabled;
        try
        {
            // false = classic extract (no layout worker required).
            Utils.UseLayout(false);

            using Document doc = new Document(input);
            string markdown = doc.ToMarkdown(showProgress: false) ?? "";

        }
        finally
        {
            Utils.UseLayout(prior);
        }
    }
}
