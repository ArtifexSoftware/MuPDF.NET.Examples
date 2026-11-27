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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ToChunks()
    {
        string input = ExamplePaths.MuPdfNetInput("bulletin.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "35-ToChunks", "bulletin.chunks.txt");
        string png = ExamplePaths.Output("MuPDF.NET", "35-ToChunks", "page.png");
        var check = new ResultCheck("MuPDF.NET", "35-ToChunks");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(png);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                ToChunks(input, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        var dump = new StringBuilder();
        foreach (string line in captured.Split('\n'))
        {
            if (line.Contains("pymupdf-layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("pymupdf_layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("greatly improved page layout", StringComparison.OrdinalIgnoreCase))
                continue;
            dump.Append(line).Append('\n');
        }

        check.Text(dump.ToString(), "to-chunks.txt");
        if (File.Exists(output))
            check.Text(File.ReadAllText(output), "bulletin.chunks.txt");
        check.FileSha256(png, "page.png.sha256");
        check.Finish();
    }

    // doc-start:ToChunks
    /// <summary>Split a PDF into retrieval-ready chunks (requires the optional layout worker)</summary>
    static void ToChunks(string inputPath, string outputPath)
    {
        // Remember prior layout flag so we restore it for other examples in the same process.
        bool prior = MuPDF4LLM.LayoutEnabled;
        try
        {
            // ToChunks requires UseLayout=true and a registered layout worker.
            if (!MuPDF4LLM.LayoutAvailable)
            {
                Console.WriteLine("LAYOUT_UNAVAILABLE");
                return;
            }

            MuPDF4LLM.SetLayout(true);

            using var doc = Document.Open(inputPath);
            ChunkedDocument chunks = doc.ToChunks(useOcr: false, maxTokens: 400, minTokens: 120);

            var listing = new StringBuilder();
            listing.AppendLine($"pageCount={doc.PageCount}");
            listing.AppendLine($"layoutEnabled={MuPDF4LLM.LayoutEnabled}");
            listing.AppendLine($"chunkCount={chunks.Count}");
            foreach (Chunk c in chunks)
            {
                int tokens = c.Metadata?.TokenCount ?? 0;
                int pageStart = c.Metadata?.PageStart ?? 0;
                int pageEnd = c.Metadata?.PageEnd ?? 0;
                listing.AppendLine();
                listing.AppendLine($"{c.Id} tokens={tokens} pages={pageStart}-{pageEnd}");
                listing.AppendLine(c.Text ?? "");
            }

            string text = listing.ToString();
            File.WriteAllText(outputPath, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            Console.Write(text);
        }
        finally
        {
            MuPDF4LLM.SetLayout(prior);
        }
    }
    // doc-end:ToChunks
}
