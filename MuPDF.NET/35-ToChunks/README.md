# 35-ToChunks

Split a PDF into retrieval-ready chunks (`Document.ToChunks`). Requires the optional **pymupdf 2.1** layout worker.

## Sample method

`ToChunks()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1

## Prerequisites

- Same layout setup as `33-ToJsonLayout` ([MuPDF.NET README — Layout analysis](https://github.com/ArtifexSoftware/MuPDF.NET#layout-analysis-optional)). Without it, the sample records `LAYOUT_UNAVAILABLE` and skips chunking.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/columns.pdf` |
| Output | `Output/MuPDF.NET/35-ToChunks/columns.chunks.txt` |
| Expected | `layout-status.txt`, `columns.chunks.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\35-ToChunks
```

## APIs used

- `Utils.LayoutAvailable`
- `Utils.UseLayout(true)`
- `Document.ToChunks`
- `ChunkedDocument` / `Chunk.Id` / `Chunk.Text` / `Chunk.Metadata`

## Related

- [`32-ToMarkdown`](../32-ToMarkdown/) — classic markdown (no layout worker)
- [`33-ToJsonLayout`](../33-ToJsonLayout/) — layout JSON
- [`34-ToText`](../34-ToText/) — layout plain text
- [`41-LoadAi`](../41-LoadAi/) — index pages and Ask / Search
