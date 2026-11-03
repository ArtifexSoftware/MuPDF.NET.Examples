# 04-Ocr

Compare Markdown extraction with OCR on vs off.

## Sample method

`OcrCompare()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.0 (via `Common`)

## Prerequisites

- OCR needs **layout mode** (`Utils.UseLayout(true)`) — `useOcr` is ignored when layout is off.
- OCR path also needs a working Tesseract / tessdata install when `useOcr: true`.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/Ocr.pdf` |
| Output | `ocr-on.md`, `ocr-off.md` under `Output/MuPDF.NET.PDF4LLM/04-Ocr/` |
| Expected | `ocr-on.md`, `ocr-off.md` |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\04-Ocr
```

## APIs used

- `Utils.UseLayout(true)`
- `Document.ToMarkdown(..., useOcr: false|true)`
