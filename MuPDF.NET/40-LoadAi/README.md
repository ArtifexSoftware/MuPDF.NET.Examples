# 40-LoadAi

Index PDFs into a RAG pipeline (`LoadAiAsync`), then **search**, **ask**, and **summarize**. MuPDF.NET 4.1 extracts page text and hands it to `Microsoft.Extensions.AI` (`IEmbeddingGenerator` / `IChatClient`).

Default run needs **no API keys**. It uses `Utils.CreateDevelopmentAiOptions` (hash embeddings + in-memory index) and a local `IChatClient` so `dotnet run` and `run-all` always PASS.

If all of `AZURE_OPENAI_ENDPOINT`, `AZURE_OPENAI_API_KEY`, `AZURE_OPENAI_CHAT_DEPLOYMENT`, and `AZURE_OPENAI_EMBEDDING_DEPLOYMENT` are set, the **same** `AskAsync` / `SearchAsync` calls use Azure OpenAI (optional Azure AI Search via `AZURE_SEARCH_*`). Live model text is written to `Output/` and is **not** compared to `Expected/`.

Swap the stub `IChatClient` for OpenAI, Azure, Ollama, or any `Microsoft.Extensions.AI` client. The MuPDF.NET extract/RAG API stays the same. Ready-made Ollama samples: [`41-Ollama`](../41-Ollama/) and [`42-OllamaMagazines`](../42-OllamaMagazines/).

## Sample method

`LoadAi()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1
- [Microsoft.Extensions.AI](https://www.nuget.org/packages/Microsoft.Extensions.AI)

## Input / output

| | Path |
|--|------|
| Input | `national-capitals.pdf`, `nato-members.pdf` |
| Output | `load-ai.txt` |
| Expected | `index.summary.txt` (+ `ask.txt` / `summarize.txt` / `search.txt` in development mode) |

## Run

```powershell
dotnet run --project MuPDF.NET\40-LoadAi
```

## APIs used

- `Utils.LoadAiAsync`
- `Utils.CreateDevelopmentAiOptions` (or `Utils.CreateAzureAiOptions`)
- `AskAsync` / `SummarizeAsync` / `SearchAsync` on the returned collection

## Related

- [`32-ToMarkdown`](../32-ToMarkdown/) — the Markdown this pipeline indexes
- [`37-LlamaMarkdownReader`](../37-LlamaMarkdownReader/) — LlamaIndex-shaped chunks without embed/ask
- [`41-Ollama`](../41-Ollama/) — same pipeline with a local Ollama chat + embedding model
