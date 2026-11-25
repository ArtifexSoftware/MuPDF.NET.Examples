# 38-LlamaMarkdownReader

Load LlamaIndex-compatible documents (one per page) via `Utils.LlamaMarkdownReader`.

Uses a dedicated two-page `llama-pages.pdf` (North Pier standing orders + berth schedule). `Utils.UseLayout(false)` selects the classic extract path.

## Sample method

`LlamaMarkdownReader()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/llama-pages.pdf` |
| Output | `Output/MuPDF.NET/38-LlamaMarkdownReader/llama-docs.txt`, `page-1.png`, `page-2.png` |
| Expected | `llama-docs.txt` + summary + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\38-LlamaMarkdownReader
```

Recreate the fixture: `python Test/create_llama_pages_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `Utils.UseLayout(false)` — classic RAG markdown (stable Expected/)
- `Utils.LlamaMarkdownReader`
- `LoadData` — one LlamaIndex document per page (`Text` / `ExtraInfo`)

## Related

- [`32-ToMarkdown`](../32-ToMarkdown/) — whole PDF as one Markdown string
- [`35-ToChunks`](../35-ToChunks/) — layout retrieval chunks (not one-per-page)
- [`41-LoadAi`](../41-LoadAi/) — embed those chunks and Ask / Search
