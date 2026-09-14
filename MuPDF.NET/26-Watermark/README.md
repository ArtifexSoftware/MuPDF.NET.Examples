# 26-Watermark

Add a centered image watermark on each page (`Page.InsertImage`).

## Sample method

`Watermark()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `sample.pdf`, `logo.png` |
| Output | `watermarked.pdf` |
| Expected | `watermarked.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\26-Watermark
```

## APIs used

- `Page.InsertImage` (`filename` / `xref`, `overlay`)
