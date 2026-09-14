# 04-ExportToMarkdownJson

Export an Office document to Markdown and JSON.

## Sample method

`ExportToMarkdownJson()` in `Program.cs`.

## Package

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)

## Prerequisites

- `MuPDFOffice.Unlock` first.
- Optional `MUPDF_OFFICE_KEY`.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.Office/pages.docx` |
| Output | `pages.md`, `pages.json` under `Output/MuPDF.NET.Office/04-ExportToMarkdownJson/` |
| Expected | `pages.md`, `pages.summary.txt` |

`pages.json` is still written for inspection. The golden check uses
`pages.summary.txt` (page size + text) because SmartOffice font substitution
differs by OS (e.g. Arial / Liberation Sans on Windows vs Liberation Serif on
Linux), which also changes glyph metrics in the full JSON.

To improve visual font parity on Linux, install Liberation fonts
(`fonts-liberation` / `fonts-liberation2` on Debian/Ubuntu). Exact Windows
font names still will not match unless you ship and force the same TTF files
via `MuPDFOffice.Unlock(..., fontPath: ...)`.

## Run

```bash
dotnet run --project MuPDF.NET.Office/04-ExportToMarkdownJson
```

## APIs used

- `MuPDFOffice.ToMarkdown`
- `MuPDFOffice.ToJson`
