# 35-ToChunks

Split a PDF into retrieval-ready chunks with `Document.ToChunks`. Requires `Utils.UseLayout(true)` and the optional layout worker.

Uses the same `bulletin.pdf` as [`32-ToMarkdown`](../32-ToMarkdown/), [`33-ToJsonLayout`](../33-ToJsonLayout/), and [`34-ToText`](../34-ToText/).

## Sample method

`ToChunks()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Prerequisites

- Same layout setup as `33-ToJsonLayout`. Without layout, the sample prints `LAYOUT_UNAVAILABLE` and skips chunking.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/bulletin.pdf` |
| Output | `Output/MuPDF.NET/35-ToChunks/bulletin.chunks.txt`, `page.png` |
| Expected | `bulletin.chunks.txt` + summary + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\35-ToChunks
```

Recreate the fixture: `python Test/create_bulletin_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `Utils.LayoutAvailable`
- `Utils.UseLayout(true)`
- `Document.ToChunks`
- `ChunkedDocument` / `Chunk.Id` / `Chunk.Text` / `Chunk.Metadata`

## Related

- [`32-ToMarkdown`](../32-ToMarkdown/) — same PDF, classic Markdown
- [`33-ToJsonLayout`](../33-ToJsonLayout/) — same PDF, layout JSON
- [`34-ToText`](../34-ToText/) — same PDF, layout plain text
- [`36-Ocr`](../36-Ocr/) — scanned page, layout OCR on vs off
- [`38-LlamaMarkdownReader`](../38-LlamaMarkdownReader/) — one LlamaIndex document per page (layout off)
