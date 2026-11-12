# 33-ToJsonLayout

Convert a PDF to layout JSON. Requires the optional **pymupdf 2.1** layout worker.

## Sample method

`ToJsonLayout()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1

## Prerequisites

- One-time layout setup: `dotnet msbuild -t:MuPDFNetSetupLayoutPython` so `Utils.LayoutAvailable` is true. Full steps: [MuPDF.NET README — Layout analysis](https://github.com/ArtifexSoftware/MuPDF.NET#layout-analysis-optional).
- Without layout, the sample records `LAYOUT_UNAVAILABLE` and skips JSON.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/columns.pdf` |
| Output | `Output/MuPDF.NET/33-ToJsonLayout/columns.json` |
| Expected | `layout-status.txt`, `columns.json` |

## Run

```powershell
dotnet run --project MuPDF.NET\33-ToJsonLayout
```

## APIs used

- `Utils.LayoutAvailable`
- `Utils.UseLayout(true)`
- `Document.ToJson`
