# 25-CompressSave

Save with compression options (`garbage`, `deflate`, `useObjstms`) per the Compressing Files guide.

## Sample method

`CompressSave()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/compress-save.pdf` |
| Output | `Output/MuPDF.NET/25-CompressSave/plain.pdf`, `compressed.pdf`, `plain.png`, `compressed.png` |
| Expected | `compress.txt` + PDF fingerprints + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\25-CompressSave
```

Recreate the fixture: `python Test/create_compress_save_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `Document.Save` (`garbage`, `deflate`, `deflateImages`, `deflateFonts`, `useObjstms`)

## Related

- [`20-RewriteImages`](../20-RewriteImages/) — downsample / recompress *embedded images*
