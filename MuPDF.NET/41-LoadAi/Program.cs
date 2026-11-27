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

    // help:skip — resolves fixtures, calls the documented overloads, then checks the harness baseline.
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

        bool prior = Utils.LayoutEnabled;
        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                Utils.UseLayout(false);
                await LoadAiAskSearch(new[] { orders, schedule }, output);
                await LoadAiAddDocuments(orders, schedule);
                await LoadAiRemoveDocument(orders, schedule);
                await LoadAiFilterSource(orders, schedule);
            }
            finally
            {
                Console.SetOut(previous);
                Utils.UseLayout(prior);
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

    // doc-start:LoadAiAskSearch
    /// <summary>Index PDFs with Utils.LoadAiAsync, then Ask / Summarize / Search</summary>
    static async Task LoadAiAskSearch(string[] inputPaths, string outputPath)
    {
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
    // doc-end:LoadAiAskSearch

    // doc-start:LoadAiAddDocuments
    /// <summary>Index more PDFs with AddDocumentsAsync</summary>
    static async Task LoadAiAddDocuments(string ordersPath, string schedulePath)
    {
        var options = Utils.CreateDevelopmentAiOptions(chatClient: new ExampleRagChatClient());
        options.UseLayout = false;

        var aiDocs = await Utils.LoadAiAsync(new[] { ordersPath }, options);
        Console.WriteLine("--- add ---");
        Console.WriteLine($"before chunkCount={aiDocs.ChunkCount} sources={string.Join(",", aiDocs.SourceFiles.OrderBy(s => s, StringComparer.Ordinal))}");
        await aiDocs.AddDocumentsAsync(new[] { schedulePath });
        Console.WriteLine($"after chunkCount={aiDocs.ChunkCount} sources={string.Join(",", aiDocs.SourceFiles.OrderBy(s => s, StringComparer.Ordinal))}");
    }
    // doc-end:LoadAiAddDocuments

    // doc-start:LoadAiRemoveDocument
    /// <summary>Drop one source PDF with RemoveDocumentAsync</summary>
    static async Task LoadAiRemoveDocument(string ordersPath, string schedulePath)
    {
        var options = Utils.CreateDevelopmentAiOptions(chatClient: new ExampleRagChatClient());
        options.UseLayout = false;

        var aiDocs = await Utils.LoadAiAsync(new[] { ordersPath, schedulePath }, options);
        Console.WriteLine("--- remove ---");
        Console.WriteLine($"before chunkCount={aiDocs.ChunkCount} sources={string.Join(",", aiDocs.SourceFiles.OrderBy(s => s, StringComparer.Ordinal))}");
        await aiDocs.RemoveDocumentAsync("pier-orders.pdf");
        Console.WriteLine($"after chunkCount={aiDocs.ChunkCount} sources={string.Join(",", aiDocs.SourceFiles.OrderBy(s => s, StringComparer.Ordinal))}");
    }
    // doc-end:LoadAiRemoveDocument

    // doc-start:LoadAiFilterSource
    /// <summary>Limit Ask / Search / Summarize to one source file</summary>
    static async Task LoadAiFilterSource(string ordersPath, string schedulePath)
    {
        var options = Utils.CreateDevelopmentAiOptions(chatClient: new ExampleRagChatClient());
        options.UseLayout = false;

        var aiDocs = await Utils.LoadAiAsync(new[] { ordersPath, schedulePath }, options);
        const string question = "Which berth is open for the timber barge?";
        var hits = await aiDocs.SearchAsync("timber barge", topK: 3, sourceFileName: "pier-schedule.pdf");
        string answer = await aiDocs.AskAsync(question, sourceFileName: "pier-schedule.pdf");
        string pageSummary = await aiDocs.SummarizeAsync("pier-schedule.pdf", pageNumber: 1);

        Console.WriteLine("--- filter ---");
        Console.WriteLine($"askSource=pier-schedule.pdf");
        Console.WriteLine(answer.Trim());
        Console.WriteLine($"summarizePage=1");
        Console.WriteLine(pageSummary.Trim());
        Console.WriteLine("rank  source              page");
        Console.WriteLine("----  ------------------  ----");
        for (int i = 0; i < hits.Count; i++)
        {
            var hit = hits[i];
            Console.WriteLine(
                $"{i + 1,-4}  {hit.Chunk.SourceFileName,-18}  {hit.Chunk.PageNumber}");
        }
    }
    // doc-end:LoadAiFilterSource

    // doc-start:ExampleRagChatClient
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
            string user = messages.FirstOrDefault(m => m.Role == ChatRole.User)?.Text ?? "";
            bool schedule = user.Contains("pier-schedule", StringComparison.OrdinalIgnoreCase);
            bool summarize = system.Contains("Summarize", StringComparison.OrdinalIgnoreCase);
            string answer = summarize
                ? schedule
                    ? "Development-mode summary (no API keys).\n\n"
                      + "Berth schedule: berth 4 timber barge is open at 07:40. Berth 5 Pilot 2 is due at 12:10."
                    : "Development-mode summary (no API keys).\n\n"
                      + "Standing orders for North Pier: tide 07:40, wind SW 12 kn. Berth 4 is open. Unload timber before noon."
                : schedule
                    ? "Development-mode answer (no API keys).\n\n"
                      + "Berth 4 is open for the timber barge (ETA 07:40)."
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
    // doc-end:ExampleRagChatClient
}
