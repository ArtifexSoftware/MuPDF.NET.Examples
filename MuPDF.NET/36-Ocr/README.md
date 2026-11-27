# 36-Ocr

Compare Markdown extraction with OCR off vs on (`Document.ToMarkdown`). Requires `MuPDF4LLM.SetLayout(true)`, the optional layout worker, and an OCR engine.

Uses a dedicated image-only `ocr-scan.pdf` (copier scan: speckle, JPEG, faint COPY stamp). Without OCR the page has no text layer. `forceOcr: true` because a noisy scan can fail the layout heuristic. Layout tags `# NORTH PIER` when RapidOCR returns a large title box.

## Sample method

`Ocr()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Prerequisites

- Same layout setup as `33-ToJsonLayout`. Without layout, the sample prints `LAYOUT_UNAVAILABLE` and skips.
- OCR engine: RapidOCR on .NET 8, or Tesseract `TESSDATA_PREFIX`. Without either, the sample prints `OCR_UNAVAILABLE`.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/ocr-scan.pdf` |
| Output | `Output/MuPDF.NET/36-Ocr/ocr-off.md`, `ocr-on.md`, `page.png` |
| Expected | `ocr-off.md` + `ocr-on.md` + summary + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\36-Ocr
```

Recreate the fixture: `python Test/create_ocr_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `MuPDF4LLM.LayoutAvailable`
- `Utils.RapidOcrAvailable`
- `Utils.TESSDATA_PREFIX`
- `MuPDF4LLM.SetLayout(true)`
- `Document.ToMarkdown(..., useOcr: false|true, forceOcr: true)`

## Related

- [`32-ToMarkdown`](../32-ToMarkdown/) — text PDF, OCR off
- [`04-TextExtractSearch`](../04-TextExtractSearch/) — `Page.GetText` (no OCR)
