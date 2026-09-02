# 23-RotateCrop

Rotate a page (`Page.SetRotation`) and set a crop box (`Page.SetCropBox`).

## Sample method

`RotateCrop()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/sample.pdf` |
| Output | `rotated.pdf`, `cropped.pdf` |
| Expected | `rotate-crop.txt` + PDF fingerprints |

## Run

```powershell
dotnet run --project MuPDF.NET\23-RotateCrop
```

## APIs used

- `Page.SetRotation`
- `Page.SetCropBox`
- `Page.Rotation` / `CropBox`
