# 42-OllamaMagazines

Index `Magazines-PDF.pdf` (a primer on magazine types, uses, parts, and sizes) with **local Ollama**, then **Ask** and **Search**.

Install Ollama and pull models first — see **[`41-Ollama/README.md`](../41-Ollama/README.md)** (Windows / macOS / Linux install, `ollama pull mistral`, `ollama pull nomic-embed-text`).

## Sample method

`OllamaMagazines()` in `Program.cs`. Shared host: [`OllamaRag.cs`](../41-Ollama/OllamaRag.cs).

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1
- [OllamaSharp](https://www.nuget.org/packages/OllamaSharp)
- [Microsoft.Extensions.AI](https://www.nuget.org/packages/Microsoft.Extensions.AI)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/Magazines-PDF.pdf` |
| Output | `Output/MuPDF.NET/42-OllamaMagazines/ollama.txt` |
| Expected | none (live model text is not baselined) |

## Run

```powershell
dotnet run --project MuPDF.NET\42-OllamaMagazines
dotnet run --project MuPDF.NET\42-OllamaMagazines -- --repl
```

Without Ollama, `dotnet run` prints `SKIP —`. `run-all` always skips this project (live model; run it directly).

Same environment variables as [`41-Ollama`](../41-Ollama/): `OLLAMA_HOST`, `OLLAMA_MODEL`, `OLLAMA_EMBED_MODEL`.

## Canned questions

Definition of a magazine; the ten types; fashion magazines; Rolling Stone as a lifestyle example; masthead contents; magazine vs newspaper; magazine vs journal vs catalog; US/UK sizes (8.5"×11" and 5.5"×8.5"); pulp paper; plus **What is the melting point of unobtainium?** (should abstain).

## APIs used

- `Utils.CreateAiOptions` / `Utils.LoadAiAsync`
- `AskAsync` / `SearchAsync` on the returned collection
- `OllamaSharp.OllamaApiClient`

## Related

- [`41-Ollama`](../41-Ollama/) — Ollama install + capitals PDF
- [`40-LoadAi`](../40-LoadAi/) — `LoadAiAsync` without Ollama
