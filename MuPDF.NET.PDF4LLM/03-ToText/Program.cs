using MuPDF.NET.Examples.Common;
using MuPDF.NET.PDF4LLM;

namespace MuPDF.NET.Examples.PDF4LLM.ToText;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.PDF4LLM / 03-ToText");
        ToText();
    }

    /// <summary>
    /// Convert a PDF to plain text via layout (requires pymupdf-layout).
    /// </summary>
    static void ToText()
    {
        string input = ExamplePaths.Pdf4LlmInput("columns.pdf");
        string output = ExamplePaths.Output("MuPDF.NET.PDF4LLM", "03-ToText", "columns.txt");
        var check = new ResultCheck("MuPDF.NET.PDF4LLM", "03-ToText");

        bool prior = MuPDF4LLM.UseLayout;
        try
        {
            // ToText requires UseLayout=true.
            if (!MuPDF4LLM.LayoutAvailable)
            {
                ConsoleEx.Info("Layout provider unavailable — ToText requires UseLayout=true.");
                check.Text("LAYOUT_UNAVAILABLE\n", "layout-status.txt");
                check.Finish();
                return;
            }

            MuPDF4LLM.SetUseLayout(true);
            string text = MuPDF4LLM.ToText(input, useOcr: false, showProgress: false) ?? "";
            File.WriteAllText(output, text);

            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Text length: {text.Length}");
            check.Text("LAYOUT_AVAILABLE\n", "layout-status.txt");
            check.Text(text, "columns.txt");
        }
        finally
        {
            MuPDF4LLM.SetUseLayout(prior);
        }

        check.Finish();
    }
}
