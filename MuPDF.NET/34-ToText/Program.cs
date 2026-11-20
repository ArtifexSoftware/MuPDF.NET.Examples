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

    /// <summary>Convert a PDF to plain text via layout (requires pymupdf 2.1 layout worker)</summary>
    static void ToText()
    {
        string input = "columns.pdf";
        string output = "columns.txt";

        bool prior = Utils.LayoutEnabled;
        try
        {
            // ToText requires UseLayout=true.
            if (!Utils.LayoutAvailable)
            {
                return;
            }

            Utils.UseLayout(true);
            using Document doc = new Document(input);
            string text = doc.ToText(useOcr: false, showProgress: false) ?? "";

        }
        finally
        {
            Utils.UseLayout(prior);
        }
    }
}
