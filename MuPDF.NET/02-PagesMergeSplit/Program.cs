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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void PagesMergeSplit()
    {
        string a = ExamplePaths.MuPdfNetInput("pages-a.pdf");
        string b = ExamplePaths.MuPdfNetInput("pages-b.pdf");
        string merged = ExamplePaths.Output("MuPDF.NET", "02-PagesMergeSplit", "merged.pdf");
        string firstPage = ExamplePaths.Output("MuPDF.NET", "02-PagesMergeSplit", "first-page.pdf");
        var check = new ResultCheck("MuPDF.NET", "02-PagesMergeSplit");

        ConsoleEx.Info($"Opened: {a}");
        ConsoleEx.Info($"Opened: {b}");
        PagesMergeSplit(a, b, merged, firstPage);

        check.Properties(PdfFingerprint.FromFile(merged), "merged.summary.txt");
        check.Properties(PdfFingerprint.FromFile(firstPage), "first-page.summary.txt");
        check.Finish();
    }

    // doc-start:PagesMergeSplit
    /// <summary>Merge pages from two PDFs, then extract the first page</summary>
    static void PagesMergeSplit(string inputPathA, string inputPathB, string mergedPath, string firstPagePath)
    {
        // Open both source PDFs.
        using (var docA = Document.Open(inputPathA))
        using (var docB = Document.Open(inputPathB))
        {
            // Insert page 0 of B into A at position 1 (0-based startAt).
            docA.InsertPdf(docB, fromPage: 0, toPage: 0, startAt: 1);
            docA.Save(mergedPath);
        }

        // Split: copy only the first page of the merged file into a new document.
        using (var src = Document.Open(mergedPath))
        using (var one = new Document())
        {
            one.InsertPdf(src, fromPage: 0, toPage: 0);
            one.Save(firstPagePath);
        }
    }
    // doc-end:PagesMergeSplit
}
