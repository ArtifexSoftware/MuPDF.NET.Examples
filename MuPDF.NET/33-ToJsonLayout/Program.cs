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

    /// <summary>Convert a PDF to layout JSON (requires pymupdf 2.1 layout worker)</summary>
    static void ToJsonLayout()
    {
        string input = "columns.pdf";
        string output = "columns.json";

        bool prior = Utils.LayoutEnabled;
        try
        {
            // ToJson needs the optional layout provider.
            if (!Utils.LayoutAvailable)
            {
                return;
            }

            Utils.UseLayout(true);
            using Document doc = new Document(input);
            string json = NormalizeJsonFilename(doc.ToJson(useOcr: false) ?? "", input);

        }
        finally
        {
            Utils.UseLayout(prior);
        }
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
