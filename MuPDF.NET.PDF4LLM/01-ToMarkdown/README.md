# 01-ToMarkdown

Convert a PDF to Markdown (classic RAG path with layout off). Uses `Document.ToMarkdown` from **MuPDF.NET 4.0**.

## Sample method

`ToMarkdown()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.0 (via `Common`)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/columns.pdf` |
| Output | `Output/MuPDF.NET.PDF4LLM/01-ToMarkdown/columns.md` |
| Expected | `Expected/columns.md` |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\01-ToMarkdown
```

## APIs used

- `Utils.UseLayout(false)`
- `Document.ToMarkdown`

## Related

- [`09-LoadAi`](../09-LoadAi/) — index that Markdown, then Ask / Search
