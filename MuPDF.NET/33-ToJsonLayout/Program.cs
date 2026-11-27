using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.ToJsonLayout;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 33-ToJsonLayout");
        ToJsonLayout();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ToJsonLayout()
    {
        string input = ExamplePaths.MuPdfNetInput("bulletin.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "33-ToJsonLayout", "bulletin.json");
        string png = ExamplePaths.Output("MuPDF.NET", "33-ToJsonLayout", "page.png");
        var check = new ResultCheck("MuPDF.NET", "33-ToJsonLayout");
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
                ToJsonLayout(input, output);
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

        check.Text(dump.ToString(), "layout.txt");
        if (File.Exists(output))
        {
            string json = NormalizeJsonFilename(File.ReadAllText(output), input);
            File.WriteAllText(output, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            check.Text(json, "bulletin.json");
        }
        check.FileSha256(png, "page.png.sha256");
        check.Finish();
    }

    static string NormalizeJsonFilename(string json, string inputPath)
    {
        string name = Path.GetFileName(inputPath);
        return Regex.Replace(
            json,
            "\"filename\"\\s*:\\s*\"(?:\\\\.|[^\"\\\\])*\"",
            "\"filename\":\"" + name.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"",
            RegexOptions.CultureInvariant,
            TimeSpan.FromSeconds(2));
    }

    // doc-start:ToJsonLayout
    /// <summary>Convert a PDF to layout JSON (requires the optional layout worker)</summary>
    static void ToJsonLayout(string inputPath, string outputPath)
    {
        // Remember prior layout flag so we restore it for other examples in the same process.
        bool prior = MuPDF4LLM.LayoutEnabled;
        try
        {
            // ToJson requires UseLayout=true and a registered layout worker.
            if (!MuPDF4LLM.LayoutAvailable)
            {
                Console.WriteLine("LAYOUT_UNAVAILABLE");
                return;
            }

            MuPDF4LLM.SetLayout(true);

            using var doc = Document.Open(inputPath);
            string json = doc.ToJson(showProgress: false, useOcr: false) ?? "";
            File.WriteAllText(outputPath, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            Console.WriteLine($"pageCount={doc.PageCount}");
            Console.WriteLine($"layoutEnabled={MuPDF4LLM.LayoutEnabled}");
            WriteBoxes(json);
        }
        finally
        {
            MuPDF4LLM.SetLayout(prior);
        }
    }
    // doc-end:ToJsonLayout

    /// <summary>Print each layout box class and its first span so the JSON is easy to scan</summary>
    static void WriteBoxes(string json)
    {
        using var parsed = JsonDocument.Parse(string.IsNullOrWhiteSpace(json) ? "{}" : json);
        JsonElement root = parsed.RootElement;
        int boxCount = 0;
        if (root.TryGetProperty("pages", out JsonElement pages) &&
            pages.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement page in pages.EnumerateArray())
            {
                if (!page.TryGetProperty("boxes", out JsonElement boxes) ||
                    boxes.ValueKind != JsonValueKind.Array)
                    continue;
                foreach (JsonElement box in boxes.EnumerateArray())
                {
                    string boxClass = box.TryGetProperty("boxclass", out JsonElement cls)
                        ? cls.GetString() ?? ""
                        : "";
                    string text = "";
                    if (box.TryGetProperty("textlines", out JsonElement lines) &&
                        lines.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement line in lines.EnumerateArray())
                        {
                            if (!line.TryGetProperty("spans", out JsonElement spans) ||
                                spans.ValueKind != JsonValueKind.Array)
                                continue;
                            foreach (JsonElement span in spans.EnumerateArray())
                            {
                                if (span.TryGetProperty("text", out JsonElement value))
                                {
                                    text = (value.GetString() ?? "").Replace('\n', ' ').Trim();
                                    break;
                                }
                            }
                            if (text.Length > 0)
                                break;
                        }
                    }
                    Console.WriteLine($"{boxCount} {boxClass} {text}");
                    boxCount++;
                }
            }
        }
        Console.WriteLine($"boxCount={boxCount}");
    }
}
