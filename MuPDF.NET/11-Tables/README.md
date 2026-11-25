# 11-Tables

Detect tables on page 1 of `tables.pdf` and export them as Markdown.

## Sample method

`FindTables()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/tables.pdf` |
| Output | `Output/MuPDF.NET/11-Tables/tables.md` |
| Expected | `tables.md`, `tables.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\11-Tables
```

## APIs used

- `Page.GetTables` (`lines_strict` strategy)
- `Table.ToMarkdown`

## Related

- [`37-TablesCsv`](../37-TablesCsv/) — same finder, CSV instead of Markdown
