# 08-MarkdownToPdf

Render a Markdown file to PDF using Story.

## Sample method

`MarkdownToPdf()` in `Program.cs`.

## Package

- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/sample.md` |
| Output | `Output/MuPDF.NET.PDF4LLM/08-MarkdownToPdf/sample.pdf` |
| Expected | `Expected/sample.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\08-MarkdownToPdf
```

## APIs used

- `MuPDF4LLM.MarkdownToPdf`
