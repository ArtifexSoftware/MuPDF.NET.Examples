using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.GetKeyValues;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 39-GetKeyValues");
        GetKeyValues();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void GetKeyValues()
    {
        string input = ExamplePaths.MuPdfNetInput("widgets.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "39-GetKeyValues", "keyvalues.txt");
        string png = ExamplePaths.Output("MuPDF.NET", "39-GetKeyValues", "page.png");
        var check = new ResultCheck("MuPDF.NET", "39-GetKeyValues");
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
                GetKeyValues(input, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        check.Text(captured, "keyvalues-dump.txt");
        if (File.Exists(output))
            check.Text(File.ReadAllText(output), "keyvalues.txt");
        check.FileSha256(png, "page.png.sha256");
        check.Finish();
    }

    // doc-start:GetKeyValues
    /// <summary>Extract AcroForm field names and values via Utils.GetKeyValues</summary>
    static void GetKeyValues(string inputPath, string outputPath)
    {
        // includeXrefs:false omits widget xref numbers (they change if the PDF is rebuilt).
        Dictionary<string, Dictionary<string, object>> fields =
            Utils.GetKeyValues(inputPath, includeXrefs: false);

        string[] headers = { "name", "field_value", "pages", "terminal" };
        var rows = new List<string[]>();
        foreach (string name in fields.Keys.OrderBy(k => k, StringComparer.Ordinal))
        {
            Dictionary<string, object> info = fields[name];
            rows.Add(new[]
            {
                name,
                FormatValue(info.GetValueOrDefault("field_value")),
                FormatValue(info.GetValueOrDefault("pages")),
                FormatValue(info.GetValueOrDefault("terminal")),
            });
        }

        int[] widths = new int[headers.Length];
        for (int c = 0; c < headers.Length; c++)
        {
            int w = headers[c].Length;
            foreach (string[] row in rows)
                w = Math.Max(w, row[c].Length);
            widths[c] = w;
        }

        var listing = new StringBuilder();
        listing.AppendLine($"fieldCount={fields.Count}");
        listing.AppendLine(FormatRow(headers, widths));
        listing.AppendLine(string.Join("  ", widths.Select(w => new string('-', w))));
        foreach (string[] row in rows)
            listing.AppendLine(FormatRow(row, widths));

        string text = listing.ToString();
        File.WriteAllText(outputPath, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        Console.Write(text);
    }

    static string FormatRow(string[] cells, int[] widths) =>
        string.Join("  ", cells.Select((cell, i) => cell.PadRight(widths[i])));

    static string FormatValue(object? value)
    {
        if (value == null)
            return "";
        if (value is List<int> pages)
            return "[" + string.Join(",", pages) + "]";
        if (value is bool flag)
            return flag ? "True" : "False";
        string s = value.ToString() ?? "";
        return s.Replace('\t', ' ').Replace('\n', ' ').Replace('\r', ' ');
    }
    // doc-end:GetKeyValues
}
