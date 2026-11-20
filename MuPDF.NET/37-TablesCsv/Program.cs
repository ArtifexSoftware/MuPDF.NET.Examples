using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.TablesCsv;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 37-TablesCsv");
        TablesToCsv();
    }

    /// <summary>Detect tables and write the first table as CSV</summary>
    static void TablesToCsv()
    {
        string input = "national-capitals.pdf";
        string output = "capitals.csv";

        using var doc = Document.Open(input);
        using Page page = doc[0];

        // Same table finder as other MuPDF.NET table samples.
        List<Table> tables = Utils.GetTables(
            page,
            clip: page.Rect,
            vertical_strategy: "lines_strict",
            horizontal_strategy: "lines_strict");

        if (tables.Count > 0)
        {
            Table table = tables[0];

            // Extract() returns row → cell text.
            List<List<string?>> rows = table.Extract();
        }

    }

    static string CsvEscape(string? cell)
    {
        string s = (cell ?? "").Replace("\r\n", " ").Replace('\n', ' ').Replace('\r', ' ').Trim();
        if (s.Contains(',') || s.Contains('"') || s.Contains(' '))
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        return s;
    }
}
