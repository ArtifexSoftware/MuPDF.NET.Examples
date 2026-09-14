using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.PagesMergeSplit;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 02-PagesMergeSplit");
        PagesMergeSplit();
    }

    /// <summary>
    /// Merge pages from two PDFs, then extract the first page.
    /// </summary>
    static void PagesMergeSplit()
    {
        string a = ExamplePaths.MuPdfNetInput("sample.pdf");
        string b = ExamplePaths.MuPdfNetInput("Blank.pdf");
        string merged = ExamplePaths.Output("MuPDF.NET", "02-PagesMergeSplit", "merged.pdf");
        string firstPage = ExamplePaths.Output("MuPDF.NET", "02-PagesMergeSplit", "first-page.pdf");
        var check = new ResultCheck("MuPDF.NET", "02-PagesMergeSplit");

        // Open both source PDFs.
        using (var docA = Document.Open(a))
        using (var docB = Document.Open(b))
        {
            ConsoleEx.Info($"A pages={docA.PageCount}, B pages={docB.PageCount}");

            // Insert page 0 of B into A at position 1 (0-based startAt).
            docA.InsertPdf(docB, fromPage: 0, toPage: 0, startAt: 1);
            docA.Save(merged);
            ConsoleEx.Info($"Merged page count: {docA.PageCount}");
        }

        // Split: copy only the first page of the merged file into a new document.
        using (var src = Document.Open(merged))
        using (var one = new Document())
        {
            one.InsertPdf(src, fromPage: 0, toPage: 0);
            one.Save(firstPage);
        }

        // Examples harness baselines.
        check.Properties(PdfFingerprint.FromFile(merged), "merged.summary.txt");
        check.Properties(PdfFingerprint.FromFile(firstPage), "first-page.summary.txt");
        check.Finish();
    }
}
