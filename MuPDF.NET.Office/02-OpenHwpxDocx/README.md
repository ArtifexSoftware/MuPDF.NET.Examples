# 02-OpenHwpxDocx

Unlock Office, then open DOCX and HWPX with `Document.Open`.

`pier-notice.docx` is a three-page North Pier notice (standing orders, berth schedule, tide note). Restricted mode still loads all three pages. `pier-notice.hwpx` is a one-page Hancom HWPX berth notice. The sample never prints the license key. Page text in the listing is collapsed whitespace so the baseline stays portable.

## Sample method

`OpenHwpxDocx()` in `Program.cs`.

## Package

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)

## Prerequisites

- Call `MuPDFOffice.Unlock` before opening Office formats.
- Optional `MUPDF_OFFICE_KEY`. Empty / unset is restricted mode (first 3 pages).

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.Office/pier-notice.docx`, `pier-notice.hwpx` |
| Output | `Output/MuPDF.NET.Office/02-OpenHwpxDocx/open.txt` |
| Expected | `open.txt` + dump |

Recreate the fixtures: `python Test/create_office_open_docs.py` from the Work root.

## Run

```powershell
dotnet run --project MuPDF.NET.Office\02-OpenHwpxDocx
```

## APIs used

- `MuPDFOffice.Unlock` (`fontPathAuto` is `true`)
- `Document.Open` on DOCX / HWPX paths
