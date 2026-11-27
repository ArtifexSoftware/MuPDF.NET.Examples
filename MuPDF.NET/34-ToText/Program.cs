using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.ToText;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 34-ToText");
        ToText();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ToText()
    {
        string input = ExamplePaths.MuPdfNetInput("bulletin.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "34-ToText", "bulletin.txt");
        string png = ExamplePaths.Output("MuPDF.NET", "34-ToText", "page.png");
        var check = new ResultCheck("MuPDF.NET", "34-ToText");
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
                ToText(input, output);
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

        check.Text(dump.ToString(), "to-text.txt");
        if (File.Exists(output))
            check.Text(File.ReadAllText(output), "bulletin.txt");
        check.FileSha256(png, "page.png.sha256");
        check.Finish();
    }

    // doc-start:ToText
    /// <summary>Convert a PDF to plain text (requires the optional layout worker)</summary>
    static void ToText(string inputPath, string outputPath)
    {
        // Remember prior layout flag so we restore it for other examples in the same process.
        bool prior = MuPDF4LLM.LayoutEnabled;
        try
        {
            // ToText requires UseLayout=true and a registered layout worker.
            if (!MuPDF4LLM.LayoutAvailable)
            {
                Console.WriteLine("LAYOUT_UNAVAILABLE");
                return;
            }

            MuPDF4LLM.SetLayout(true);

            using var doc = Document.Open(inputPath);
            string text = doc.ToText(showProgress: false, useOcr: false) ?? "";
            File.WriteAllText(outputPath, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            Console.WriteLine($"pageCount={doc.PageCount}");
            Console.WriteLine($"layoutEnabled={MuPDF4LLM.LayoutEnabled}");
            Console.WriteLine($"charCount={text.Length}");
            Console.WriteLine(text);
        }
        finally
        {
            MuPDF4LLM.SetLayout(prior);
        }
    }
    // doc-end:ToText
}
