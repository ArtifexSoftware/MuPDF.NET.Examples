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

    /// <summary>
    /// Export an Office document to Markdown and JSON.
    /// </summary>
    static void ExportToMarkdownJson()
    {
        string input = ExamplePaths.OfficeInput("pages.docx");
        string mdOut = ExamplePaths.Output("MuPDF.NET.Office", "04-ExportToMarkdownJson", "pages.md");
        string jsonOut = ExamplePaths.Output("MuPDF.NET.Office", "04-ExportToMarkdownJson", "pages.json");
        var check = new ResultCheck("MuPDF.NET.Office", "04-ExportToMarkdownJson");

        MuPDFOffice.Unlock(OfficeLicense.KeyFromEnvironment(), fontPathAuto: true);

        // High-level Office export helpers write files directly.
        MuPDFOffice.ToMarkdown(input, mdOut);
        MuPDFOffice.ToJson(input, jsonOut);

        string md = File.ReadAllText(mdOut);
        string json = File.ReadAllText(jsonOut);
        ConsoleEx.Info($"Markdown bytes: {md.Length}, JSON bytes: {json.Length}");

        // Markdown text is stable across OS. JSON embeds OS-specific font names
        // and glyph metrics — fingerprint page size + text only.
        check.Text(md, "pages.md");
        check.Properties(OfficeJsonFingerprint.FromJson(json), "pages.summary.txt");
        check.Finish();
    }
}
