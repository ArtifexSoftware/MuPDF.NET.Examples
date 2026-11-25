using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.LlamaMarkdownReader;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 38-LlamaMarkdownReader");
        LlamaMarkdownReader();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void LlamaMarkdownReader()
    {
        string input = ExamplePaths.MuPdfNetInput("llama-pages.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "38-LlamaMarkdownReader", "llama-docs.txt");
        string png1 = ExamplePaths.Output("MuPDF.NET", "38-LlamaMarkdownReader", "page-1.png");
        string png2 = ExamplePaths.Output("MuPDF.NET", "38-LlamaMarkdownReader", "page-2.png");
        var check = new ResultCheck("MuPDF.NET", "38-LlamaMarkdownReader");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        {
            using (Page page = doc[0])
            using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
                pix.Save(png1);
            using (Page page = doc[1])
            using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
                pix.Save(png2);
        }

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                LlamaMarkdownReader(input, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        var dump = new StringBuilder();
        foreach (string line in captured.Split('\n'))
        {
            if (line.StartsWith("Consider using the pymupdf_layout", StringComparison.Ordinal)
                || line.Contains("pymupdf-layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("greatly improved page layout", StringComparison.OrdinalIgnoreCase))
                continue;
            dump.Append(line).Append('\n');
        }
        captured = dump.ToString();
        Console.Write(captured);

        check.Text(captured, "llama-reader.txt");
        if (File.Exists(output))
            check.Text(File.ReadAllText(output), "llama-docs.txt");
        check.FileSha256(png1, "page-1.png.sha256");
        check.FileSha256(png2, "page-2.png.sha256");
        check.Finish();
    }

    // doc-start:LlamaMarkdownReader
    /// <summary>Load LlamaIndex-style documents (one per page) via Utils.LlamaMarkdownReader</summary>
    static void LlamaMarkdownReader(string inputPath, string outputPath)
    {
        // Remember prior layout flag so we restore it for other examples in the same process.
        bool prior = Utils.LayoutEnabled;
        try
        {
            // false = classic extract (no layout worker required).
            Utils.UseLayout(false);

            // Reader wraps ToMarkdown per page and fills ExtraInfo (page, total_pages, metadata).
            var reader = Utils.LlamaMarkdownReader();
            var docs = reader.LoadData(inputPath);

            var listing = new StringBuilder();
            listing.AppendLine($"docCount={docs.Count}");
            listing.AppendLine($"layoutEnabled={Utils.LayoutEnabled}");
            foreach (var d in docs)
            {
                object? page = null;
                object? total = null;
                d.ExtraInfo?.TryGetValue("page", out page);
                d.ExtraInfo?.TryGetValue("total_pages", out total);
                listing.AppendLine();
                listing.AppendLine($"--- page {page} / {total} ---");
                listing.AppendLine(d.Text ?? "");
            }

            string text = listing.ToString();
            File.WriteAllText(outputPath, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            Console.Write(text);
        }
        finally
        {
            Utils.UseLayout(prior);
        }
    }
    // doc-end:LlamaMarkdownReader
}
