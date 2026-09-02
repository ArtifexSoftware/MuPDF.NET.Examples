# 27-ImageToPdf

Convert a PNG to a single-page PDF sized to the image.

## Sample method

`ImageToPdf()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/apple.png` |
| Output | `apple.pdf` |
| Expected | `apple.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\27-ImageToPdf
```

## APIs used

- `Pixmap` (from image file)
- `Document.NewPage`
- `Page.InsertImage`
