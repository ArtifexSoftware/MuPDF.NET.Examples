# 04-ExportToMarkdownJson

Export an Office document to Markdown and JSON with `MuPDFOffice.ToMarkdown` and `MuPDFOffice.ToJson`.

`pier-yard.docx` is the same one-page North Pier yard board as 03-ExportToPdf (heading, mark image, status shapes, berth table). The sample never prints the license key. Markdown is compared in full. Full JSON embeds OS font names and glyph metrics, so Expected fingerprints page size + text and the sample listing only prints block type and text.

## Sample method

`ExportToMarkdownJson()` in `Program.cs`.

## Package

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)

## Prerequisites

- Call `MuPDFOffice.Unlock` before exporting Office formats.
- Optional `MUPDF_OFFICE_KEY`. Empty / unset is restricted mode (first 3 pages).

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.Office/pier-yard.docx` |
| Output | `Output/MuPDF.NET.Office/04-ExportToMarkdownJson/pier-yard.md`, `pier-yard.json` |
| Expected | `pier-yard.md`, `json.summary.txt`, dump |

## Run

```powershell
dotnet run --project MuPDF.NET.Office\04-ExportToMarkdownJson
```

## APIs used

- `MuPDFOffice.Unlock` (`fontPathAuto` is `true`)
- `MuPDFOffice.ToMarkdown`
- `MuPDFOffice.ToJson`
