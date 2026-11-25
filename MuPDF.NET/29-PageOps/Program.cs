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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void PageOps()
    {
        string input = ExamplePaths.MuPdfNetInput("page-ops.pdf");
        string insert = ExamplePaths.MuPdfNetInput("page-ops-insert.pdf");
        string built = ExamplePaths.Output("MuPDF.NET", "29-PageOps", "built.pdf");
        string selected = ExamplePaths.Output("MuPDF.NET", "29-PageOps", "selected.pdf");
        var check = new ResultCheck("MuPDF.NET", "29-PageOps");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        ConsoleEx.Info($"Opened: {insert}");

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                PageOps(input, insert, built, selected);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        RenderPages(input, "orig", 3);
        RenderPages(built, "built", 4);
        RenderPages(selected, "selected", 1);

        check.Text(captured, "page-ops.txt");
        check.Properties(PdfFingerprint.FromFile(built), "built.summary.txt");
        check.Properties(PdfFingerprint.FromFile(selected), "selected.summary.txt");
        for (int i = 0; i < 3; i++)
            check.FileSha256(ExamplePaths.Output("MuPDF.NET", "29-PageOps", $"orig-{i}.png"), $"orig-{i}.png.sha256");
        for (int i = 0; i < 4; i++)
            check.FileSha256(ExamplePaths.Output("MuPDF.NET", "29-PageOps", $"built-{i}.png"), $"built-{i}.png.sha256");
        check.FileSha256(ExamplePaths.Output("MuPDF.NET", "29-PageOps", "selected-0.png"), "selected-0.png.sha256");
        check.Finish();
    }

    // help:skip
    static void RenderPages(string pdf, string prefix, int count)
    {
        using var doc = Document.Open(pdf);
        for (int i = 0; i < count; i++)
        {
            string png = ExamplePaths.Output("MuPDF.NET", "29-PageOps", $"{prefix}-{i}.png");
            using Page page = doc[i];
            using Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2));
            pix.Save(png);
        }
    }

    // doc-start:PageOps
    /// <summary>Delete, move, and select pages in a multi-page PDF</summary>
    static void PageOps(string inputPath, string insertPath, string builtPath, string selectedPath)
    {
        static string Labels(Document doc)
        {
            var parts = new List<string>();
            for (int i = 0; i < doc.PageCount; i++)
            {
                using Page page = doc[i];
                string text = (page.GetText("text") ?? "").Replace("\r\n", "\n").Trim();
                string first = text.Split('\n')[0].Trim();
                parts.Add(first);
            }
            return string.Join(",", parts);
        }

        using (var doc = Document.Open(inputPath))
        {
            Console.WriteLine($"before={Labels(doc)}");

            // Insert X at index 1, then copy page A again at index 2.
            using (var extra = Document.Open(insertPath))
                doc.InsertPdf(extra, fromPage: 0, toPage: 0, startAt: 1);
            using (var again = Document.Open(inputPath))
                doc.InsertPdf(again, fromPage: 0, toPage: 0, startAt: 2);
            Console.WriteLine($"afterInsert={Labels(doc)}");
            Console.WriteLine($"afterInsert.pageCount={doc.PageCount}");

            // Move last page to the front.
            doc.MovePage(doc.PageCount - 1, 0);
            Console.WriteLine($"afterMove={Labels(doc)}");
            Console.WriteLine($"afterMove.pageCount={doc.PageCount}");

            // Delete the middle page (index 1).
            doc.DeletePage(1);
            Console.WriteLine($"afterDelete={Labels(doc)}");
            Console.WriteLine($"afterDelete.pageCount={doc.PageCount}");
            doc.Save(builtPath);
        }

        // Select keeps only listed page numbers (0-based), in that order.
        using (var doc = Document.Open(builtPath))
        {
            doc.Select(new[] { 0 });
            Console.WriteLine($"afterSelect={Labels(doc)}");
            Console.WriteLine($"afterSelect.pageCount={doc.PageCount}");
            doc.Save(selectedPath);
        }
    }
    // doc-end:PageOps
}
