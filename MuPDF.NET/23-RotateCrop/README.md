# 23-RotateCrop

Rotate a page (`Page.SetRotation`) and set a crop box (`Page.SetCropBox`).

## Sample method

`RotateCrop()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/rotate-crop.pdf` |
| Output | `Output/MuPDF.NET/23-RotateCrop/rotated.pdf`, `cropped.pdf`, `original.png`, `rotated.png`, `cropped.png` |
| Expected | `rotate-crop.txt` + PDF fingerprints + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\23-RotateCrop
```

Recreate the fixture: `python Test/create_rotate_crop_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `Page.SetRotation`
- `Page.SetCropBox`
- `Page.Rotation` / `Rect` / `CropBox`
