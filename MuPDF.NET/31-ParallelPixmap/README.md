# 31-ParallelPixmap

Render the first page of several independent PDFs in parallel (`Parallel.ForEach` + `GetPixmap`).

Each worker **opens its own `Document`**. Do not share one `Document` across threads: MuPDF serializes `LoadPage` / display-list build on the same document. Cap `MaxDegreeOfParallelism` at `Environment.ProcessorCount` and dispose pixmap, page, and document (`using`).

For a multi-page file, use the same pattern: one `Document.Open` per worker, then `doc[pageIndex]`.

You may see `warning: found duplicate fz_icc_link in the store`. That is harmless (shared native color cache).

To **render one page sequentially**, see [`03-RenderPixmap`](../03-RenderPixmap/).

## Sample method

`ParallelPixmap()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/parallel-a.pdf`, `parallel-b.pdf`, `parallel-c.pdf` |
| Output | `Output/MuPDF.NET/31-ParallelPixmap/parallel-a.png`, `parallel-b.png`, `parallel-c.png` |
| Expected | `parallel.txt` + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\31-ParallelPixmap
```

Recreate the fixtures: `python Test/create_parallel_pixmap_pdfs.py` from `D:\Artifex\Work`.

## APIs used

- `Document.Open` (one per worker)
- `Page.GetPixmap` with `Matrix(2, 2)`
- `Parallel.ForEach` + `ParallelOptions.MaxDegreeOfParallelism`
- `Pixmap.Save`

## Related

- [`03-RenderPixmap`](../03-RenderPixmap/) — sequential single-page render
