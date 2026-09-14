# 11-Tables

Detect tables on a page and export them as Markdown.

## Sample method

`FindTables()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/err_table.pdf` |
| Output | `Output/MuPDF.NET/11-Tables/tables.md` |
| Expected | `Expected/tables.md` |

## Run

```powershell
dotnet run --project MuPDF.NET\11-Tables
```

## APIs used

- `Utils.GetTables` (`lines_strict` strategy)
- `Table.ToMarkdown`
