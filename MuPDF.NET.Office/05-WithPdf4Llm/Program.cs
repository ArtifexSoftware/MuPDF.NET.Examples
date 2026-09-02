using System.Text;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.Office;
using MuPDF.NET.PDF4LLM;

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
    /// Unlock Office, then run MuPDF4LLM.ToMarkdown on an HWPX path.
    /// </summary>
    static void WithPdf4Llm()
    {
        string input = ExamplePaths.OfficeInput("sample.hwpx");
        string output = ExamplePaths.Output("MuPDF.NET.Office", "05-WithPdf4Llm", "sample.hwpx.md");
        var check = new ResultCheck("MuPDF.NET.Office", "05-WithPdf4Llm");

        // Office unlock registers the document handler so PDF4LLM can open HWPX/DOCX paths.
        MuPDFOffice.Unlock(OfficeLicense.KeyFromEnvironment(), fontPathAuto: true);

        bool prior = MuPDF4LLM.UseLayout;
        try
        {
            MuPDF4LLM.SetUseLayout(false);
            string markdown = MuPDF4LLM.ToMarkdown(input, showProgress: false) ?? "";
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
            MuPDF4LLM.SetUseLayout(prior);
        }

        check.Finish();
    }
}
