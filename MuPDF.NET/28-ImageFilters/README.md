# 28-ImageFilters

Apply an `ImageFilterPipeline` (median) to a pixmap and save a PNG.

## Sample method

`ImageFilters()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) (includes SkiaSharp filters)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/image-filters.png` |
| Output | `Output/MuPDF.NET/28-ImageFilters/filtered.png`, `source.png` |
| Expected | `image-filters.txt` + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\28-ImageFilters
```

Recreate the fixture: `python Test/create_image_filters_png.py` from `D:\Artifex\Work`.

## APIs used

- `ImageFilterPipeline.AddMedian`
- `Pixmap.ApplyImageFilters`

## Related

- [`27-ImageToPdf`](../27-ImageToPdf/) — wrap an image as a one-page PDF
- [`05-Recolor`](../05-Recolor/) — convert page colorspace
- [`20-RewriteImages`](../20-RewriteImages/) — downsample / recompress embedded images
