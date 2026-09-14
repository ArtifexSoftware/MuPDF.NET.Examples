using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.PDF4LLM.TablesCsv;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.PDF4LLM / 05-TablesCsv");
        TablesToCsv();
    }

    /// <summary>
    /// Detect tables and write the first table as CSV.
    /// </summary>
    static void TablesToCsv()
    {
        string input = ExamplePaths.Pdf4LlmInput("national-capitals.pdf");
        string output = ExamplePaths.Output("MuPDF.NET.PDF4LLM", "05-TablesCsv", "capitals.csv");
        var check = new ResultCheck("MuPDF.NET.PDF4LLM", "05-TablesCsv");

        using var doc = Document.Open(input);
        using Page page = doc[0];

        // Same table finder as MuPDF.NET (PDF4LLM package pulls it in).
        List<Table> tables = Utils.GetTables(
            page,
            clip: page.Rect,
            vertical_strategy: "lines_strict",
            horizontal_strategy: "lines_strict");

        var sb = new StringBuilder();
        sb.Append("tableCount=").Append(tables.Count).Append('\n');
        if (tables.Count > 0)
        {
            Table table = tables[0];
            sb.Append("rows=").Append(table.RowCount).Append(" cols=").Append(table.ColCount).Append('\n');

            // Extract() returns row → cell text.
            List<List<string?>> rows = table.Extract();
            foreach (List<string?> row in rows)
                sb.Append(string.Join(",", row.Select(CsvEscape))).Append('\n');
        }

        string csv = sb.ToString();
        File.WriteAllText(output, csv);
        ConsoleEx.Info($"Opened: {input}");
        ConsoleEx.Info($"Tables found: {tables.Count}");
        check.Text(csv, "capitals.csv");
        check.Finish();
    }

    static string CsvEscape(string? cell)
    {
        string s = (cell ?? "").Replace("\r\n", " ").Replace('\n', ' ').Replace('\r', ' ').Trim();
        if (s.Contains(',') || s.Contains('"') || s.Contains(' '))
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        return s;
    }
}
