# 01-OpenSave

Open a PDF and write a full copy to disk.

## Sample method

`OpenSave()` in `Program.cs` — copy this method into your project and adjust paths.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/sample.pdf` |
| Output | `Output/MuPDF.NET/01-OpenSave/sample-copy.pdf` |
| Expected | `Expected/sample-copy.summary.txt` (page count + text fingerprint) |

## Run

```powershell
dotnet run --project MuPDF.NET\01-OpenSave
# refresh baseline:
dotnet run --project MuPDF.NET\01-OpenSave -- --update-expected
```

## APIs used

- `Document.Open`
- `Document.Save`
