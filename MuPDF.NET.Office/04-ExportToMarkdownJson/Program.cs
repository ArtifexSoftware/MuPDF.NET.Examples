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

    /// <summary>Export an Office document to Markdown and JSON</summary>
    static void ExportToMarkdownJson()
    {
        string input = "pages.docx";
        string mdOut = "pages.md";
        string jsonOut = "pages.json";

        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;
        MuPDFOffice.Unlock(key, fontPathAuto: true);

        // High-level Office export helpers write files directly.
        MuPDFOffice.ToMarkdown(input, mdOut);
        MuPDFOffice.ToJson(input, jsonOut);

        string md = File.ReadAllText(mdOut);
        string json = File.ReadAllText(jsonOut);

        // Markdown text is stable across OS. JSON embeds OS-specific font names
        // and glyph metrics — fingerprint page size + text only.
    }
}
