# 03-ToText

Convert a PDF to plain text via layout. Requires the **pymupdf 2.0** layout worker.

## Sample method

`ToText()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.0 (via `Common`)

## Prerequisites

- Same layout setup as `02-ToJsonLayout`. Without it, the sample records `LAYOUT_UNAVAILABLE`.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/columns.pdf` |
| Output | `Output/MuPDF.NET.PDF4LLM/03-ToText/columns.txt` |
| Expected | `layout-status.txt`, `columns.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\03-ToText
```

## APIs used

- `Utils.UseLayout(true)`
- `Document.ToText`
