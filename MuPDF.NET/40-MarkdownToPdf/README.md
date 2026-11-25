# 40-MarkdownToPdf

Render a Markdown file to PDF with `Utils.MarkdownToPdf` (Story).

Uses a dedicated `north-pier.md` (heading, paragraph, list, berth table). `pageRect` is 500×400 with 36 pt margins so the page is compact for the docs image (the API default is A4). When `outputPath` is set, the PDF is written to disk and the return value is null.

## Sample method

`MarkdownToPdf()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/north-pier.md` |
| Output | `Output/MuPDF.NET/40-MarkdownToPdf/north-pier.pdf`, `page.png` |
| Expected | `north-pier.summary.txt` + dump + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\40-MarkdownToPdf
```

Recreate the fixture: `python Test/create_north_pier_md.py` from `D:\Artifex\Work`.

## APIs used

- `Utils.MarkdownToPdf` (`pageRect`, `margins`, `outputPath`)

## Related

- [`32-ToMarkdown`](../32-ToMarkdown/) — PDF to Markdown (the reverse)
- [`06-StoryHtmlBox`](../06-StoryHtmlBox/) — Story layout from HTML, not Markdown
