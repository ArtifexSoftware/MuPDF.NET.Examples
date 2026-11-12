# 36-TablesCsv

Detect tables on a page and write the first table as CSV.

## Sample method

`TablesToCsv()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) 4.1

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/national-capitals.pdf` |
| Output | `Output/MuPDF.NET/36-TablesCsv/capitals.csv` |
| Expected | `Expected/capitals.csv` |

## Run

```powershell
dotnet run --project MuPDF.NET\36-TablesCsv
```

## APIs used

- `Utils.GetTables`
- `Table.Extract`
