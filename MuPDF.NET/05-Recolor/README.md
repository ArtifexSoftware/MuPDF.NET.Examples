# 05-Recolor

Convert page 1 of `color-shapes.pdf` to CMYK with `Document.Recolor(0, 4)`.

`color-shapes.pdf` is a one-page RGB board (hue bar, swatches, circle, round rect, triangle). Recreate: `python Test/create_color_shapes_pdf.py` from `D:\Artifex\Work`. The after pixmap is a bit duller because CMYK cannot match every RGB hue.

This is **not** ICC soft-proofing / print separations — for those see [`19-ColorManagement`](../19-ColorManagement/).

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/color-shapes.pdf` |
| Output | `Output/MuPDF.NET/05-Recolor/recolor.pdf`, `before.png`, `after.png` |
| Expected | `recolor.summary.txt`, `before.png.sha256`, `after.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\05-Recolor
```

## APIs used

- `Document.Open`
- `Document.Recolor(page, components)` — `1`=Gray, `3`=RGB, `4`=CMYK
- `Document.Save`
