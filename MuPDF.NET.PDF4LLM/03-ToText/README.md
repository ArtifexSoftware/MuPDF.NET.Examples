# 03-ToText

Convert a PDF to plain text via layout. Requires **pymupdf-layout**.

## Sample method

`ToText()` in `Program.cs`.

## Package

- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)

## Prerequisites

- Same layout provider as `02-ToJsonLayout`. Without it, the sample records `LAYOUT_UNAVAILABLE`.

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

- `MuPDF4LLM.SetUseLayout(true)`
- `MuPDF4LLM.ToText`
