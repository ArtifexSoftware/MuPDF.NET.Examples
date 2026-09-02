# 03-RenderPixmap

Render page 1 to a PNG pixmap at 2× zoom.

## Sample method

`RenderPixmap()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/sample.pdf` |
| Output | `Output/MuPDF.NET/03-RenderPixmap/page-1.png` |
| Expected | `Expected/page-1.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\03-RenderPixmap
```

## APIs used

- `Page.GetPixmap` with `Matrix(2, 2)`
- `Pixmap.Save`
