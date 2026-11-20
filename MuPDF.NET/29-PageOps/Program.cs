using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.PageOps;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 29-PageOps");
        PageOps();
    }

    /// <summary>Delete, move, and select pages in a multi-page PDF</summary>
    static void PageOps()
    {
        string a = "sample.pdf";
        string b = "Blank.pdf";
        string built = "built.pdf";
        string selected = "selected.pdf";

        // sample.pdf + one blank page + first page of sample.pdf again.
        using (var doc = Document.Open(a))
        {
            using (var blank = Document.Open(b))
                doc.InsertPdf(blank, fromPage: 0, toPage: 0, startAt: 1);
            using (var again = Document.Open(a))
                doc.InsertPdf(again, fromPage: 0, toPage: 0, startAt: 2);

            // Move last page to the front.
            doc.MovePage(doc.PageCount - 1, 0);

            // Delete the middle page (index 1).
            doc.DeletePage(1);

            doc.Save(built);
        }

        // Select keeps only listed page numbers (0-based), in that order.
        using (var doc = Document.Open(built))
        {
            doc.Select(new[] { 0 });
            doc.Save(selected);
        }
    }
}
