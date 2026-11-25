# 03-RenderPixmap

Render page 1 of `color-shapes.pdf` to a PNG pixmap at 2× zoom.

`color-shapes.pdf` is a one-page RGB board (hue bar, swatches, circle, round rect, triangle). Recreate: `python Test/create_color_shapes_pdf.py` from `D:\Artifex\Work`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/color-shapes.pdf` |
| Output | `Output/MuPDF.NET/03-RenderPixmap/page-1.png` |
| Expected | `Expected/page-1.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\03-RenderPixmap
```

## APIs used

- `Document.Open`
- `Page.GetPixmap` with `Matrix(2, 2)`
- `Pixmap.Save`

## Related

- [`31-ParallelPixmap`](../31-ParallelPixmap/) — render several PDFs in parallel
