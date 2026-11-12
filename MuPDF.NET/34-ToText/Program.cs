using MuPDF.NET;
using MuPDF.NET.Examples.Common;
namespace MuPDF.NET.Examples.Llm.ToText;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 34-ToText");
        ToText();
    }

    /// <summary>
    /// Convert a PDF to plain text via layout (requires pymupdf 2.1 layout worker).
    /// </summary>
    static void ToText()
    {
        string input = ExamplePaths.MuPdfNetInput("columns.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "34-ToText", "columns.txt");
        var check = new ResultCheck("MuPDF.NET", "34-ToText");

        bool prior = Utils.LayoutEnabled;
        try
        {
            // ToText requires UseLayout=true.
            if (!Utils.LayoutAvailable)
            {
                ConsoleEx.Info("Layout provider unavailable — ToText requires UseLayout=true.");
                check.Text("LAYOUT_UNAVAILABLE\n", "layout-status.txt");
                check.Finish();
                return;
            }

            Utils.UseLayout(true);
            using Document doc = new Document(input);
            string text = doc.ToText(useOcr: false, showProgress: false) ?? "";
            File.WriteAllText(output, text);

            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Text length: {text.Length}");
            check.Text("LAYOUT_AVAILABLE\n", "layout-status.txt");
            check.Text(text, "columns.txt");
        }
        finally
        {
            Utils.UseLayout(prior);
        }

        check.Finish();
    }
}
