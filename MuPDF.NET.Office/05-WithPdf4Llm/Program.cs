using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.Office;

namespace MuPDF.NET.Examples.Office.WithPdf4Llm;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.Office / 05-WithPdf4Llm");
        WithPdf4Llm();
    }

    /// <summary>
    /// Unlock Office, then run <see cref="Document.ToMarkdown"/> on an HWPX path.
    /// </summary>
    static void WithPdf4Llm()
    {
        string input = ExamplePaths.OfficeInput("sample.hwpx");
        string output = ExamplePaths.Output("MuPDF.NET.Office", "05-WithPdf4Llm", "sample.hwpx.md");
        var check = new ResultCheck("MuPDF.NET.Office", "05-WithPdf4Llm");

        // Office unlock registers the document handler so Document can open HWPX/DOCX paths.
        MuPDFOffice.Unlock(OfficeLicense.KeyFromEnvironment(), fontPathAuto: true);

        bool prior = Utils.LayoutEnabled;
        try
        {
            Utils.UseLayout(false);
            using Document doc = new Document(input);
            string markdown = doc.ToMarkdown(showProgress: false) ?? "";
            // Always write UTF-8 (no BOM) so Output/ is readable on every OS.
            File.WriteAllText(output, markdown, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

            ConsoleEx.Info($"Opened via Office unlock: {input}");
            ConsoleEx.Info($"Markdown length: {markdown.Length}");
            ConsoleEx.Info(
                OfficeTextShape.ContainsHangul(markdown)
                    ? "Hangul text extracted."
                    : "Hangul not in Unicode form (common on Windows when SmartOffice falls back to non-CJK fonts); using shape baseline.");

            // Full markdown differs by OS font/ToUnicode coverage. Shape maps Hangul,
            // U+FFFD, and NUL to H so Windows and Linux share one Expected file.
            check.Text(OfficeTextShape.FromText(markdown), "sample.shape.txt");
        }
        finally
        {
            Utils.UseLayout(prior);
        }

        check.Finish();
    }
}
