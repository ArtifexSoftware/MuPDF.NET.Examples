using System.Globalization;
using System.Text;
using System.Text.Json;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.Office;

namespace MuPDF.NET.Examples.Office.ExportToMarkdownJson;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.Office / 04-ExportToMarkdownJson");
        ExportToMarkdownJson();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ExportToMarkdownJson()
    {
        string input = ExamplePaths.OfficeInput("pier-yard.docx");
        string markdownPath = ExamplePaths.Output("MuPDF.NET.Office", "04-ExportToMarkdownJson", "pier-yard.md");
        string jsonPath = ExamplePaths.Output("MuPDF.NET.Office", "04-ExportToMarkdownJson", "pier-yard.json");
        string png = ExamplePaths.Output("MuPDF.NET.Office", "04-ExportToMarkdownJson", "docx-page.png");
        var check = new ResultCheck("MuPDF.NET.Office", "04-ExportToMarkdownJson");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                ExportToMarkdownJson(input, markdownPath, jsonPath);
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
            dump.Append(line).Append('\n');
        }
        captured = dump.ToString();
        Console.Write(captured);

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(png);

        check.Text(captured, "export-dump.txt");
        if (File.Exists(markdownPath))
            check.Text(File.ReadAllText(markdownPath), "pier-yard.md");
        const string jsonMark = "--- json ---\n";
        int jsonAt = captured.IndexOf(jsonMark, StringComparison.Ordinal);
        if (jsonAt >= 0)
            check.Text(captured[(jsonAt + jsonMark.Length)..], "json-summary.txt");
        if (File.Exists(jsonPath))
            check.Properties(OfficeJsonFingerprint.FromJson(File.ReadAllText(jsonPath)), "json.summary.txt");
        check.Finish();
    }

    // doc-start:ExportToMarkdownJson
    /// <summary>Export an Office document to Markdown and JSON</summary>
    static void ExportToMarkdownJson(string inputPath, string markdownPath, string jsonPath)
    {
        // Empty / unset MUPDF_OFFICE_KEY = restricted mode (first 3 pages). Never print the key.
        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;
        MuPDFOffice.Unlock(key, fontPathAuto: true);

        // When the output path is set, these write the file and return null.
        MuPDFOffice.ToMarkdown(inputPath, markdownPath);
        MuPDFOffice.ToJson(inputPath, jsonPath);

        string markdown = File.ReadAllText(markdownPath);
        string json = File.ReadAllText(jsonPath);

        Console.WriteLine("--- markdown ---");
        Console.Write(markdown);
        if (!markdown.EndsWith('\n'))
            Console.WriteLine();

        // Full JSON embeds OS font names and glyph metrics — dump type + text only.
        Console.WriteLine("--- json ---");
        WriteJsonSummary(json);
    }

    static void WriteJsonSummary(string json)
    {
        using var parsed = JsonDocument.Parse(string.IsNullOrWhiteSpace(json) ? "{}" : json);
        JsonElement root = parsed.RootElement;
        if (!root.TryGetProperty("pages", out JsonElement pages) ||
            pages.ValueKind != JsonValueKind.Array)
        {
            Console.WriteLine("jsonPageCount=0");
            return;
        }

        Console.WriteLine($"jsonPageCount={pages.GetArrayLength()}");
        if (pages.GetArrayLength() > 0)
        {
            JsonElement page = pages[0];
            if (page.TryGetProperty("width", out JsonElement width))
                Console.WriteLine($"jsonPageWidth={FormatNumber(width)}");
            if (page.TryGetProperty("height", out JsonElement height))
                Console.WriteLine($"jsonPageHeight={FormatNumber(height)}");
        }

        int boxCount = 0;
        int pathCount = 0;
        var texts = new List<string>();
        foreach (JsonElement page in pages.EnumerateArray())
        {
            if (!page.TryGetProperty("blocks", out JsonElement blocks) ||
                blocks.ValueKind != JsonValueKind.Array)
                continue;
            foreach (JsonElement block in blocks.EnumerateArray())
            {
                string type = block.TryGetProperty("type", out JsonElement typeEl)
                    ? typeEl.GetString() ?? ""
                    : "";
                if (type == "box")
                {
                    boxCount++;
                    continue;
                }
                if (type == "path")
                {
                    pathCount++;
                    continue;
                }
                string text = "";
                if (block.TryGetProperty("text", out JsonElement textEl))
                    text = CollapseText(textEl.GetString());
                texts.Add(text.Length > 0 ? text : type);
            }
        }
        Console.WriteLine($"jsonBoxCount={boxCount}");
        Console.WriteLine($"jsonPathCount={pathCount}");
        Console.WriteLine($"jsonTextCount={texts.Count}");
        for (int i = 0; i < texts.Count; i++)
            Console.WriteLine($"jsonText{i}={texts[i]}");
    }

    static string FormatNumber(JsonElement el)
    {
        if (el.ValueKind == JsonValueKind.Number && el.TryGetDouble(out double d))
            return d.ToString("0.0", CultureInfo.InvariantCulture);
        return el.ToString();
    }

    static string CollapseText(string? value)
    {
        string s = value ?? "";
        var sb = new StringBuilder(s.Length);
        bool space = false;
        foreach (char c in s)
        {
            if (char.IsWhiteSpace(c))
            {
                space = true;
                continue;
            }
            if (space && sb.Length > 0)
                sb.Append(' ');
            space = false;
            sb.Append(c);
        }
        return sb.ToString();
    }
    // doc-end:ExportToMarkdownJson
}
