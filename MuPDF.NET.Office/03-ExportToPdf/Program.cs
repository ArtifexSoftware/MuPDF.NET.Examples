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

    /// <summary>
    /// Export an Office document to PDF.
    /// </summary>
    static void ExportToPdf()
    {
        string input = ExamplePaths.OfficeInput("pages.docx");
        string output = ExamplePaths.Output("MuPDF.NET.Office", "03-ExportToPdf", "pages.pdf");
        var check = new ResultCheck("MuPDF.NET.Office", "03-ExportToPdf");

        MuPDFOffice.Unlock(OfficeLicense.KeyFromEnvironment(), fontPathAuto: true);

        // Renders Office content through sodochandler into a PDF file.
        MuPDFOffice.ToPdf(input, output);

        ConsoleEx.Info($"Opened: {input}");
        ConsoleEx.Info($"Wrote: {output} ({new System.IO.FileInfo(output).Length} bytes)");

        check.Properties(PdfFingerprint.FromFile(output), "pages.summary.txt");
        check.Finish();
    }
}
