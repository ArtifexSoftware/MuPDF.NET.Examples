# 02-PagesMergeSplit

Merge pages from two PDFs, then extract the first page into a new document.

`PagesMergeSplit()` opens `pages-a.pdf` and `pages-b.pdf`, inserts page 0 of B into A at position 1, saves `merged.pdf`, then copies the first page of that file to `first-page.pdf`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/pages-a.pdf`, `pages-b.pdf` |
| Output | `merged.pdf`, `first-page.pdf` under `Output/MuPDF.NET/02-PagesMergeSplit/` |
| Expected | `merged.summary.txt`, `first-page.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\02-PagesMergeSplit
```

## APIs used

- `Document.Open`
- `Document.InsertPdf`
- `Document.Save`

## Related

- [`29-PageOps`](../29-PageOps/) — `MovePage`, `DeletePage`, `Select`
