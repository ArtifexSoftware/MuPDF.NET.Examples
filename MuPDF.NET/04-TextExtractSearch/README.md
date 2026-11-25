# 04-TextExtractSearch

Extract plain text from page 1 of `unicode.pdf` and search for `こんにちは`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/unicode.pdf` |
| Output | `Output/MuPDF.NET/04-TextExtractSearch/page-1.txt` |
| Expected | `page-1.txt`, `search.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\04-TextExtractSearch
```

## APIs used

- `Document.Open`
- `Page.GetText("text")`
- `Page.SearchFor`

## Related

- [`32-ToMarkdown`](../32-ToMarkdown/) — `Document.ToMarkdown` (classic extract)
- [`34-ToText`](../34-ToText/) — `Document.ToText` (layout worker)
- [`36-Ocr`](../36-Ocr/) — scanned page, layout OCR
