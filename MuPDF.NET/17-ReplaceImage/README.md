# 17-ReplaceImage

Replace the first image on `images.pdf` with `Page.ReplaceImage`.

`images.pdf` is a two-image fixture (`triangle.png` and `rectangle.png`). The sample swaps the first xref (`triangle.png`) with `circle.png`. `ReplaceImage` keeps the old draw matrix.

To **add** a new image, see [`09-InsertImage`](../09-InsertImage/).

## Sample method

`ReplaceImage()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/images.pdf`, `circle.png` |
| Output | `Output/MuPDF.NET/17-ReplaceImage/replaced.pdf`, `before.png`, `after.png` |
| Expected | `replaced.txt`, `replaced.summary.txt`, `before.png.sha256`, `after.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\17-ReplaceImage
```

Recreate the fixture with `python Test/create_images_pdf.py`.

## APIs used

- `Page.GetImages`
- `Page.ReplaceImage`

## Related

| Example | Topic |
|---------|--------|
| [`09-InsertImage`](../09-InsertImage/) | Insert a new image onto a page |
