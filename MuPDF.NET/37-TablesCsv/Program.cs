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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void TablesToCsv()
    {
        string input = ExamplePaths.MuPdfNetInput("tables-csv.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "37-TablesCsv", "berth.csv");
        string png = ExamplePaths.Output("MuPDF.NET", "37-TablesCsv", "page.png");
        var check = new ResultCheck("MuPDF.NET", "37-TablesCsv");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(png);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                TablesToCsv(input, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        var dump = new StringBuilder();
        foreach (string line in captured.Split('\n'))
        {
            if (line.StartsWith("Consider using the pymupdf_layout", StringComparison.Ordinal)
                || line.Contains("pymupdf-layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("greatly improved page layout", StringComparison.OrdinalIgnoreCase))
                continue;
            dump.Append(line).Append('\n');
        }
        captured = dump.ToString();
        Console.Write(captured);

        check.Text(captured, "tables-csv.txt");
        if (File.Exists(output))
            check.Text(File.ReadAllText(output), "berth.csv");
        check.FileSha256(png, "page.png.sha256");
        check.Finish();
    }

    // doc-start:TablesToCsv
    /// <summary>Detect tables on a page and write the first table as CSV</summary>
    static void TablesToCsv(string inputPath, string outputPath)
    {
        using var doc = Document.Open(inputPath);
        using Page page = doc[0];

        // lines_strict uses stroked vector lines as the table grid.
        List<Table> tables = page.GetTables(
            clip: page.Rect,
            vertical_strategy: "lines_strict",
            horizontal_strategy: "lines_strict");

        Console.WriteLine($"tableCount={tables.Count}");
        if (tables.Count == 0)
            return;

        Table table = tables[0];
        Console.WriteLine($"rows={table.RowCount} cols={table.ColCount}");
        List<List<string?>> extracted = table.Extract();

        var csv = new StringBuilder();
        foreach (List<string?> row in extracted)
            csv.AppendLine(string.Join(",", row.Select(CsvEscape)));

        string text = csv.ToString();
        File.WriteAllText(outputPath, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        Console.Write(text);
    }

    static string CsvEscape(string? cell)
    {
        string s = (cell ?? "").Replace("\r\n", " ").Replace('\n', ' ').Replace('\r', ' ').Trim();
        if (s.Contains(',') || s.Contains('"') || s.Contains(' '))
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        return s;
    }
    // doc-end:TablesToCsv
}
