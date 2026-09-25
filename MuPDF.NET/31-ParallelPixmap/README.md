# 31-ParallelPixmap

Render the first page of several independent PDFs in parallel (`Parallel.ForEach` + `GetPixmap`).

Each worker **opens its own `Document`**. Do not share one `Document` across threads: MuPDF serializes `LoadPage` / display-list build on the same document. Cap `MaxDegreeOfParallelism` at `Environment.ProcessorCount` and dispose pixmap, page, and document (`using`). Requires MuPDF.NET **3.28.2.4** or later.

For a multi-page file, use the same pattern: one `Document.Open` per worker, then `doc[pageIndex]`.

You may see `warning: found duplicate fz_icc_link in the store`. That is harmless (shared native color cache).

## Sample method

`ParallelPixmap()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `sample.pdf`, `Blank.pdf`, `Color.pdf` |
| Output | `sample.png`, `Blank.png`, `Color.png` |
| Expected | `sample.png.sha256`, `Blank.png.sha256`, `Color.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\31-ParallelPixmap
```

## APIs used

- `Document.Open` (one per worker)
- `Page.GetPixmap` with `Matrix(2, 2)`
- `Parallel.ForEach` + `ParallelOptions.MaxDegreeOfParallelism`
- `Pixmap.Save`

## Related

- [`03-RenderPixmap`](../03-RenderPixmap/) — sequential single-page render
