# 25-CompressSave

Save with compression options (`garbage`, `deflate`, `useObjstms`) per the Compressing Files guide.

## Sample method

`CompressSave()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/sample.pdf` |
| Output | `plain.pdf`, `compressed.pdf` |
| Expected | `compress.txt` + PDF fingerprints |

## Run

```powershell
dotnet run --project MuPDF.NET\25-CompressSave
```

## APIs used

- `Document.Save` (`garbage`, `deflate`, `deflateImages`, `deflateFonts`, `useObjstms`)

## Related

- [`20-RewriteImages`](../20-RewriteImages/) — downsample / recompress *embedded images*
