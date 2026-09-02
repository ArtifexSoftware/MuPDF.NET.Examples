# 03-ExportToPdf

Export an Office document (DOCX) to PDF.

## Sample method

`ExportToPdf()` in `Program.cs`.

## Package

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)

## Prerequisites

- `MuPDFOffice.Unlock` first.
- Optional `MUPDF_OFFICE_KEY`.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.Office/pages.docx` |
| Output | `Output/MuPDF.NET.Office/03-ExportToPdf/pages.pdf` |
| Expected | `Expected/pages.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET.Office\03-ExportToPdf
```

## APIs used

- `MuPDFOffice.Unlock`
- `MuPDFOffice.ToPdf`
