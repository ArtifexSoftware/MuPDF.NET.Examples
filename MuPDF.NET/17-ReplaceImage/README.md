# 17-ReplaceImage

**Replace** an existing page image while keeping the replacement’s width/height ratio (`InsertImage` with `keepProportion: true`).

`Page.ReplaceImage` swaps image bytes but keeps the old draw matrix (it can stretch). This sample clears the old image (`DeleteImage`) and inserts into the same rectangle with `keepProportion: true`.

To **add** a new image, see [`09-InsertImage`](../09-InsertImage/).

## Sample method

`ReplaceImage()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/Color.pdf`, `logo.png` |
| Output | `Output/MuPDF.NET/17-ReplaceImage/replaced.pdf` |
| Expected | `Expected/replaced.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\17-ReplaceImage
```

## APIs used

- `Page.GetImages`
- `Page.GetImageRects`
- `Page.DeleteImage`
- `Page.InsertImage` (`keepProportion: true`)

## Related

| Example | Topic |
|---------|--------|
| [`09-InsertImage`](../09-InsertImage/) | Insert a new image onto a page |
