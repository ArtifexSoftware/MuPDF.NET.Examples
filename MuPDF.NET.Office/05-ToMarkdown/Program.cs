using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.Office;

namespace MuPDF.NET.Examples.Office.ToMarkdown;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.Office / 05-ToMarkdown");
        ToMarkdown();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ToMarkdown()
    {
        string input = ExamplePaths.OfficeInput("pier-notice.hwpx");
        string output = ExamplePaths.Output("MuPDF.NET.Office", "05-ToMarkdown", "pier-notice.md");
        string png = ExamplePaths.Output("MuPDF.NET.Office", "05-ToMarkdown", "hwpx-page.png");
        var check = new ResultCheck("MuPDF.NET.Office", "05-ToMarkdown");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");

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
        var dump = new StringBuilder();
        foreach (string line in captured.Split('\n'))
        {
            // Unlock may print a restricted/trial banner; it is not part of the listing.
            if (line.StartsWith("MuPDF.NET Office:", StringComparison.Ordinal))
                continue;
            if (line.Contains("pymupdf_layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("pymupdf-layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("greatly improved page layout", StringComparison.OrdinalIgnoreCase))
                continue;
            dump.Append(line).Append('\n');
        }
        captured = dump.ToString();
        Console.Write(captured);

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(png);

        check.Text(captured, "to-markdown.txt");
        if (File.Exists(output))
            check.Text(File.ReadAllText(output), "pier-notice.md");
        check.Finish();
    }

    // doc-start:ToMarkdown
    /// <summary>Unlock Office, then run Document.ToMarkdown on an HWPX path</summary>
    static void ToMarkdown(string inputPath, string outputPath)
    {
        // Empty / unset MUPDF_OFFICE_KEY = restricted mode (first 3 pages). Never print the key.
        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;
        MuPDFOffice.Unlock(key, fontPathAuto: true);

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
