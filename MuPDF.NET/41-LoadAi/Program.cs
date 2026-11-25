using System.Linq;
using System.Text;
using Microsoft.Extensions.AI;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.LoadAi;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 41-LoadAi");
        LoadAi().GetAwaiter().GetResult();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static async Task LoadAi()
    {
        string orders = ExamplePaths.MuPdfNetInput("pier-orders.pdf");
        string schedule = ExamplePaths.MuPdfNetInput("pier-schedule.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "41-LoadAi", "load-ai.txt");
        string pngOrders = ExamplePaths.Output("MuPDF.NET", "41-LoadAi", "orders.png");
        string pngSchedule = ExamplePaths.Output("MuPDF.NET", "41-LoadAi", "schedule.png");
        var check = new ResultCheck("MuPDF.NET", "41-LoadAi");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {orders}");
        ConsoleEx.Info($"Opened: {schedule}");
        using (var doc = Document.Open(orders))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(pngOrders);
        using (var doc = Document.Open(schedule))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(pngSchedule);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                await LoadAi(new[] { orders, schedule }, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        var dump = new StringBuilder();
        foreach (string line in captured.Split('\n'))
        {
            if (line.StartsWith("Consider using the pymupdf_layout", StringComparison.Ordinal)
                || line.Contains("pymupdf-layout", StringComparison.OrdinalIgnoreCase)
                || line.Contains("greatly improved page layout", StringComparison.OrdinalIgnoreCase))
                continue;
            dump.Append(line).Append('\n');
        }
        captured = dump.ToString();
        Console.Write(captured);

        check.Text(captured, "load-ai-dump.txt");
        if (File.Exists(output))
            check.Text(File.ReadAllText(output), "load-ai.txt");
        check.FileSha256(pngOrders, "orders.png.sha256");
        check.FileSha256(pngSchedule, "schedule.png.sha256");
        check.Finish();
    }

    // doc-start:LoadAi
    /// <summary>Index PDFs with Utils.LoadAiAsync, then Ask / Summarize / Search</summary>
    static async Task LoadAi(string[] inputPaths, string outputPath)
    {
        // Remember prior layout flag so we restore it for other examples in the same process.
        bool prior = Utils.LayoutEnabled;
        try
        {
            // false = classic extract (no layout worker required).
            Utils.UseLayout(false);

            // Hash embeddings + in-memory index. Swap ExampleRagChatClient for a real IChatClient.
            var options = Utils.CreateDevelopmentAiOptions(chatClient: new ExampleRagChatClient());
            options.UseLayout = false;

            var aiDocs = await Utils.LoadAiAsync(inputPaths, options);

            const string question = "Which berth is open for the timber barge?";
            const string searchQuery = "timber barge";

            string answer = await aiDocs.AskAsync(question);
            string summary = await aiDocs.SummarizeAsync("pier-orders.pdf");
            var hits = await aiDocs.SearchAsync(searchQuery, topK: 3);

            var listing = new StringBuilder();
            listing.AppendLine($"chunkCount={aiDocs.ChunkCount}");
            listing.AppendLine($"layoutEnabled={Utils.LayoutEnabled}");
            foreach (string source in aiDocs.SourceFiles.OrderBy(s => s, StringComparer.Ordinal))
                listing.AppendLine($"source={source}");

            listing.AppendLine();
            listing.AppendLine("--- ask ---");
            listing.AppendLine($"question={question}");
            listing.AppendLine(answer.Trim());

            listing.AppendLine();
            listing.AppendLine("--- summarize ---");
            listing.AppendLine("source=pier-orders.pdf");
            listing.AppendLine(summary.Trim());

            listing.AppendLine();
            listing.AppendLine("--- search ---");
            listing.AppendLine($"query={searchQuery}");
            listing.AppendLine("rank  source              page");
            listing.AppendLine("----  ------------------  ----");
            for (int i = 0; i < hits.Count; i++)
            {
                var hit = hits[i];
                listing.AppendLine(
                    $"{i + 1,-4}  {hit.Chunk.SourceFileName,-18}  {hit.Chunk.PageNumber}");
            }

            string text = listing.ToString();
            File.WriteAllText(outputPath, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            Console.Write(text);
        }
        finally
        {
            Utils.UseLayout(prior);
        }
    }

    /// <summary>Offline IChatClient so the sample runs with no API keys.</summary>
    sealed class ExampleRagChatClient : IChatClient
    {
        public ChatClientMetadata Metadata { get; } = new("example-rag");

        public Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            string system = messages.FirstOrDefault(m => m.Role == ChatRole.System)?.Text ?? "";
            string answer = system.Contains("Summarize", StringComparison.OrdinalIgnoreCase)
                ? "Development-mode summary (no API keys).\n\n"
                  + "Standing orders for North Pier: tide 07:40, wind SW 12 kn. Berth 4 is open. Unload timber before noon."
                : "Development-mode answer (no API keys).\n\n"
                  + "Berth 4 is open for the timber barge. Unload timber before noon.";
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
    // doc-end:LoadAi
}
