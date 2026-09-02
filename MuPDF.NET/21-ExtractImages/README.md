# 21-ExtractImages

List images on a page (`Page.GetImages`) and write each stream to disk (`Document.ExtractImage`).

## Sample method

`ExtractImages()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/Color.pdf` |
| Output | `Output/MuPDF.NET/21-ExtractImages/img-*.png` (etc.) |
| Expected | `extract-images.txt` + per-file `.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\21-ExtractImages
```

## APIs used

- `Page.GetImages`
- `Document.ExtractImage`
