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

    /// <summary>
    /// Unlock Office, then open DOCX and HWPX via Document.Open.
    /// </summary>
    static void OpenHwpxDocx()
    {
        var check = new ResultCheck("MuPDF.NET.Office", "02-OpenHwpxDocx");

        // Must unlock before opening Office formats.
        MuPDFOffice.Unlock(OfficeLicense.KeyFromEnvironment(), fontPathAuto: true);

        string docx = ExamplePaths.OfficeInput("pages.docx");
        string hwpx = ExamplePaths.OfficeInput("sample.hwpx");
        int docxPages;
        int hwpxPages;

        using (var d1 = Document.Open(docx))
        {
            docxPages = d1.PageCount;
            ConsoleEx.Info($"DOCX pages: {docxPages}");
        }

        using (var d2 = Document.Open(hwpx))
        {
            hwpxPages = d2.PageCount;
            ConsoleEx.Info($"HWPX pages: {hwpxPages}");
        }

        check.Properties(
            new Dictionary<string, string>
            {
                ["docxPages"] = docxPages.ToString(),
                ["hwpxPages"] = hwpxPages.ToString(),
            },
            "pages.summary.txt");

        check.Finish();
    }
}
