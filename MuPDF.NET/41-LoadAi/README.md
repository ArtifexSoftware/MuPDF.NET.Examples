# 41-LoadAi

Index PDFs with `Utils.LoadAiAsync`, then **Ask**, **Summarize**, and **Search**.

Uses two dedicated North Pier PDFs (`pier-orders.pdf`, `pier-schedule.pdf`). `Utils.UseLayout(false)` selects the classic extract path. `CreateDevelopmentAiOptions` uses hash embeddings and an in-memory index. `ExampleRagChatClient` is a local `IChatClient` so `dotnet run` needs **no API keys**. Swap that client for OpenAI, Azure, or Ollama — the MuPDF.NET extract/RAG API stays the same.

## Sample method

`LoadAi()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)
- [Microsoft.Extensions.AI](https://www.nuget.org/packages/Microsoft.Extensions.AI)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/pier-orders.pdf`, `pier-schedule.pdf` |
| Output | `Output/MuPDF.NET/41-LoadAi/load-ai.txt`, `orders.png`, `schedule.png` |
| Expected | `load-ai.txt` + dump + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\41-LoadAi
```

Recreate the fixtures: `python Test/create_load_ai_pdfs.py` from `D:\Artifex\Work`.

## APIs used

- `Utils.UseLayout(false)`
- `Utils.CreateDevelopmentAiOptions`
- `Utils.LoadAiAsync`
- `AskAsync` / `SummarizeAsync` / `SearchAsync`

## Related

- [`32-ToMarkdown`](../32-ToMarkdown/) — PDF to Markdown (the text this pipeline indexes)
- [`35-ToChunks`](../35-ToChunks/) — layout retrieval chunks, no embed/ask
- [`38-LlamaMarkdownReader`](../38-LlamaMarkdownReader/) — one LlamaIndex document per page, no embed/ask
