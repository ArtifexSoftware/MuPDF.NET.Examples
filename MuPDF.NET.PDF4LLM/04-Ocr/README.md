# 04-Ocr

Compare Markdown extraction with OCR on vs off.

## Sample method

`OcrCompare()` in `Program.cs`.

## Package

- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)

## Prerequisites

- OCR needs **layout mode** (`SetUseLayout(true)`) — `useOcr` is ignored when layout is off.
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

- `MuPDF4LLM.ToMarkdown(..., useOcr: false|true)`
