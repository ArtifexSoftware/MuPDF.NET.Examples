# 02-OpenHwpxDocx

Unlock Office, then open DOCX and HWPX with `Document.Open`.

## Sample method

`OpenHwpxDocx()` in `Program.cs`.

## Package

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)

## Prerequisites

- Call `MuPDFOffice.Unlock` before opening Office formats.
- Optional `MUPDF_OFFICE_KEY`.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.Office/pages.docx`, `sample.hwpx` |
| Expected | `Expected/pages.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET.Office\02-OpenHwpxDocx
```

## APIs used

- `MuPDFOffice.Unlock`
- `Document.Open` on DOCX / HWPX paths
