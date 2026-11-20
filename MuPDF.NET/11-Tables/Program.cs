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

    /// <summary>Detect tables on a page and export Markdown</summary>
    static void FindTables()
    {
        string input = "err_table.pdf";
        string output = "tables.md";

        using var doc = Document.Open(input);
        using Page page = doc[0];

        // lines_strict uses vector lines as table grid (good for ruled tables).
        List<Table> tables = Utils.GetTables(
            page,
            clip: page.Rect,
            vertical_strategy: "lines_strict",
            horizontal_strategy: "lines_strict");

        for (int i = 0; i < tables.Count; i++)
        {
            Table t = tables[i];
            try
            {
                // ToMarkdown builds a GitHub-style markdown table from cell text.
            }
            catch (Exception ex)
            {
            }
        }

    }
}
