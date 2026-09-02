# 02-ToJsonLayout

Convert a PDF to layout JSON. Requires the optional **pymupdf-layout** provider.

## Sample method

`ToJsonLayout()` in `Program.cs`.

## Package

- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)

## Prerequisites

- Install [pymupdf-layout](https://pypi.org/project/pymupdf-layout/) so `MuPDF4LLM.LayoutAvailable` is true.
- Without layout, the sample records `LAYOUT_UNAVAILABLE` and skips JSON.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/columns.pdf` |
| Output | `Output/MuPDF.NET.PDF4LLM/02-ToJsonLayout/columns.json` |
| Expected | `layout-status.txt`, `columns.json` |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\02-ToJsonLayout
```

## APIs used

- `MuPDF4LLM.LayoutAvailable`
- `MuPDF4LLM.SetUseLayout(true)`
- `MuPDF4LLM.ToJson`
