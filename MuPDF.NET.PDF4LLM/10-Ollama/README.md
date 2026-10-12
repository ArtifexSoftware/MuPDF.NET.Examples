# 10-Ollama

Index `national-capitals.pdf` with **[Ollama](https://ollama.com)** as the `IChatClient` and `IEmbeddingGenerator`, then **Ask** and **Search** through `MuPDF4LLM.LoadAiAsync`.

This is the same RAG path as [`09-LoadAi`](../09-LoadAi/), but the chat and embedding models run **locally**. Default `dotnet run` / `run-all` **SKIP** when Ollama is not running or the models are not pulled — no API keys required.

You need **two** Ollama models: a chat model (`mistral`) and a separate embedding model (`nomic-embed-text`). Chat models typically cannot embed.

## Install Ollama

### Windows

1. Download the installer from [https://ollama.com/download](https://ollama.com/download) (or `winget install Ollama.Ollama`).
2. Run the installer. The Ollama app starts a server at `http://localhost:11434`.
3. Open a terminal and confirm:

```powershell
ollama --version
curl http://localhost:11434/api/tags
```

If the app is not running, start it from the Start menu, or:

```powershell
ollama serve
```

### macOS

```bash
brew install ollama
# or download https://ollama.com/download
ollama serve
```

### Linux

```bash
curl -fsSL https://ollama.com/install.sh | sh
ollama serve
```

The install script adds a systemd user service on many distros. `curl http://localhost:11434/api/tags` should return JSON.

## Pull the models

```powershell
ollama pull mistral
ollama pull nomic-embed-text
```

| Model | Role | Notes |
|-------|------|--------|
| `mistral` | Chat (`AskAsync`) | 8k context; override with `OLLAMA_MODEL` (`llama3.1`, `qwen3`, …) |
| `nomic-embed-text` | Embeddings (index + search) | Required; `mistral` cannot embed |

List what is installed:

```powershell
ollama list
```

## Sample method

`OllamaCapitals()` in `Program.cs`. Shared host: `OllamaRag.RunAsync` in `OllamaRag.cs`.

## Package

- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)
- [OllamaSharp](https://www.nuget.org/packages/OllamaSharp) (`IChatClient` / `IEmbeddingGenerator` for Ollama)
- [Microsoft.Extensions.AI](https://www.nuget.org/packages/Microsoft.Extensions.AI)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/national-capitals.pdf` |
| Output | `Output/MuPDF.NET.PDF4LLM/10-Ollama/ollama.txt` |
| Expected | none (live model text is not baselined) |

## Run

From the **MuPDF.NET.Examples** repo root, after Ollama is up and the two models are pulled:

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\10-Ollama
```

Interactive extra questions (after the canned Ask / Search list):

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\10-Ollama -- --repl
```

Without Ollama, `dotnet run` prints `SKIP —`. `run-all` always skips this project (live model; run it directly).

## Environment

| Variable | Default | Meaning |
|----------|---------|---------|
| `OLLAMA_HOST` | `http://localhost:11434` | Ollama API base URL |
| `OLLAMA_MODEL` | `mistral` | Chat model |
| `OLLAMA_EMBED_MODEL` | `nomic-embed-text` | Embedding model |

## Canned questions

Facts from the capitals table (South Sudan / Juba, Canada, Gibraltar, Tanzania, Vatican City, United States) plus **What is the melting point of unobtainium?** which the PDF cannot answer (the model should abstain).

## APIs used

- `MuPDF4LLM.LoadAiAsync`
- `MicrosoftAIConnectorOptions` (`ChatClient`, `EmbeddingGenerator`, `UseLayout`)
- `AiDocumentCollection.AskAsync` / `SearchAsync`
- `OllamaSharp.OllamaApiClient`

## Troubleshooting

| Symptom | What to do |
|---------|------------|
| `SKIP — Cannot reach Ollama` | Start the Ollama app or `ollama serve`. Check `OLLAMA_HOST`. |
| `SKIP — Model 'mistral' is not installed` | `ollama pull mistral` (or your `OLLAMA_MODEL`) |
| `That model cannot embed` / HTTP 501 | Pull `nomic-embed-text`. Do not use the chat model as the embedder. |
| Indexing is slow or the connection resets | Local models are heavy; keep `nomic-embed-text` for embed and `mistral` for chat. Close other GPU apps. |
| Answers ignore the PDF | Ask uses retrieved excerpts only. Try Search first to see which chunks ranked. |

## Related

- [`09-LoadAi`](../09-LoadAi/) — same `LoadAiAsync` path with a stub client or Azure OpenAI
- [`11-OllamaMagazines`](../11-OllamaMagazines/) — same Ollama wiring on `Magazines-PDF.pdf`
