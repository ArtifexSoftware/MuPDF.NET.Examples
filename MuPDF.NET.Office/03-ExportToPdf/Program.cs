using MuPDF.NET.Examples.Common;
using MuPDF.NET.Office;

namespace MuPDF.NET.Examples.Office.ExportToPdf;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.Office / 03-ExportToPdf");
        ExportToPdf();
    }

    /// <summary>Export an Office document to PDF</summary>
    static void ExportToPdf()
    {
        string input = "pages.docx";
        string output = "pages.pdf";

        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;
        MuPDFOffice.Unlock(key, fontPathAuto: true);

        // Renders Office content through sodochandler into a PDF file.
        MuPDFOffice.ToPdf(input, output);

        // ToPdf embeds OS fonts (Arial vs Liberation), so SHA-256 of GetText
        // is not portable. Compare page count + collapsed per-page text.
    }
}
