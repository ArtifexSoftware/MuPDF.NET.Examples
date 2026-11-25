# 29-PageOps

Delete, move, and select pages (`DeletePage`, `MovePage`, `Select`).

To **merge / extract via InsertPdf only**, see [`02-PagesMergeSplit`](../02-PagesMergeSplit/).

## Sample method

`PageOps()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/page-ops.pdf`, `page-ops-insert.pdf` |
| Output | `Output/MuPDF.NET/29-PageOps/built.pdf`, `selected.pdf`, page PNGs |
| Expected | `page-ops.txt` + PDF fingerprints + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\29-PageOps
```

Recreate the fixtures: `python Test/create_page_ops_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `Document.InsertPdf`
- `Document.MovePage`
- `Document.DeletePage`
- `Document.Select`

## Related

- [`02-PagesMergeSplit`](../02-PagesMergeSplit/) — merge / extract via `InsertPdf` only
