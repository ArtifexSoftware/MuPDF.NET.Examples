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

    /// <summary>
    /// Extract plain text from page 1 and search for a string.
    /// </summary>
    static void TextExtractSearch()
    {
        string input = ExamplePaths.MuPdfNetInput("sample.pdf");
        const string needle = "Hydraulik";
        var check = new ResultCheck("MuPDF.NET", "04-TextExtractSearch");

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        {
            // "text" = plain text extraction (also: "blocks", "words", "html", "dict", …).
            string text = page.GetText("text") ?? "";

            // SearchFor returns hit rectangles on the page.
            var hits = page.SearchFor(needle);

            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Page 1 characters: {text.Length}");
            ConsoleEx.Info($"SearchFor(\"{needle}\") hits: {hits.Count}");

            // Examples harness baselines.
            check.Text(text, "page-1.txt");
            check.Properties(
                new Dictionary<string, string>
                {
                    ["needle"] = needle,
                    ["hitCount"] = hits.Count.ToString(),
                },
                "search.summary.txt");
        }

        check.Finish();
    }
}
