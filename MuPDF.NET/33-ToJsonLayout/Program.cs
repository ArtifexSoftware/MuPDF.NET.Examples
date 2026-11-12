using System.Text.RegularExpressions;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;
namespace MuPDF.NET.Examples.Llm.ToJsonLayout;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 33-ToJsonLayout");
        ToJsonLayout();
    }

    /// <summary>
    /// Convert a PDF to layout JSON (requires pymupdf 2.0 layout worker).
    /// </summary>
    static void ToJsonLayout()
    {
        string input = ExamplePaths.MuPdfNetInput("columns.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "33-ToJsonLayout", "columns.json");
        var check = new ResultCheck("MuPDF.NET", "33-ToJsonLayout");

        bool prior = Utils.LayoutEnabled;
        try
        {
            // ToJson needs the optional layout provider.
            if (!Utils.LayoutAvailable)
            {
                ConsoleEx.Info("Layout provider unavailable — ToJson requires UseLayout=true.");
                ConsoleEx.Info("Install pymupdf 2.0 (dotnet msbuild -t:MuPDFNetSetupLayoutPython) and re-run.");
                check.Text("LAYOUT_UNAVAILABLE\n", "layout-status.txt");
                check.Finish();
                return;
            }

            Utils.UseLayout(true);
            using Document doc = new Document(input);
            // ToJson embeds an absolute input path; keep only the file name for portable baselines.
            string json = NormalizeJsonFilename(doc.ToJson(useOcr: false) ?? "", input);
            File.WriteAllText(output, json);

            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"JSON length: {json.Length}");
            check.Text("LAYOUT_AVAILABLE\n", "layout-status.txt");
            check.Text(json, "columns.json");
        }
        finally
        {
            Utils.UseLayout(prior);
        }

        check.Finish();
    }

    /// <summary>
    /// Replace the absolute <c>filename</c> field with <see cref="Path.GetFileName"/> so Expected/
    /// baselines do not depend on the machine checkout path.
    /// </summary>
    static string NormalizeJsonFilename(string json, string inputPath)
    {
        string name = Path.GetFileName(inputPath);
        return Regex.Replace(
            json,
            "\"filename\"\\s*:\\s*\"(?:\\\\.|[^\"\\\\])*\"",
            "\"filename\":\"" + name.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"",
            RegexOptions.CultureInvariant,
            TimeSpan.FromSeconds(2));
    }
}
