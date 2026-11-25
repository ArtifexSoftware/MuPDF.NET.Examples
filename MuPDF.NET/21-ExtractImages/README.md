# 21-ExtractImages

List images on a page (`Page.GetImages`) and write each stream to disk (`Document.ExtractImage`).

`extract-images.pdf` is a one-page fixture with three rasters (color PNG, color JPEG, gray PNG). `ExtractImage` keeps the original format, so the JPEG stays JPEG instead of being converted to PNG.

## Sample method

`ExtractImages()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/extract-images.pdf` |
| Output | `Output/MuPDF.NET/21-ExtractImages/img-*.png` / `.jpeg`, `extract-images.png` |
| Expected | `extract-images.txt` + per-file `.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\21-ExtractImages
```

Recreate the fixture with `python Test/create_extract_images_pdf.py`.

## APIs used

- `Page.GetImages`
- `Document.ExtractImage`

## Related

| Example | Topic |
|---------|--------|
| [`09-InsertImage`](../09-InsertImage/) | Place a new image on a page |
| [`17-ReplaceImage`](../17-ReplaceImage/) | Replace an existing image by xref |
| [`20-RewriteImages`](../20-RewriteImages/) | Downsample / recompress images in place |
