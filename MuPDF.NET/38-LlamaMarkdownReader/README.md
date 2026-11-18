# 38-LlamaMarkdownReader

Load LlamaIndex-compatible documents (one per page) via `Utils.LlamaMarkdownReader`.

## Sample method

`LlamaMarkdownReader()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/columns.pdf` |
| Output | `Output/MuPDF.NET/38-LlamaMarkdownReader/llama-docs.txt` |
| Expected | `Expected/llama-docs.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\38-LlamaMarkdownReader
```

## APIs used

- `Utils.UseLayout(false)` — classic RAG markdown (stable Expected/)
- `Utils.LlamaMarkdownReader`
- `LoadData` — one LlamaIndex document per page (`Text` / `ExtraInfo`)

## Related

- [`41-LoadAi`](../41-LoadAi/) — embed those chunks and Ask / Search
