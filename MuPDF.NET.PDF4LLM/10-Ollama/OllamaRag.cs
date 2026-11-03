using System.Net.Http;
using Microsoft.Extensions.AI;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.PDF4LLM;
using MuPDF.NET.PDF4LLM.AI;
using MuPDF.NET.PDF4LLM.AI.Models;
using MuPDF.NET.PDF4LLM.AI.Options;
using OllamaSharp;
using OllamaSharp.Models;

namespace MuPDF.NET.Examples.PDF4LLM.OllamaSupport;

/// <summary>
/// Shared Ollama + LoadAiAsync host used by 10-Ollama and 11-OllamaMagazines.
/// </summary>
internal static class OllamaRag
{
    public static async Task RunAsync(
        string exampleName,
        string pdfFileName,
        IReadOnlyList<string> askQuestions,
        IReadOnlyList<string> searchQueries,
        bool repl)
    {
        string host = Environment.GetEnvironmentVariable("OLLAMA_HOST") ?? "http://localhost:11434";
        string chatModel = Environment.GetEnvironmentVariable("OLLAMA_MODEL") ?? "mistral";
        string embedModel = Environment.GetEnvironmentVariable("OLLAMA_EMBED_MODEL") ?? "nomic-embed-text";

        if (!Uri.TryCreate(host, UriKind.Absolute, out Uri? uri))
        {
            Console.WriteLine($"SKIP — Invalid OLLAMA_HOST '{host}'.");
            return;
        }

        string pdfPath = ExamplePaths.Pdf4LlmInput(pdfFileName);
        string output = ExamplePaths.Output("MuPDF.NET.PDF4LLM", exampleName, "ollama.txt");

        using HttpClient chatHttp = CreateHttp(uri);
        using HttpClient embedHttp = CreateHttp(uri);
        using OllamaApiClient chatOllama = new(chatHttp, chatModel);
        using OllamaApiClient embedOllama = new(embedHttp, embedModel);

        string? resolvedChat = await TryResolveModelAsync(chatOllama, uri, chatModel);
        if (resolvedChat == null)
            return;

        chatModel = resolvedChat;
        string? resolvedEmbed = chatModel;
        if (!string.Equals(chatModel, embedModel, StringComparison.OrdinalIgnoreCase))
        {
            resolvedEmbed = await TryResolveModelAsync(embedOllama, uri, embedModel);
            if (resolvedEmbed == null)
                return;
        }

        embedModel = resolvedEmbed!;
        ConsoleEx.Info($"Ollama {uri}  chat={chatModel}  embed={embedModel}");
        ConsoleEx.Info($"Indexing {pdfFileName} …");

        bool layout = MuPDF4LLM.LayoutAvailable;
        bool prior = MuPDF4LLM.UseLayout;
        var report = new System.Text.StringBuilder();
        try
        {
            MuPDF4LLM.SetUseLayout(layout);
            var options = new MicrosoftAIConnectorOptions
            {
                ChatClient = chatOllama,
                EmbeddingGenerator = embedOllama,
                UseLayout = layout,
            };

            AiDocumentCollection aiDocs = await MuPDF4LLM.LoadAiAsync(new[] { pdfPath }, options);
            ConsoleEx.Info($"Indexed {aiDocs.ChunkCount} chunks from: {string.Join(", ", aiDocs.SourceFiles)}");

            report.Append("chunkCount=").Append(aiDocs.ChunkCount).Append('\n');
            foreach (string source in aiDocs.SourceFiles)
                report.Append("source=").Append(source).Append('\n');

            Console.WriteLine();
            Console.WriteLine("--- Ask ---");
            foreach (string question in askQuestions)
            {
                string answer = await PrintAskAsync(aiDocs, question);
                report.Append("--- ask: ").Append(question).Append(" ---\n");
                report.Append(ResultCheck.NormalizeText(answer)).Append('\n');
            }

            Console.WriteLine("--- Search ---");
            foreach (string query in searchQueries)
            {
                string search = await PrintSearchAsync(aiDocs, query);
                report.Append("--- search: ").Append(query).Append(" ---\n");
                report.Append(search);
            }

            File.WriteAllText(output, report.ToString());
            ConsoleEx.Info($"Wrote: {output}");

            var check = new ResultCheck("MuPDF.NET.PDF4LLM", exampleName);
            check.Equal(aiDocs.ChunkCount > 0, true, "indexed chunks");
            check.Finish();

            if (!repl || Console.IsInputRedirected)
                return;

            Console.WriteLine($"Ask another question about {pdfFileName}. Type 'exit' to quit.");
            Console.WriteLine();
            while (true)
            {
                Console.Write("> ");
                string? line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;
                Console.WriteLine();
                await PrintAskAsync(aiDocs, line);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Indexing or chat failed: {ex.Message}");
            if (ex.InnerException != null)
                Console.Error.WriteLine(ex.InnerException.Message);
            if (ex.StatusCode == System.Net.HttpStatusCode.NotImplemented)
            {
                Console.Error.WriteLine("That model cannot embed. Pull an embedding model:");
                Console.Error.WriteLine("  ollama pull nomic-embed-text");
            }

            Environment.ExitCode = 1;
        }
        finally
        {
            MuPDF4LLM.SetUseLayout(prior);
        }
    }

    static async Task<string> PrintAskAsync(AiDocumentCollection aiDocs, string question)
    {
        Console.WriteLine($"Ask: {question}");
        try
        {
            string answer = await aiDocs.AskAsync(question);
            Console.WriteLine(answer);
            Console.WriteLine();
            return answer;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Ask failed: {ex.Message}");
            if (ex.InnerException != null)
                Console.Error.WriteLine(ex.InnerException.Message);
            Console.WriteLine();
            return "";
        }
    }

    static async Task<string> PrintSearchAsync(AiDocumentCollection aiDocs, string query)
    {
        Console.WriteLine($"Search: {query}");
        var block = new System.Text.StringBuilder();
        try
        {
            IReadOnlyList<SearchResult> hits = await aiDocs.SearchAsync(query, topK: 10);
            Console.WriteLine($"  Ranked all {aiDocs.ChunkCount} chunks; showing top {hits.Count}.");
            for (int i = 0; i < hits.Count; i++)
            {
                SearchResult hit = hits[i];
                if (i > 0)
                    Console.WriteLine();
                Console.WriteLine($"  {i + 1}. [{hit.Score:F3}] {hit.Chunk.SourceFileName} p{hit.Chunk.PageNumber}");
                foreach (string line in SplitLines(hit.Chunk.Text))
                    Console.WriteLine($"      {line}");
                block.Append(i + 1).Append(". ")
                    .Append(hit.Chunk.SourceFileName)
                    .Append(" p").Append(hit.Chunk.PageNumber)
                    .Append('\n');
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Search failed: {ex.Message}");
            if (ex.InnerException != null)
                Console.Error.WriteLine(ex.InnerException.Message);
        }

        Console.WriteLine();
        return block.ToString();
    }

    static IEnumerable<string> SplitLines(string text)
    {
        string[] lines = text.Replace("\r\n", "\n").Replace('\r', '\n').Trim().Split('\n');
        for (int i = 0; i < lines.Length; i++)
            lines[i] = lines[i].TrimEnd();
        return lines;
    }

    static HttpClient CreateHttp(Uri uri) =>
        new()
        {
            BaseAddress = uri,
            Timeout = TimeSpan.FromMinutes(10),
        };

    static async Task<string?> TryResolveModelAsync(OllamaApiClient ollama, Uri uri, string model)
    {
        try
        {
            if (!await ollama.IsRunningAsync())
            {
                Console.WriteLine($"SKIP — Cannot reach Ollama at {uri}.");
                Console.WriteLine("Install Ollama, start it (`ollama serve` or the Ollama app), then:");
                Console.WriteLine("  ollama pull mistral");
                Console.WriteLine("  ollama pull nomic-embed-text");
                Console.WriteLine("See MuPDF.NET.PDF4LLM/10-Ollama/README.md.");
                return null;
            }

            IEnumerable<Model> local = await ollama.ListLocalModelsAsync();
            string? installed = ResolveInstalledName(local, model);
            if (installed == null)
            {
                Console.WriteLine($"SKIP — Model '{model}' is not installed.");
                Console.WriteLine($"Install it with: ollama pull {model}");
                return null;
            }

            if (!string.Equals(installed, model, StringComparison.OrdinalIgnoreCase))
                ConsoleEx.Info($"Using installed model '{installed}' for '{model}'.");

            ollama.SelectedModel = installed;
            return installed;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"SKIP — Cannot reach Ollama at {uri}.");
            Console.WriteLine(ex.Message);
            Console.WriteLine("Start the Ollama app or `ollama serve`, then retry.");
            return null;
        }
    }

    static string? ResolveInstalledName(IEnumerable<Model> models, string requested)
    {
        var names = new List<string>();
        foreach (Model model in models)
        {
            AddName(names, model.Name);
            AddName(names, model.ModelName);
        }

        foreach (string name in names)
        {
            if (string.Equals(name, requested, StringComparison.OrdinalIgnoreCase))
                return name;
        }

        string latest = requested + ":latest";
        foreach (string name in names)
        {
            if (string.Equals(name, latest, StringComparison.OrdinalIgnoreCase))
                return name;
        }

        foreach (string name in names)
        {
            int colon = name.IndexOf(':');
            string baseName = colon < 0 ? name : name.Substring(0, colon);
            if (string.Equals(baseName, requested, StringComparison.OrdinalIgnoreCase))
                return name;
        }

        return null;
    }

    static void AddName(List<string> names, string? candidate)
    {
        if (string.IsNullOrEmpty(candidate))
            return;
        if (!names.Exists(n => string.Equals(n, candidate, StringComparison.OrdinalIgnoreCase)))
            names.Add(candidate);
    }
}
