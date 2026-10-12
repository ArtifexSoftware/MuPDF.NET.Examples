using MuPDF.NET.Examples.Common;
using MuPDF.NET.Examples.PDF4LLM.OllamaSupport;

namespace MuPDF.NET.Examples.PDF4LLM.OllamaMagazines;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.PDF4LLM / 11-OllamaMagazines");
        OllamaMagazines(args).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Index Magazines-PDF.pdf with a local Ollama chat + embedding model, then Ask / Search.
    /// </summary>
    static Task OllamaMagazines(string[] args)
    {
        bool repl = args.Any(a => string.Equals(a, "--repl", StringComparison.OrdinalIgnoreCase));
        return OllamaRag.RunAsync(
            "11-OllamaMagazines",
            "Magazines-PDF.pdf",
            AskQuestions,
            SearchQueries,
            repl);
    }

    static readonly string[] AskQuestions =
    {
        "What is a magazine, according to the definition in this document?",
        "Name the ten types of magazines listed in the document.",
        "What do fashion magazines focus on?",
        "Which magazine is given as an example of a lifestyle magazine?",
        "What details does a magazine masthead contain?",
        "What is the difference between a magazine and a newspaper?",
        "What is the difference between a magazine, a journal, and a catalog?",
        "What standard magazine sizes are used in the US and UK?",
        "What type of paper is commonly used for magazine printing?",
        "What is the melting point of unobtainium?",
    };

    static readonly string[] SearchQueries =
    {
        "lifestyle magazine Rolling Stone",
        "magazine masthead editors writers designers",
        "standard magazine sizes 8.5 by 11",
    };
}
