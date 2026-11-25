# 27-ImageToPdf

Wrap a PNG as a one-page PDF sized to the image (`Document.NewPage` + `Page.InsertImage`).

To **place an image on a page that already has a size**, see [`09-InsertImage`](../09-InsertImage/).

## Sample method

`ImageToPdf()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/image-to-pdf.png` |
| Output | `Output/MuPDF.NET/27-ImageToPdf/wrapped.pdf`, `source.png`, `page.png` |
| Expected | `image-to-pdf.txt` + PDF fingerprint + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\27-ImageToPdf
```

Recreate the fixture: `python Test/create_image_to_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `Pixmap` (from image file)
- `Document.NewPage`
- `Page.InsertImage`

## Related

- [`09-InsertImage`](../09-InsertImage/) — place a new image on a fixed-size page
- [`26-Watermark`](../26-Watermark/) — stamp an image on every page
- [`28-ImageFilters`](../28-ImageFilters/) — median-filter a pixmap
