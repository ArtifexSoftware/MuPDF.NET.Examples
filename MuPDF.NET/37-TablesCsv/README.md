# 37-TablesCsv

Detect tables on a page and write the first table as CSV (`Page.GetTables`, `Table.Extract`).

Uses a dedicated ruled `tables-csv.pdf` (North Pier berth grid). Cells with spaces are quoted in the CSV.

## Sample method

`TablesToCsv()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/tables-csv.pdf` |
| Output | `Output/MuPDF.NET/37-TablesCsv/berth.csv`, `page.png` |
| Expected | `berth.csv` + summary + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\37-TablesCsv
```

Recreate the fixture: `python Test/create_tables_csv_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `Page.GetTables` (`lines_strict` strategy)
- `Table.Extract`

## Related

- [`11-Tables`](../11-Tables/) — same finder, Markdown instead of CSV
- [`32-ToMarkdown`](../32-ToMarkdown/) — whole PDF to Markdown (includes a berth table)
