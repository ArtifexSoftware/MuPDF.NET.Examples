# 05-TablesCsv

Detect tables on a page and write the first table as CSV.

## Sample method

`TablesToCsv()` in `Program.cs`.

## Package

- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM) (uses MuPDF.NET table APIs)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/national-capitals.pdf` |
| Output | `Output/MuPDF.NET.PDF4LLM/05-TablesCsv/capitals.csv` |
| Expected | `Expected/capitals.csv` |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\05-TablesCsv
```

## APIs used

- `Utils.GetTables`
- `Table.Extract`
