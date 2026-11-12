using System.Linq;
using System.Text;
using MuPDF.NET.Examples.Common;
using MuPDF.NET;
namespace MuPDF.NET.Examples.Llm.GetKeyValues;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 38-GetKeyValues");
        GetKeyValues();
    }

    /// <summary>
    /// Extract AcroForm field names/values via Utils.GetKeyValues.
    /// </summary>
    static void GetKeyValues()
    {
        string input = ExamplePaths.MuPdfNetInput("Widget.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "38-GetKeyValues", "keyvalues.txt");
        var check = new ResultCheck("MuPDF.NET", "38-GetKeyValues");

        // Returns fieldName → property bag (value, page, …) for interactive form fields.
        Dictionary<string, Dictionary<string, object>> fields =
            Utils.GetKeyValues(input, includeXrefs: false);

        var sb = new StringBuilder();
        sb.Append("fieldCount=").Append(fields.Count).Append('\n');
        foreach (string name in fields.Keys.OrderBy(k => k, StringComparer.Ordinal))
        {
            Dictionary<string, object> info = fields[name];
            // Property keys may vary slightly by form; dump a stable sorted view.
            string props = string.Join(
                ";",
                info.OrderBy(kv => kv.Key, StringComparer.Ordinal)
                    .Select(kv => kv.Key + "=" + FormatValue(kv.Value)));
            sb.Append(name).Append('\t').Append(props).Append('\n');
        }

        string text = sb.ToString();
        File.WriteAllText(output, text);
        ConsoleEx.Info($"Opened: {input}");
        ConsoleEx.Info($"Form fields: {fields.Count}");
        check.Text(text, "keyvalues.txt");
        check.Finish();
    }

    static string FormatValue(object? value)
    {
        if (value == null)
            return "";
        string s = value.ToString() ?? "";
        return s.Replace('\t', ' ').Replace('\n', ' ').Replace('\r', ' ');
    }
}
