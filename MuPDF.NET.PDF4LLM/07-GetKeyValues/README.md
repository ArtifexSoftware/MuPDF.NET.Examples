# 07-GetKeyValues

Extract AcroForm field names and values via `MuPDF4LLM.GetKeyValues`.

## Sample method

`GetKeyValues()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.0 (via `Common`)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/Widget.pdf` |
| Output | `Output/MuPDF.NET.PDF4LLM/07-GetKeyValues/keyvalues.txt` |
| Expected | `Expected/keyvalues.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\07-GetKeyValues
```

## APIs used

- `MuPDF4LLM.GetKeyValues`
