# 34-ToText

Convert a PDF to plain text via layout. Requires the **pymupdf 2.1** layout worker.

## Sample method

`ToText()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1

## Prerequisites

- Same layout setup as `33-ToJsonLayout` ([MuPDF.NET README — Layout analysis](https://github.com/ArtifexSoftware/MuPDF.NET#layout-analysis-optional)). Without it, the sample records `LAYOUT_UNAVAILABLE`.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/columns.pdf` |
| Output | `Output/MuPDF.NET/34-ToText/columns.txt` |
| Expected | `layout-status.txt`, `columns.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\34-ToText
```

## APIs used

- `Utils.UseLayout(true)`
- `Document.ToText`
