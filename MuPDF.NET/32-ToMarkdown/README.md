# 32-ToMarkdown

Convert a PDF to Markdown (classic RAG path with layout off). Uses `Document.ToMarkdown` from **MuPDF.NET 4.1**.

## Sample method

`ToMarkdown()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/columns.pdf` |
| Output | `Output/MuPDF.NET/32-ToMarkdown/columns.md` |
| Expected | `Expected/columns.md` |

## Run

```powershell
dotnet run --project MuPDF.NET\32-ToMarkdown
```

## APIs used

- `Utils.UseLayout(false)`
- `Document.ToMarkdown`

## Related

- [`40-LoadAi`](../40-LoadAi/) — index that Markdown, then Ask / Search
