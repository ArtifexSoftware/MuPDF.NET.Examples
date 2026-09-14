# 20-RewriteImages

Downsample and recompress images in a PDF (`Document.RewriteImages`). Same API as PyMuPDF `Document.rewrite_images`.

## Sample method

`RewriteImages()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/test-rewrite-images.pdf` |
| Output | `Output/MuPDF.NET/20-RewriteImages/rewritten.pdf` |
| Expected | `Expected/rewritten.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\20-RewriteImages
```

## APIs used

- `Document.RewriteImages`

## Related

| Example | Topic |
|---------|--------|
| [`09-InsertImage`](../09-InsertImage/) | Place a new image on a page |
| [`17-ReplaceImage`](../17-ReplaceImage/) | Replace an existing image by xref |
