# 02-ToJsonLayout

Convert a PDF to layout JSON. Requires the optional **pymupdf 2.0** layout worker.

## Sample method

`ToJsonLayout()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.0 (via `Common`)

## Prerequisites

- One-time layout setup: `dotnet msbuild -t:MuPDFNetSetupLayoutPython` (or `python` `setup_layout_python.py`) so `MuPDF4LLM.LayoutAvailable` is true.
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
- `Utils.UseLayout(true)`
- `Document.ToJson`
