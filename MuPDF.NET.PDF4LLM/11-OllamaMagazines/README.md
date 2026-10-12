# 11-OllamaMagazines

Index `Magazines-PDF.pdf` (a primer on magazine types, uses, parts, and sizes) with **local Ollama**, then **Ask** and **Search**.

Install Ollama and pull models first — see **[`10-Ollama/README.md`](../10-Ollama/README.md)** (Windows / macOS / Linux install, `ollama pull mistral`, `ollama pull nomic-embed-text`).

## Sample method

`OllamaMagazines()` in `Program.cs`. Shared host: [`OllamaRag.cs`](../10-Ollama/OllamaRag.cs).

## Package

- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)
- [OllamaSharp](https://www.nuget.org/packages/OllamaSharp)
- [Microsoft.Extensions.AI](https://www.nuget.org/packages/Microsoft.Extensions.AI)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/Magazines-PDF.pdf` |
| Output | `Output/MuPDF.NET.PDF4LLM/11-OllamaMagazines/ollama.txt` |
| Expected | none (live model text is not baselined) |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\11-OllamaMagazines
dotnet run --project MuPDF.NET.PDF4LLM\11-OllamaMagazines -- --repl
```

Without Ollama, `dotnet run` prints `SKIP —`. `run-all` always skips this project (live model; run it directly).

Same environment variables as [`10-Ollama`](../10-Ollama/): `OLLAMA_HOST`, `OLLAMA_MODEL`, `OLLAMA_EMBED_MODEL`.

## Canned questions

Definition of a magazine; the ten types; fashion magazines; Rolling Stone as a lifestyle example; masthead contents; magazine vs newspaper; magazine vs journal vs catalog; US/UK sizes (8.5"×11" and 5.5"×8.5"); pulp paper; plus **What is the melting point of unobtainium?** (should abstain).

## APIs used

- `MuPDF4LLM.LoadAiAsync`
- `MicrosoftAIConnectorOptions` (`ChatClient`, `EmbeddingGenerator`, `UseLayout`)
- `AiDocumentCollection.AskAsync` / `SearchAsync`
- `OllamaSharp.OllamaApiClient`

## Related

- [`10-Ollama`](../10-Ollama/) — Ollama install + capitals PDF
- [`09-LoadAi`](../09-LoadAi/) — `LoadAiAsync` without Ollama
