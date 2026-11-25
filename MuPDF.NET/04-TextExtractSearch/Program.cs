using System.Text;
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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void TextExtractSearch()
    {
        string input = ExamplePaths.MuPdfNetInput("unicode.pdf");
        const string needle = "こんにちは";
        string extractPath = ExamplePaths.Output("MuPDF.NET", "04-TextExtractSearch", "page-1.txt");
        var check = new ResultCheck("MuPDF.NET", "04-TextExtractSearch");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                TextExtractSearch(input, needle);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);
        File.WriteAllText(extractPath, captured);

        int split = captured.IndexOf("hitCount=", StringComparison.Ordinal);
        string textPart = split >= 0 ? captured[..split] : captured;
        string searchPart = split >= 0 ? captured[split..] : "";
        check.Text(textPart, "page-1.txt");
        check.Text(searchPart, "search.summary.txt");
        check.Finish();
    }

    // doc-start:TextExtractSearch
    /// <summary>Extract plain text from page 1 and search for a string</summary>
    static void TextExtractSearch(string inputPath, string needle)
    {
        using (var doc = Document.Open(inputPath))
        using (Page page = doc[0])
        {
            // "text" = plain text extraction (also: "blocks", "words", "html", "dict", …).
            string text = page.GetText("text") ?? "";
            Console.WriteLine(text);

            // SearchFor returns hit quads on the page.
            var hits = page.SearchFor(needle);
            Console.WriteLine($"hitCount={hits.Count}");
            Console.WriteLine($"needle={needle}");
            foreach (Quad hit in hits)
                Console.WriteLine($"  {hit}");
        }
    }
    // doc-end:TextExtractSearch
}
