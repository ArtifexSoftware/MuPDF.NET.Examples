# 32-ToMarkdown

Convert a PDF to Markdown (classic extract with layout off). Uses `Document.ToMarkdown`.

To **extract plain text** with `Page.GetText`, see [`04-TextExtractSearch`](../04-TextExtractSearch/).

## Sample method

`ToMarkdown()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/bulletin.pdf` |
| Output | `Output/MuPDF.NET/32-ToMarkdown/to-markdown.md`, `page.png` |
| Expected | `to-markdown.md` + summary + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\32-ToMarkdown
```

Recreate the fixture: `python Test/create_bulletin_pdf.py` from `D:\Artifex\Work`. Same PDF as [`33-ToJsonLayout`](../33-ToJsonLayout/).

## APIs used

- `Utils.UseLayout(false)`
- `Document.ToMarkdown`

## Related

- [`04-TextExtractSearch`](../04-TextExtractSearch/) — `Page.GetText` / `SearchFor`
- [`11-Tables`](../11-Tables/) — `Table.ToMarkdown`
- [`33-ToJsonLayout`](../33-ToJsonLayout/) — same PDF, layout JSON (`UseLayout(true)`)
- [`34-ToText`](../34-ToText/) — same PDF, layout plain text
- [`35-ToChunks`](../35-ToChunks/) — same PDF, retrieval chunks
- [`36-Ocr`](../36-Ocr/) — scanned page, `useOcr` on vs off
- [`38-LlamaMarkdownReader`](../38-LlamaMarkdownReader/) — one LlamaIndex document per page
- [`40-MarkdownToPdf`](../40-MarkdownToPdf/) — Markdown to PDF (the reverse)
