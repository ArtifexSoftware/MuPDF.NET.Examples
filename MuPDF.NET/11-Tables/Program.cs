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

    /// <summary>
    /// Detect tables on a page and export Markdown.
    /// </summary>
    static void FindTables()
    {
        string input = ExamplePaths.MuPdfNetInput("err_table.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "11-Tables", "tables.md");
        var check = new ResultCheck("MuPDF.NET", "11-Tables");

        using var doc = Document.Open(input);
        using Page page = doc[0];

        // lines_strict uses vector lines as table grid (good for ruled tables).
        List<Table> tables = Utils.GetTables(
            page,
            clip: page.Rect,
            vertical_strategy: "lines_strict",
            horizontal_strategy: "lines_strict");

        var sb = new StringBuilder();
        sb.Append("tableCount=").Append(tables.Count).Append('\n');
        for (int i = 0; i < tables.Count; i++)
        {
            Table t = tables[i];
            sb.Append("table[").Append(i).Append("] rows=").Append(t.RowCount)
                .Append(" cols=").Append(t.ColCount).Append('\n');
            try
            {
                // ToMarkdown builds a GitHub-style markdown table from cell text.
                sb.Append(t.ToMarkdown() ?? "").Append('\n');
            }
            catch (Exception ex)
            {
                sb.Append("ToMarkdown failed: ").Append(ex.Message).Append('\n');
            }
        }

        string text = sb.ToString();
        File.WriteAllText(output, text);
        ConsoleEx.Info($"Opened: {input}");
        ConsoleEx.Info($"Tables found: {tables.Count}");
        check.Text(text, "tables.md");
        check.Finish();
    }
}
