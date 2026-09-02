using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.OpenSave;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 01-OpenSave");
        OpenSave();
    }

    /// <summary>
    /// Open a PDF and save a copy. Copy this method into your project and adjust paths.
    /// </summary>
    static void OpenSave()
    {
        // Input fixture and output path used by this examples solution.
        string input = ExamplePaths.MuPdfNetInput("sample.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "01-OpenSave", "sample-copy.pdf");
        var check = new ResultCheck("MuPDF.NET", "01-OpenSave");

        // Open the document (dispose closes native handles).
        using (var doc = Document.Open(input))
        {
            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Pages: {doc.PageCount}");

            // Write a full copy to disk.
            doc.Save(output);
        }

        // Compare against Expected/ baseline (examples harness — remove when copying).
        check.Properties(PdfFingerprint.FromFile(output), "sample-copy.summary.txt");
        check.Finish();
    }
}
