using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.TextExtractSearch;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 04-TextExtractSearch");
        TextExtractSearch();
    }

    /// <summary>Extract plain text from page 1 and search for a string</summary>
    static void TextExtractSearch()
    {
        string input = "sample.pdf";
        const string needle = "Hydraulik";

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        {
            // "text" = plain text extraction (also: "blocks", "words", "html", "dict", …).
            string text = page.GetText("text") ?? "";

            // SearchFor returns hit rectangles on the page.
            var hits = page.SearchFor(needle);

        }
    }
}
