# 39-MarkdownToPdf

Render a Markdown file to PDF using Story.

## Sample method

`MarkdownToPdf()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/sample.md` |
| Output | `Output/MuPDF.NET/39-MarkdownToPdf/sample.pdf` |
| Expected | `Expected/sample.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\39-MarkdownToPdf
```

## APIs used

- `Utils.MarkdownToPdf`
