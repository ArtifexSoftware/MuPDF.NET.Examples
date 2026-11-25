# 26-Watermark

Add a centered image watermark on each page (`Page.InsertImage`).

## Sample method

`Watermark()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/watermark.pdf`, `watermark.png` |
| Output | `Output/MuPDF.NET/26-Watermark/watermarked.pdf`, `before.png`, `page-0.png`, `page-1.png` |
| Expected | `watermark.txt` + PDF fingerprint + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\26-Watermark
```

Recreate the fixtures: `python Test/create_watermark_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `Page.InsertImage` (`filename` / `xref`, `overlay`)

## Related

- [`09-InsertImage`](../09-InsertImage/) — place a new image on a page
- [`27-ImageToPdf`](../27-ImageToPdf/) — wrap an image as a one-page PDF
