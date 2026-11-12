# 37-LlamaMarkdownReader

Load LlamaIndex-compatible documents (one per page) via `PDFMarkdownReader`.

## Sample method

`LlamaMarkdownReader()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/columns.pdf` |
| Output | `Output/MuPDF.NET/37-LlamaMarkdownReader/llama-docs.txt` |
| Expected | `Expected/llama-docs.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\37-LlamaMarkdownReader
```

## APIs used

- `Utils.UseLayout(false)` — classic RAG markdown (stable Expected/)
- `PDFMarkdownReader.LoadData`
- `LlamaIndexDocument.Text` / `ExtraInfo`

## Related

- [`40-LoadAi`](../40-LoadAi/) — embed those chunks and Ask / Search
