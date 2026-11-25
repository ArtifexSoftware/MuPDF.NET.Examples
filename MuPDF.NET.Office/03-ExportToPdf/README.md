# 03-ExportToPdf

Export an Office document (DOCX) to PDF with `MuPDFOffice.ToPdf`.

`pier-yard.docx` is a one-page North Pier yard board: heading, an embedded mark image, status shapes (circle, rounded rectangle, rectangle), and a three-row berth table. The sample never prints the license key. `ToPdf` embeds OS fonts (Arial vs Liberation), so Expected compares page count and collapsed per-page text, not PDF bytes.

## Sample method

`ExportToPdf()` in `Program.cs`.

## Package

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)

## Prerequisites

- Call `MuPDFOffice.Unlock` before exporting Office formats.
- Optional `MUPDF_OFFICE_KEY`. Empty / unset is restricted mode (first 3 pages).

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.Office/pier-yard.docx` |
| Output | `Output/MuPDF.NET.Office/03-ExportToPdf/pier-yard.pdf` |
| Expected | `export.txt` + dump |

Recreate the fixture: `python Test/create_office_export_docx.py` from the Work root.

## Run

```powershell
dotnet run --project MuPDF.NET.Office\03-ExportToPdf
```

## APIs used

- `MuPDFOffice.Unlock` (`fontPathAuto` is `true`)
- `MuPDFOffice.ToPdf`
- `Document.Open` on the exported PDF
