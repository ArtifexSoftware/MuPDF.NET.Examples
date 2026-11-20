using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.ToChunks;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 35-ToChunks");
        ToChunks();
    }

    /// <summary>Split a PDF into retrieval-ready chunks (requires pymupdf 2.1 layout worker)</summary>
    static void ToChunks()
    {
        string input = "columns.pdf";
        string output = "columns.chunks.txt";

        bool prior = Utils.LayoutEnabled;
        try
        {
            // ToChunks is layout-only (classic extract throws NotSupportedException).
            if (!Utils.LayoutAvailable)
            {
                return;
            }

            Utils.UseLayout(true);
            using Document doc = new Document(input);
            ChunkedDocument chunks = doc.ToChunks(useOcr: false, maxTokens: 400, minTokens: 120);

            foreach (Chunk c in chunks)
            {
                int tokens = c.Metadata?.TokenCount ?? 0;
                int pageStart = c.Metadata?.PageStart ?? 0;
                int pageEnd = c.Metadata?.PageEnd ?? 0;
            }

        }
        finally
        {
            Utils.UseLayout(prior);
        }
    }
}
