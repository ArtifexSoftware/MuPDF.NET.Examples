# 14-Metadata

Read `basic.pdf` metadata, update it with `SetMetadata`, and print before / after.

## Sample method

`Metadata()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/basic.pdf` (user password `user`) |
| Output | `Output/MuPDF.NET/14-Metadata/with-metadata.pdf` |
| Expected | `metadata.txt`, `with-metadata.summary.txt`, `with-metadata.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\14-Metadata
```

## APIs used

- `Document.Authenticate`
- `Document.Metadata`
- `Document.SetMetadata`
