using MuPDF.NET.Examples.Common;
using MuPDF.NET.Examples.PDF4LLM.OllamaSupport;

namespace MuPDF.NET.Examples.PDF4LLM.OllamaCapitals;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.PDF4LLM / 10-Ollama");
        OllamaCapitals(args).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Index national-capitals.pdf with a local Ollama chat + embedding model, then Ask / Search.
    /// </summary>
    static Task OllamaCapitals(string[] args)
    {
        bool repl = args.Any(a => string.Equals(a, "--repl", StringComparison.OrdinalIgnoreCase));
        return OllamaRag.RunAsync(
            "10-Ollama",
            "national-capitals.pdf",
            AskQuestions,
            SearchQueries,
            repl);
    }

    static readonly string[] AskQuestions =
    {
        "What is the capital of South Sudan?",
        "What was the population of Juba in 2017?",
        "What is the capital of Canada?",
        "What percentage of Gibraltar's population lives in the capital?",
        "What is the capital of Tanzania?",
        "What is the listed population of Vatican City?",
        "What is the capital of the United States?",
        "What percentage of the United States population lives in Washington D.C.?",
        "What is the melting point of unobtainium?",
    };

    static readonly string[] SearchQueries =
    {
        "capital of South Sudan",
        "Vatican City population",
        "Ottawa Canada capital",
    };
}
