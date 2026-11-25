# 12-Barcodes

Write several barcode types (including QR) into a PDF, then read them back.

## Sample method

`WriteAndReadBarcode()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | Payloads in code (QR, Aztec, Code 128, Code 39, EAN-8, PDF417) |
| Output | `Output/MuPDF.NET/12-Barcodes/barcodes.pdf` |
| Expected | `barcodes.txt`, `barcodes.summary.txt`, `barcodes.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\12-Barcodes
```

## APIs used

- `Page.WriteBarcode`
- `Page.ReadBarcodes`
- `BarcodeFormat` (`QR`, `AZTEC`, `CODE128`, `CODE39`, `EAN8`, `PDF417`)
