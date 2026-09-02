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

    /// <summary>
    /// Delete, move, and select pages in a multi-page PDF.
    /// </summary>
    static void PageOps()
    {
        string a = ExamplePaths.MuPdfNetInput("sample.pdf");
        string b = ExamplePaths.MuPdfNetInput("Blank.pdf");
        string built = ExamplePaths.Output("MuPDF.NET", "29-PageOps", "built.pdf");
        string selected = ExamplePaths.Output("MuPDF.NET", "29-PageOps", "selected.pdf");
        var check = new ResultCheck("MuPDF.NET", "29-PageOps");
        var report = new StringBuilder();

        // Build a 3-page document: sample page + blank + sample page again.
        using (var doc = Document.Open(a))
        using (var blank = Document.Open(b))
        using (var again = Document.Open(a))
        {
            doc.InsertPdf(blank, fromPage: 0, toPage: 0, startAt: 1);
            doc.InsertPdf(again, fromPage: 0, toPage: 0, startAt: 2);
            report.Append("built.pages=").Append(doc.PageCount).Append('\n');

            // Move last page to the front.
            doc.MovePage(doc.PageCount - 1, 0);
            report.Append("afterMove.pages=").Append(doc.PageCount).Append('\n');

            // Delete the middle page (index 1).
            doc.DeletePage(1);
            report.Append("afterDelete.pages=").Append(doc.PageCount).Append('\n');

            doc.Save(built);
        }

        // Select keeps only listed page numbers (0-based), in that order.
        using (var doc = Document.Open(built))
        {
            doc.Select(new[] { 0 });
            report.Append("afterSelect.pages=").Append(doc.PageCount).Append('\n');
            doc.Save(selected);
        }

        check.Text(report.ToString(), "page-ops.txt");
        check.Properties(PdfFingerprint.FromFile(built), "built.summary.txt");
        check.Properties(PdfFingerprint.FromFile(selected), "selected.summary.txt");
        check.Finish();
    }
}
