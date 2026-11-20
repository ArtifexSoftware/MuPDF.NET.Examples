using MuPDF.NET;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.Office;

namespace MuPDF.NET.Examples.Office.OpenHwpxDocx;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.Office / 02-OpenHwpxDocx");
        OpenHwpxDocx();
    }

    /// <summary>Unlock Office, then open DOCX and HWPX via Document.Open</summary>
    static void OpenHwpxDocx()
    {
        // Must unlock before opening Office formats.
        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;
        MuPDFOffice.Unlock(key, fontPathAuto: true);

        string docx = "pages.docx";
        string hwpx = "sample.hwpx";
        int docxPages;
        int hwpxPages;

        using (var d1 = Document.Open(docx))
        {
            docxPages = d1.PageCount;
        }

        using (var d2 = Document.Open(hwpx))
        {
            hwpxPages = d2.PageCount;
        }
    }
}
