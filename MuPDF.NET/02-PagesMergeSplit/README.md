# 02-PagesMergeSplit

Merge pages from two PDFs, then extract the first page into a new document.

## Sample method

`PagesMergeSplit()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/sample.pdf`, `Blank.pdf` |
| Output | `merged.pdf`, `first-page.pdf` under `Output/MuPDF.NET/02-PagesMergeSplit/` |
| Expected | `merged.summary.txt`, `first-page.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\02-PagesMergeSplit
```

## APIs used

- `Document.InsertPdf`
- `Document.Save`
