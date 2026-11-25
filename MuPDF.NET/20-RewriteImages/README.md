# 20-RewriteImages

Downsample and recompress images in a PDF (`Document.RewriteImages`).

`rewrite-images.pdf` is a one-page fixture with four 360 DPI rasters (color PNG, color JPEG, gray PNG, bitonal PNG). The sample subsamples images above 100 DPI down to 72 DPI and recompresses at JPEG quality 33.

## Sample method

`RewriteImages()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/rewrite-images.pdf` |
| Output | `Output/MuPDF.NET/20-RewriteImages/rewritten.pdf`, `before.png`, `after.png`, `raster-before.png`, `raster-after.png` |
| Expected | `rewritten.txt`, `rewritten.summary.txt`, `before.png.sha256`, `after.png.sha256`, `raster-before.png.sha256`, `raster-after.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\20-RewriteImages
```

Recreate the fixture with `python Test/create_rewrite_images_pdf.py`.

## APIs used

- `Document.RewriteImages`
- `Page.GetImages`

## Related

| Example | Topic |
|---------|--------|
| [`09-InsertImage`](../09-InsertImage/) | Place a new image on a page |
| [`17-ReplaceImage`](../17-ReplaceImage/) | Replace an existing image by xref |
