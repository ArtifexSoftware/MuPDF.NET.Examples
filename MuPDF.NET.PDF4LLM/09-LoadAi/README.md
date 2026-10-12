# 09-LoadAi

Index PDFs into a RAG pipeline (`LoadAiAsync`), then **search**, **ask**, and **summarize**. This is why MuPDF.NET.PDF4LLM exists: extract clean page text, then hand it to `Microsoft.Extensions.AI` (`IEmbeddingGenerator` / `IChatClient`).

Default run needs **no API keys**. It uses `CreateForDevelopment()` (hash embeddings + in-memory index) and a local `IChatClient` so `dotnet run` and `run-all` always PASS.

If all of `AZURE_OPENAI_ENDPOINT`, `AZURE_OPENAI_API_KEY`, `AZURE_OPENAI_CHAT_DEPLOYMENT`, and `AZURE_OPENAI_EMBEDDING_DEPLOYMENT` are set, the **same** `AskAsync` / `SearchAsync` calls use Azure OpenAI (optional Azure AI Search via `AZURE_SEARCH_*`). Live model text is written to `Output/` and is **not** compared to `Expected/`.

Swap the stub `IChatClient` for OpenAI, Azure, Ollama, or any `Microsoft.Extensions.AI` client. The MuPDF.NET.PDF4LLM part stays the same. Ready-made Ollama samples: [`10-Ollama`](../10-Ollama/) and [`11-OllamaMagazines`](../11-OllamaMagazines/).

## Sample method

`LoadAi()` in `Program.cs`.

## Package

- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)

## Input / output

| | Path |
|--|------|
| Input | `national-capitals.pdf`, `nato-members.pdf` |
| Output | `load-ai.txt` |
| Expected | `index.summary.txt` (+ `ask.txt` / `summarize.txt` / `search.txt` in development mode) |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\09-LoadAi
```

## APIs used

- `MuPDF4LLM.LoadAiAsync`
- `MicrosoftAIConnectorOptions.CreateForDevelopment` (or `AzureAiOptions.ToConnectorOptions`)
- `AiDocumentCollection.AskAsync` / `SummarizeAsync` / `SearchAsync`

## Related

- [`01-ToMarkdown`](../01-ToMarkdown/) — the Markdown this pipeline indexes
- [`06-LlamaMarkdownReader`](../06-LlamaMarkdownReader/) — LlamaIndex-shaped chunks without embed/ask
- [`10-Ollama`](../10-Ollama/) — same pipeline with a local Ollama chat + embedding model
