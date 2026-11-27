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

    // help:skip — resolves fixtures, calls the documented overloads, then checks the harness baseline.
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

        bool prior = Utils.LayoutEnabled;
        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                // false = classic extract (no layout worker required).
                Utils.UseLayout(false);
                LlamaMarkdownReaderDefault(input, output);
                LlamaMarkdownReaderMetaFilter(input);
                LlamaMarkdownReaderExtraInfo(input);
                LlamaMarkdownReaderLoadKwargs(input);
            }
            finally
            {
                Console.SetOut(previous);
                Utils.UseLayout(prior);
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

    // doc-start:LlamaMarkdownReaderDefault
    /// <summary>Load LlamaIndex-style documents (one per page) via Utils.LlamaMarkdownReader</summary>
    static void LlamaMarkdownReaderDefault(string inputPath, string outputPath)
    {
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
    // doc-end:LlamaMarkdownReaderDefault

    // doc-start:LlamaMarkdownReaderMetaFilter
    /// <summary>Rewrite ExtraInfo with the metaFilter callback</summary>
    static void LlamaMarkdownReaderMetaFilter(string inputPath)
    {
        // metaFilter runs after built-in keys (page, total_pages, file_path, PDF metadata).
        var reader = Utils.LlamaMarkdownReader(metaFilter: extra =>
        {
            extra.Remove("file_path");
            extra["yard"] = "North Pier";
            return extra;
        });
        var docs = reader.LoadData(inputPath);

        object? page = null;
        object? yard = null;
        docs[0].ExtraInfo?.TryGetValue("page", out page);
        docs[0].ExtraInfo?.TryGetValue("yard", out yard);
        bool hasFilePath = docs[0].ExtraInfo != null && docs[0].ExtraInfo.ContainsKey("file_path");
        Console.WriteLine("--- metaFilter ---");
        Console.WriteLine($"page={page} yard={yard} hasFilePath={hasFilePath}");
    }
    // doc-end:LlamaMarkdownReaderMetaFilter

    // doc-start:LlamaMarkdownReaderExtraInfo
    /// <summary>Merge your own keys into every page ExtraInfo</summary>
    static void LlamaMarkdownReaderExtraInfo(string inputPath)
    {
        var extraInfo = new Dictionary<string, object>
        {
            ["yard"] = "North Pier",
            ["issue"] = 12,
        };
        var reader = Utils.LlamaMarkdownReader();
        var docs = reader.LoadData(inputPath, extraInfo);

        object? yard = null;
        object? issue = null;
        docs[0].ExtraInfo?.TryGetValue("yard", out yard);
        docs[0].ExtraInfo?.TryGetValue("issue", out issue);
        Console.WriteLine("--- extraInfo ---");
        Console.WriteLine($"yard={yard} issue={issue}");
    }
    // doc-end:LlamaMarkdownReaderExtraInfo

    // doc-start:LlamaMarkdownReaderLoadKwargs
    /// <summary>Forward loadKwargs to the Markdown extractor</summary>
    static void LlamaMarkdownReaderLoadKwargs(string inputPath)
    {
        // table_output: "markdown" (default) or "html". Used on the classic extract path.
        var loadKwargs = new Dictionary<string, object>
        {
            ["table_output"] = "html",
            ["show_progress"] = false,
        };
        var reader = Utils.LlamaMarkdownReader();
        var docs = reader.LoadData(inputPath, loadKwargs: loadKwargs);

        string page2 = docs[1].Text ?? "";
        bool htmlTable = page2.Contains("<table", StringComparison.OrdinalIgnoreCase);
        Console.WriteLine("--- loadKwargs ---");
        Console.WriteLine($"table_output=html htmlTable={htmlTable}");
        foreach (string line in page2.Replace("\r\n", "\n").Split('\n'))
        {
            if (line.Contains("<table", StringComparison.OrdinalIgnoreCase)
                || line.Contains("<tr", StringComparison.OrdinalIgnoreCase)
                || line.Contains("</table", StringComparison.OrdinalIgnoreCase))
                Console.WriteLine(line);
        }
    }
    // doc-end:LlamaMarkdownReaderLoadKwargs
}
