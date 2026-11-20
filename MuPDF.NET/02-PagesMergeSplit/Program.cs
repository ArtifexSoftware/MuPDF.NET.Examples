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

    // doc-start:PagesMergeSplit
    /// <summary>Merge pages from two PDFs, then extract the first page</summary>
    static void PagesMergeSplit()
    {
        string a = "sample.pdf";
        string b = "Blank.pdf";
        string merged = "merged.pdf";
        string firstPage = "first-page.pdf";

        // Open both source PDFs.
        using (var docA = Document.Open(a))
        using (var docB = Document.Open(b))
        {

            // Insert page 0 of B into A at position 1 (0-based startAt).
            docA.InsertPdf(docB, fromPage: 0, toPage: 0, startAt: 1);
            docA.Save(merged);
        }

        // Split: copy only the first page of the merged file into a new document.
        using (var src = Document.Open(merged))
        using (var one = new Document())
        {
            one.InsertPdf(src, fromPage: 0, toPage: 0);
            one.Save(firstPage);
        }
    }
    // doc-end:PagesMergeSplit
}
