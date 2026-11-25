using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.Tables;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 11-Tables");
        FindTables();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void FindTables()
    {
        string input = ExamplePaths.MuPdfNetInput("tables.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "11-Tables", "tables.md");
        string png = ExamplePaths.Output("MuPDF.NET", "11-Tables", "tables.png");
        var check = new ResultCheck("MuPDF.NET", "11-Tables");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                FindTables(input);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        captured = string.Join(
            "\n",
            captured.Split('\n').Where(line =>
                !line.StartsWith("Consider using the pymupdf_layout", StringComparison.Ordinal)));
        Console.Write(captured);
        File.WriteAllText(output, captured);

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(png);

        check.Text(captured, "tables.md");
        check.FileSha256(png, "tables.png.sha256");
        check.Finish();
    }

    // doc-start:FindTables
    /// <summary>Detect tables on a page and export Markdown</summary>
    static void FindTables(string inputPath)
    {
        using (var doc = Document.Open(inputPath))
        using (Page page = doc[0])
        {
            // lines_strict uses stroked vector lines as the table grid.
            List<Table> tables = page.GetTables(
                clip: page.Rect,
                vertical_strategy: "lines_strict",
                horizontal_strategy: "lines_strict");

            Console.WriteLine($"tableCount={tables.Count}");
            for (int i = 0; i < tables.Count; i++)
            {
                Table t = tables[i];
                Console.WriteLine($"table[{i}] rows={t.RowCount} cols={t.ColCount}");
                // ToMarkdown builds a GitHub-style markdown table from cell text.
                Console.WriteLine(t.ToMarkdown());
            }
        }
    }
    // doc-end:FindTables
}
