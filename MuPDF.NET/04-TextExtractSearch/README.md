# 04-TextExtractSearch

Extract plain text from page 1 and search for a string (`Hydraulik`).

## Sample method

`TextExtractSearch()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/sample.pdf` |
| Expected | `page-1.txt`, `search.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\04-TextExtractSearch
```

## APIs used

- `Page.GetText("text")`
- `Page.SearchFor`
