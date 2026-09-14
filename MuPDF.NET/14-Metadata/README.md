# 14-Metadata

Read and update PDF document metadata (`title`, `author`, …).

## Sample method

`Metadata()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/sample.pdf` |
| Output | `with-metadata.pdf`, `metadata.txt` |
| Expected | `metadata.txt`, `with-metadata.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\14-Metadata
```

## APIs used

- `Document.MetaData`
- `Document.SetMetadata`
