using MuPDF.NET;
using MuPDF.NET.Examples.Common;
using System.Text;

namespace MuPDF.NET.Examples.Llm.ToMarkdown;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 32-ToMarkdown");
        ToMarkdown();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ToMarkdown()
    {
        string input = ExamplePaths.MuPdfNetInput("bulletin.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "32-ToMarkdown", "to-markdown.md");
        string png = ExamplePaths.Output("MuPDF.NET", "32-ToMarkdown", "page.png");
        var check = new ResultCheck("MuPDF.NET", "32-ToMarkdown");
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
                ToMarkdown(input, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        // ToMarkdown may print a layout-package hint on stdout; the baseline is our dump only.
        var dump = new StringBuilder();
        foreach (string line in captured.Split('\n'))
        {
            if (line.Contains("pymupdf_layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("greatly improved page layout", StringComparison.OrdinalIgnoreCase))
                continue;
            dump.Append(line).Append('\n');
        }

        check.Text(dump.ToString(), "to-markdown.txt");
        check.Text(File.ReadAllText(output), "to-markdown.md");
        check.FileSha256(png, "page.png.sha256");
        check.Finish();
    }

    // doc-start:ToMarkdown
    /// <summary>Convert a PDF to Markdown (classic extract, layout off)</summary>
    static void ToMarkdown(string inputPath, string outputPath)
    {
        // Remember prior layout flag so we restore it for other examples in the same process.
        bool prior = MuPDF4LLM.LayoutEnabled;
        try
        {
            // false = classic extract (no layout worker required).
            MuPDF4LLM.SetLayout(false);

            using var doc = Document.Open(inputPath);
            string markdown = doc.ToMarkdown(showProgress: false, useOcr: false) ?? "";
            File.WriteAllText(outputPath, markdown, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            Console.WriteLine($"pageCount={doc.PageCount}");
            Console.WriteLine($"layoutEnabled={MuPDF4LLM.LayoutEnabled}");
            Console.WriteLine($"charCount={markdown.Length}");
            Console.WriteLine(markdown);
        }
        finally
        {
            MuPDF4LLM.SetLayout(prior);
        }
    }
    // doc-end:ToMarkdown
}
