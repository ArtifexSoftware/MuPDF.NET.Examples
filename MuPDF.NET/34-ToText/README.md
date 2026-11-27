# 34-ToText

Convert a PDF to plain text with `Document.ToText`. Requires `MuPDF4LLM.SetLayout(true)` and the optional layout worker.

Uses the same `bulletin.pdf` as [`32-ToMarkdown`](../32-ToMarkdown/) and [`33-ToJsonLayout`](../33-ToJsonLayout/).

## Sample method

`ToText()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Prerequisites

- Same layout setup as `33-ToJsonLayout`. Without layout, the sample prints `LAYOUT_UNAVAILABLE` and skips text.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/bulletin.pdf` |
| Output | `Output/MuPDF.NET/34-ToText/bulletin.txt`, `page.png` |
| Expected | `bulletin.txt` + summary + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\34-ToText
```

Recreate the fixture: `python Test/create_bulletin_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `MuPDF4LLM.LayoutAvailable`
- `MuPDF4LLM.SetLayout(true)`
- `Document.ToText`

## Related

- [`04-TextExtractSearch`](../04-TextExtractSearch/) — `Page.GetText` (no layout worker)
- [`32-ToMarkdown`](../32-ToMarkdown/) — same PDF, classic Markdown
- [`33-ToJsonLayout`](../33-ToJsonLayout/) — same PDF, layout JSON
- [`35-ToChunks`](../35-ToChunks/) — same PDF, retrieval chunks
