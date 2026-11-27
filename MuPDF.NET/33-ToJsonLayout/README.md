# 33-ToJsonLayout

Convert a PDF to layout JSON with `Document.ToJson`. Requires `MuPDF4LLM.SetLayout(true)` and the optional layout worker.

Uses the same `bulletin.pdf` as [`32-ToMarkdown`](../32-ToMarkdown/).

## Sample method

`ToJsonLayout()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Prerequisites

- One-time layout setup: **Python 3.11+** on `PATH`, then `dotnet msbuild -t:MuPDFNetSetupLayoutPython` so `MuPDF4LLM.LayoutAvailable` is true. Without layout, the sample prints `LAYOUT_UNAVAILABLE` and skips JSON.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/bulletin.pdf` |
| Output | `Output/MuPDF.NET/33-ToJsonLayout/bulletin.json`, `page.png` |
| Expected | box listing + `bulletin.json` + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\33-ToJsonLayout
```

Recreate the fixture: `python Test/create_bulletin_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `MuPDF4LLM.LayoutAvailable`
- `MuPDF4LLM.SetLayout(true)`
- `Document.ToJson`

## Related

- [`32-ToMarkdown`](../32-ToMarkdown/) — same PDF, classic extract (`MuPDF4LLM.SetLayout(false)`)
- [`34-ToText`](../34-ToText/) — same PDF, layout plain text
- [`35-ToChunks`](../35-ToChunks/) — retrieval chunks from the same layout worker
