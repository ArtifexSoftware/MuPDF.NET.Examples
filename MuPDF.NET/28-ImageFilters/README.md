# 28-ImageFilters

Apply an `ImageFilterPipeline` (median) to a pixmap and save a PNG.

## Sample method

`ImageFilters()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) (includes SkiaSharp filters)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/apple.png` |
| Output | `filtered.png` |
| Expected | `filtered.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\28-ImageFilters
```

## APIs used

- `ImageFilterPipeline` / `Pixmap.ApplyImageFilters`
- `ImageProcessingFilterType.Median`
