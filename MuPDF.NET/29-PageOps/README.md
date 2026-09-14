# 29-PageOps

Delete, move, and select pages (`DeletePage`, `MovePage`, `Select`).

## Sample method

`PageOps()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `sample.pdf`, `Blank.pdf` |
| Output | `built.pdf`, `selected.pdf` |
| Expected | `page-ops.txt` + fingerprints |

## Run

```powershell
dotnet run --project MuPDF.NET\29-PageOps
```

## APIs used

- `Document.InsertPdf`
- `Document.MovePage`
- `Document.DeletePage`
- `Document.Select`

## Related

- [`02-PagesMergeSplit`](../02-PagesMergeSplit/) — merge / extract via `InsertPdf` only
