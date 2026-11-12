# 38-GetKeyValues

Extract AcroForm field names and values via `Utils.GetKeyValues`.

## Sample method

`GetKeyValues()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/Widget.pdf` |
| Output | `Output/MuPDF.NET/38-GetKeyValues/keyvalues.txt` |
| Expected | `Expected/keyvalues.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\38-GetKeyValues
```

## APIs used

- `Utils.GetKeyValues`
