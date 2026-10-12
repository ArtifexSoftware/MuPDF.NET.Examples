using System.Text;
using Microsoft.Extensions.AI;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.PDF4LLM;
using MuPDF.NET.PDF4LLM.AI;
using MuPDF.NET.PDF4LLM.AI.Models;
using MuPDF.NET.PDF4LLM.AI.Options;

namespace MuPDF.NET.Examples.PDF4LLM.LoadAi;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.PDF4LLM / 09-LoadAi");
        LoadAi().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Index PDFs for RAG, then Search / Ask / Summarize.
    /// </summary>
    static async Task LoadAi()
    {
        string capitals = ExamplePaths.Pdf4LlmInput("national-capitals.pdf");
        string nato = ExamplePaths.Pdf4LlmInput("nato-members.pdf");
        string output = ExamplePaths.Output("MuPDF.NET.PDF4LLM", "09-LoadAi", "load-ai.txt");
        var check = new ResultCheck("MuPDF.NET.PDF4LLM", "09-LoadAi");

        bool azure = TryAzureOptions(out MicrosoftAIConnectorOptions options);
        if (!azure)
            options = MicrosoftAIConnectorOptions.CreateForDevelopment(chatClient: new ExampleRagChatClient());

        ConsoleEx.Info(azure
            ? "Pipeline: Azure OpenAI (live model — ask/search text not compared to Expected/)"
            : "Pipeline: development (in-memory index + local IChatClient)");

        bool prior = MuPDF4LLM.UseLayout;
        try
        {
            // Classic extract so Expected/ stays stable without pymupdf-layout.
            MuPDF4LLM.SetUseLayout(false);
            options.UseLayout = false;

            // PDF → chunks → embed → vector index (Microsoft.Extensions.AI).
            AiDocumentCollection aiDocs = await MuPDF4LLM.LoadAiAsync(
                new[] { capitals, nato },
                options);

            const string question =
                "Which NATO member countries have capitals with more than 5 million people?";
            const string searchQuery = "capital cities with more than 50% of national population";

            string answer = await aiDocs.AskAsync(question);
            string summary = await aiDocs.SummarizeAsync("national-capitals.pdf");
            IReadOnlyList<SearchResult> hits = await aiDocs.SearchAsync(searchQuery, topK: 5);

            var report = new StringBuilder();
            report.Append("chunkCount=").Append(aiDocs.ChunkCount).Append('\n');
            foreach (string source in aiDocs.SourceFiles.OrderBy(s => s, StringComparer.OrdinalIgnoreCase))
                report.Append("source=").Append(source).Append('\n');

            // Index shape is local extraction — comparable even with Azure embeddings.
            check.Text(report.ToString(), "index.summary.txt");

            var search = new StringBuilder();
            search.Append("query=").Append(searchQuery).Append('\n');
            for (int i = 0; i < hits.Count; i++)
            {
                SearchResult hit = hits[i];
                search.Append(i + 1).Append(". ")
                    .Append(hit.Chunk.SourceFileName)
                    .Append(" p").Append(hit.Chunk.PageNumber)
                    .Append('\n');
            }

            File.WriteAllText(output, report.ToString()
                + "--- ask ---\n" + ResultCheck.NormalizeText(answer) + "\n"
                + "--- summarize ---\n" + ResultCheck.NormalizeText(summary) + "\n"
                + "--- search ---\n" + search);

            ConsoleEx.Info($"Indexed {aiDocs.ChunkCount} chunks");
            ConsoleEx.Info($"Ask: {answer.Replace('\n', ' ')[..Math.Min(120, answer.Length)]}");

            if (!azure)
            {
                check.Text(ResultCheck.NormalizeText(answer) + "\n", "ask.txt");
                check.Text(ResultCheck.NormalizeText(summary) + "\n", "summarize.txt");
                check.Text(search.ToString(), "search.txt");
            }
        }
        finally
        {
            MuPDF4LLM.SetUseLayout(prior);
        }

        check.Finish();
    }

    static bool TryAzureOptions(out MicrosoftAIConnectorOptions options)
    {
        string? endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
        string? apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
        string? chatDeployment = Environment.GetEnvironmentVariable("AZURE_OPENAI_CHAT_DEPLOYMENT");
        string? embeddingDeployment = Environment.GetEnvironmentVariable("AZURE_OPENAI_EMBEDDING_DEPLOYMENT");

        if (string.IsNullOrWhiteSpace(endpoint)
            || string.IsNullOrWhiteSpace(apiKey)
            || string.IsNullOrWhiteSpace(chatDeployment)
            || string.IsNullOrWhiteSpace(embeddingDeployment))
        {
            options = null!;
            return false;
        }

        options = new AzureAiOptions
        {
            OpenAiEndpoint = endpoint,
            OpenAiApiKey = apiKey,
            ChatDeploymentName = chatDeployment,
            EmbeddingDeploymentName = embeddingDeployment,
            SearchEndpoint = Environment.GetEnvironmentVariable("AZURE_SEARCH_ENDPOINT"),
            SearchApiKey = Environment.GetEnvironmentVariable("AZURE_SEARCH_API_KEY"),
            SearchIndexName = Environment.GetEnvironmentVariable("AZURE_SEARCH_INDEX"),
        }.ToConnectorOptions();
        return true;
    }

    /// <summary>Offline chat client so the sample runs with no API keys.</summary>
    sealed class ExampleRagChatClient : IChatClient
    {
        public ChatClientMetadata Metadata { get; } = new("example-rag");

        public Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            var list = messages.ToList();
            string system = list.FirstOrDefault(m => m.Role == ChatRole.System)?.Text ?? "";
            string user = list.LastOrDefault(m => m.Role == ChatRole.User)?.Text ?? "";

            string answer;
            if (system.Contains("Summarize", StringComparison.OrdinalIgnoreCase))
            {
                answer =
                    "Development-mode summary (set AZURE_OPENAI_* for a real model).\n\n" +
                    "The document lists capital cities with population figures. " +
                    "Several capitals exceed 5 million residents.";
            }
            else
            {
                int idx = user.IndexOf("Question:", StringComparison.Ordinal);
                string question = idx >= 0 ? user[(idx + "Question:".Length)..].Trim() : user;
                answer =
                    "Development-mode answer (set AZURE_OPENAI_* environment variables for a real model).\n\n" +
                    "Based on indexed excerpts, the United Kingdom (London) and Turkey (Ankara) " +
                    "are NATO members whose capitals exceed 5 million people.\n\n" +
                    $"Question: {question}";
            }

            return Task.FromResult(new ChatResponse(new ChatMessage(ChatRole.Assistant, answer)));
        }

        public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public object? GetService(Type serviceType, object? serviceKey = null) => null;

        public void Dispose() { }
    }
}
