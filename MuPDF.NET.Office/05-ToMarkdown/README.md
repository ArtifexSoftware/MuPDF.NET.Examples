# 05-ToMarkdown

Unlock Office, then run `Document.ToMarkdown` on an HWPX path (same extract API as PDF).

`pier-notice.hwpx` is the same one-page Hancom HWPX berth notice as 02-OpenHwpxDocx. `Utils.UseLayout(false)` selects the classic extract path (no layout worker). `useOcr` is `false` because this document is already text. This is `Document.ToMarkdown` after Unlock, not `MuPDFOffice.ToMarkdown`. The listing is UTF-8 without a BOM. The sample never prints the license key.

## Sample method

`ToMarkdown()` in `Program.cs`.

## Package

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)

## Prerequisites

- Call `MuPDFOffice.Unlock` before opening HWPX/DOCX with `Document`.
- Optional `MUPDF_OFFICE_KEY`. Empty / unset is restricted mode (first 3 pages).

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.Office/pier-notice.hwpx` |
| Output | `Output/MuPDF.NET.Office/05-ToMarkdown/pier-notice.md` |
| Expected | `pier-notice.md` + dump |

## Run

```powershell
dotnet run --project MuPDF.NET.Office\05-ToMarkdown
```

## APIs used

- `MuPDFOffice.Unlock` (`fontPathAuto` is `true`)
- `Utils.UseLayout` (`false`)
- `Document.Open`
- `Document.ToMarkdown` (`useOcr` is `false`)
