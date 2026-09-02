# 12-Barcodes

Write a QR code into a PDF, then read it back.

## Sample method

`WriteAndReadBarcode()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | Payload string in code (`MuPDF.NET.Examples`) |
| Output | `Output/MuPDF.NET/12-Barcodes/qr.pdf` |
| Expected | `barcodes.txt`, `qr.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\12-Barcodes
```

## APIs used

- `Page.WriteBarcode` (`BarcodeFormat.QR`)
- `Page.ReadBarcodes`
