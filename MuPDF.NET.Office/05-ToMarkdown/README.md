# 05-ToMarkdown

Unlock **MuPDF.NET.Office**, then run `Document.ToMarkdown` on an HWPX path (same extract API as PDF).

## Sample method

`ToMarkdown()` in `Program.cs`.

## Packages

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)
- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) (via Office + `Common`)

## Prerequisites

- `MuPDFOffice.Unlock` registers the Office document handler so `Document` can open HWPX/DOCX.
- Optional `MUPDF_OFFICE_KEY`.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.Office/sample.hwpx` |
| Output | `Output/MuPDF.NET.Office/05-ToMarkdown/sample.hwpx.md` |
| Expected | `Expected/sample.shape.txt` (portable) |

`sample.hwpx.md` under Expected/ is the correct Hangul reference
(`안녕하세요 이건 테스트 파일입니다.`). On Linux, SmartOffice typically
extracts that Unicode text. On Windows it often falls back to
Arial/Liberation **without Hangul glyphs**, so `ToMarkdown` yields U+FFFD
placeholders (and you may see `cannot create ToUnicode mapping for …Arial`).

The golden check therefore compares a **text shape** (`H` = Hangul or
replacement/NUL), which matches on both OS. Full markdown is still written
to `Output/` for inspection.

## Run

```bash
dotnet run --project MuPDF.NET.Office/05-ToMarkdown
```

## APIs used

- `MuPDFOffice.Unlock`
- `Utils.UseLayout(false)`
- `Document.ToMarkdown`
