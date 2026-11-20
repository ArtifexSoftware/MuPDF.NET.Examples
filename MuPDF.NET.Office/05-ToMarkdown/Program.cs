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
        ConsoleEx.Title("MuPDF.NET.Office / 05-ToMarkdown()");
        ToMarkdown();
    }

    /// <summary>Unlock Office, then run &lt;see cref="Document.ToMarkdown"/&gt; on an HWPX path</summary>
    static void ToMarkdown()
    {
        string input = "sample.hwpx";
        string output = "sample.hwpx.md";

        // Office unlock registers the document handler so Document can open HWPX/DOCX paths.
        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;
        MuPDFOffice.Unlock(key, fontPathAuto: true);

        bool prior = Utils.LayoutEnabled;
        try
        {
            Utils.UseLayout(false);
            using Document doc = new Document(input);
            string markdown = doc.ToMarkdown(showProgress: false) ?? "";
            // Always write UTF-8 (no BOM) so Output/ is readable on every OS.

            // Full markdown differs by OS font/ToUnicode coverage. Shape maps Hangul,
            // U+FFFD, and NUL to H so Windows and Linux share one Expected file.
        }
        finally
        {
            Utils.UseLayout(prior);
        }
    }
}
