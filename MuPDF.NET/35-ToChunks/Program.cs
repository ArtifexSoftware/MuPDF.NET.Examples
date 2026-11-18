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

    /// <summary>
    /// Split a PDF into retrieval-ready chunks (requires pymupdf 2.1 layout worker).
    /// </summary>
    static void ToChunks()
    {
        string input = ExamplePaths.MuPdfNetInput("columns.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "35-ToChunks", "columns.chunks.txt");
        var check = new ResultCheck("MuPDF.NET", "35-ToChunks");

        bool prior = Utils.LayoutEnabled;
        try
        {
            // ToChunks is layout-only (classic extract throws NotSupportedException).
            if (!Utils.LayoutAvailable)
            {
                ConsoleEx.Info("Layout provider unavailable — ToChunks requires UseLayout=true.");
                ConsoleEx.Info("Install pymupdf 2.1 (dotnet msbuild -t:MuPDFNetSetupLayoutPython) and re-run.");
                check.Text("LAYOUT_UNAVAILABLE\n", "layout-status.txt");
                return;
            }

            Utils.UseLayout(true);
            using Document doc = new Document(input);
            ChunkedDocument chunks = doc.ToChunks(useOcr: false, maxTokens: 400, minTokens: 120);

            var sb = new StringBuilder();
            sb.AppendLine(chunks.ToString());
            sb.AppendLine();
            foreach (Chunk c in chunks)
            {
                int tokens = c.Metadata?.TokenCount ?? 0;
                int pageStart = c.Metadata?.PageStart ?? 0;
                int pageEnd = c.Metadata?.PageEnd ?? 0;
                sb.Append(c.Id).Append("  tokens=").Append(tokens)
                    .Append("  pages=").Append(pageStart).Append('-').Append(pageEnd).Append('\n');
                sb.AppendLine(c.Text ?? "");
                sb.AppendLine();
            }

            string report = sb.ToString();
            File.WriteAllText(output, report);

            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info(chunks.ToString());
            check.Text("LAYOUT_AVAILABLE\n", "layout-status.txt");
            check.Text(report, "columns.chunks.txt");
        }
        finally
        {
            Utils.UseLayout(prior);
        }

        check.Finish();
    }
}
