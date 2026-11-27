# MuPDF.NET.Examples

Customer-facing sample apps for **[MuPDF.NET](https://mupdf.com/mupdf-dotnet) 4.1.2** and **[MuPDF.NET.Office](https://mupdf.com/mupdf-dotnet/office) 2.1.0**.

Documentation for these samples: [MuPDF.NET examples](https://mupdfnet.readthedocs.io/en/latest/examples/index.html).

Markdown / JSON / text extract (`Document.ToMarkdown`, `ToJson`, `ToText`, `ToChunks`) and RAG helpers (`Utils.LoadAiAsync`, `Utils.LlamaMarkdownReader`, `Utils.MarkdownToPdf`) are in **MuPDF.NET 4.1.2**. Layout is switched with `MuPDF4LLM.SetLayout` (`using MuPDF.NET;`). Samples `PackageReference` **MuPDF.NET** and **MuPDF.NET.Office** only.

Each feature is a small console project. Packages are referenced via **NuGet only**.

Every `Program.cs` uses `Main` plus one named sample method (for example `Basic()`) with line comments so you can copy that method into your own project. `Main` only parses args and calls the sample.

Every example writes results under `Output/` and **compares them to golden files** in that project’s `Expected/` folder. Look for **`PASS —`** / **`FAIL —`** / **`SKIP —`** in the console. Use `run-all.cmd`, `.\run-all.ps1`, or `./run-all.sh` to batch-test after NuGet upgrades (they key off those lines; Office samples may AV during process teardown after a successful PASS).

## Clone and restore

```powershell
git clone https://github.com/ArtifexSoftware/MuPDF.NET.Examples.git
cd MuPDF.NET.Examples
dotnet restore
```

Package versions are pinned in [`Versions.props`](Versions.props). [`NuGet.Config`](NuGet.Config) restores from the repo-local **`LocalNuget/`** folder, then **nuget.org**.

## Layout

| Path | Role |
|------|------|
| Documentation | [MuPDF.NET examples](https://mupdfnet.readthedocs.io/en/latest/examples/index.html) |
| `Common/` | Paths, `ResultCheck`, PDF fingerprints |
| `Input/` | Fixtures (committed) |
| `Output/` | Generated files (gitignored) |
| `artifacts/` | Shared `bin/` / `obj/` for all samples (gitignored) |
| `{product}/{NN-Name}/Expected/` | Golden baselines (committed) |
| `run-all.cmd` / `run-all.ps1` / `run-all.sh` | Batch PASS/FAIL/SKIP runner |
| `LocalNuget/` | Optional local `.nupkg` drop folder |
| `LICENSE.md` | Artifex Community License |
| `SECURITY.md` | Vulnerability reporting |

## Prerequisites

- .NET 8 SDK (Windows, Linux, or macOS)
- Packages from [nuget.org](https://www.nuget.org/) (see `Versions.props`)
- Optional `MUPDF_OFFICE_KEY` for Office unlock
- `33-ToJsonLayout` / `34-ToText` / `35-ToChunks`: optional layout worker needs **Python 3.11+** (Ubuntu 22.04: `sudo apt install python3.11 python3.11-venv python3.11-full`) then `dotnet msbuild -t:MuPDFNetSetupLayoutPython` — [MuPDF.NET README — Layout analysis](https://github.com/ArtifexSoftware/MuPDF.NET#layout-analysis-optional)
- `41-LoadAi`: optional `AZURE_OPENAI_*` (and `AZURE_SEARCH_*`) for a live model; default run needs no keys

## Run (compare against Expected/)

```powershell
dotnet restore
.\run-all.cmd          # Windows
# or: .\run-all.ps1
# or one project:
dotnet run --project MuPDF.NET\01-Basic
```

```bash
# Linux / macOS (requires pwsh: https://aka.ms/powershell)
dotnet restore
chmod +x ./run-all.sh
./run-all.sh
# or: pwsh ./run-all.ps1
dotnet run --project MuPDF.NET/01-Basic
```

## Refresh baselines after a trusted package upgrade

```powershell
.\run-all.cmd --update-expected
# or: .\run-all.ps1 -UpdateExpected
# or:
dotnet run --project MuPDF.NET\01-Basic -- --update-expected
```

## What is compared

| Kind | Method |
|------|--------|
| Markdown / JSON / text | Exact text (LF-normalized) |
| PNG | SHA-256 |
| PDF | `pageCount` + SHA-256 of extracted text (not raw PDF bytes) |
| Unlock / page counts | Small `*.summary.txt` property files |

## Example projects

Each project folder has its own `README.md` (what it shows, inputs, how to run, APIs).

**Naming tip — color vs images**

| Want to… | Use |
|----------|-----|
| Convert page to DeviceCMYK/RGB/Gray | `05-Recolor` |
| ICC soft proof, separations, overprint | `19-ColorManagement` (`color-managed.pdf`) |
| Place a new image on a page | `09-InsertImage` |
| Stamp an image on every page | `26-Watermark` (`watermark.pdf`) |
| Wrap an image as a one-page PDF | `27-ImageToPdf` (`image-to-pdf.png`) |
| Denoise a pixmap (median filter) | `28-ImageFilters` (`image-filters.png`) |
| Reorder / delete / select pages | `29-PageOps` (`page-ops.pdf`) |
| Swap an existing image xref | `17-ReplaceImage` |
| Embed Factur-X / ZUGFeRD XML | `18-ZugferdEmbedded` (vs generic `13-EmbeddedFiles`) |
| Extract embedded images to files | `21-ExtractImages` (`extract-images.pdf`) |
| Extract vector drawings | `22-GetDrawings` |
| Rotate a page / set CropBox | `23-RotateCrop` (`rotate-crop.pdf`) |
| Encrypt / decrypt AES-256 | `24-EncryptDecrypt` (`encrypt-decrypt.pdf`) |
| Downsample / recompress images | `20-RewriteImages` (`rewrite-images.pdf`) |
| Compress PDF streams on save | `25-CompressSave` (`compress-save.pdf`) |
| Page file attachment annot | `30-FileAnnot` (`file-annot.pdf`, vs `13-EmbeddedFiles`) |
| Render many files/pages in parallel | `31-ParallelPixmap` (`parallel-a.pdf` / `b` / `c`, vs sequential `03-RenderPixmap`) |
| Convert a PDF to Markdown | `32-ToMarkdown` (`bulletin.pdf`) |
| Convert a PDF to layout JSON | `33-ToJsonLayout` (`bulletin.pdf`, layout worker) |
| Convert a PDF to plain text | `34-ToText` (`bulletin.pdf`, layout worker) |
| Split a PDF into retrieval chunks | `35-ToChunks` (`bulletin.pdf`, layout worker) |
| OCR a scanned page to Markdown | `36-Ocr` (`ocr-scan.pdf`, layout worker) |
| Detect a table and write CSV | `37-TablesCsv` (`tables-csv.pdf`) |
| Load LlamaIndex documents (one per page) | `38-LlamaMarkdownReader` (`llama-pages.pdf`) |

| Product | Projects |
|---------|----------|
| MuPDF.NET | Open/Save, Pages, Render, Parallel pixmap, Text, Recolor, Story/HTML, Annotations, Widgets, Insert/Replace/Extract images, Outline/Links, Tables, Barcodes, Embedded files, Metadata, TextWriter, Draw shapes, ZUGFeRD, Color management, Rewrite images, GetDrawings, Rotate/Crop, Encrypt, Compress, Watermark, Image→PDF, Image filters, Page ops, File annot, Markdown, JSON layout, Plain text, OCR, Tables→CSV, Llama markdown reader, GetKeyValues, Markdown→PDF, LoadAi (RAG ask/search), Retrieval chunks |
| MuPDF.NET.Office | Unlock, Open HWPX/DOCX, Export PDF, Export MD/JSON, Unlock then `Document.ToMarkdown` |

## Versions

Edit `Versions.props` only. Current pins: MuPDF.NET **4.1.2**, Office **2.1.0**. Restore looks in `LocalNuget/`, then nuget.org.

## License

See [`LICENSE.md`](LICENSE.md) (Artifex Community License). The NuGet packages these samples use have their own license terms on nuget.org / artifex.com.
