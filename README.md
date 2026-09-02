# MuPDF.NET.Examples

Customer-facing sample apps for **[MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)**, **[MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)**, and **[MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)**.

Each feature is a small console project. Packages are referenced via **NuGet only**.

Every `Program.cs` uses `Main` plus one named sample method (for example `OpenSave()`) with line comments so you can copy that method into your own project. `Main` only parses args and calls the sample.

Every example writes results under `Output/` and **compares them to golden files** in that project’s `Expected/` folder. Look for **`PASS —`** / **`FAIL —`** / **`SKIP —`** in the console. Use `run-all.cmd`, `.\run-all.ps1`, or `./run-all.sh` to batch-test after NuGet upgrades (they key off those lines; Office samples may AV during process teardown after a successful PASS).

## Clone and restore

```powershell
git clone https://github.com/ArtifexSoftware/MuPDF.NET.Examples.git
cd MuPDF.NET.Examples
dotnet restore
```

Package versions are pinned in [`Versions.props`](Versions.props). [`NuGet.Config`](NuGet.Config) uses a repo-local **`LocalNuget/`** folder (optional pre-release packages) and **nuget.org**. On Windows you can symlink `LocalNuget` → `D:\Artifex\LocalNuget` if you pack there.

## Layout

| Path | Role |
|------|------|
| `Common/` | Paths, `ResultCheck`, PDF fingerprints |
| `Input/` | Fixtures (committed) |
| `Output/` | Generated files (gitignored) |
| `{product}/{NN-Name}/Expected/` | Golden baselines (committed) |
| `run-all.cmd` / `run-all.ps1` / `run-all.sh` | Batch PASS/FAIL/SKIP runner |
| `LocalNuget/` | Optional local `.nupkg` drop folder |
| `LICENSE.md` | Artifex Community License |
| `SECURITY.md` | Vulnerability reporting |

## Prerequisites

- .NET 8 SDK (Windows, Linux, or macOS)
- Packages from [nuget.org](https://www.nuget.org/) (see `Versions.props`)
- Optional `MUPDF_OFFICE_KEY` for Office unlock
- `02-ToJsonLayout` / `03-ToText`: optional [pymupdf-layout](https://pypi.org/project/pymupdf-layout/) for layout mode

## Run (compare against Expected/)

```powershell
dotnet restore
.\run-all.cmd          # Windows
# or: .\run-all.ps1
# or one project:
dotnet run --project MuPDF.NET\01-OpenSave
```

```bash
# Linux / macOS (requires pwsh: https://aka.ms/powershell)
dotnet restore
chmod +x ./run-all.sh
./run-all.sh
# or: pwsh ./run-all.ps1
dotnet run --project MuPDF.NET/01-OpenSave
```

## Refresh baselines after a trusted package upgrade

```powershell
.\run-all.cmd --update-expected
# or: .\run-all.ps1 -UpdateExpected
# or:
dotnet run --project MuPDF.NET\01-OpenSave -- --update-expected
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
| ICC soft proof, separations, overprint | `19-ColorManagement` |
| Place a new image on a page | `09-InsertImage` |
| Swap an existing image xref | `17-ReplaceImage` |
| Extract embedded images to files | `21-ExtractImages` |
| Downsample / recompress images | `20-RewriteImages` |
| Compress PDF streams on save | `25-CompressSave` |
| Page file attachment annot | `30-FileAnnot` (vs `13-EmbeddedFiles`) |

| Product | Projects |
|---------|----------|
| MuPDF.NET | Open/Save, Pages, Render, Text, Recolor, Story/HTML, Annotations, Widgets, Insert/Replace/Extract images, Outline/Links, Tables, Barcodes, Embedded files, Metadata, TextWriter, Draw shapes, ZUGFeRD, Color management, Rewrite images, GetDrawings, Rotate/Crop, Encrypt, Compress, Watermark, Image→PDF, Image filters, Page ops, File annot |
| MuPDF.NET.PDF4LLM | Markdown, JSON layout, Plain text, OCR, Tables→CSV, Llama markdown reader, GetKeyValues, Markdown→PDF |
| MuPDF.NET.Office | Unlock/fonts, Open HWPX/DOCX, Export PDF, Export MD/JSON, With PDF4LLM |

## Versions

Edit `Versions.props` only.

## License

See [`LICENSE.md`](LICENSE.md) (Artifex Community License). The NuGet packages these samples use have their own license terms on nuget.org / artifex.com.
