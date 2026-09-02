# 05-WithPdf4Llm

Unlock Office, then run `MuPDF4LLM.ToMarkdown` on an HWPX path.

## Sample method

`WithPdf4Llm()` in `Program.cs`.

## Packages

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)
- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)

## Prerequisites

- `MuPDFOffice.Unlock` registers the Office document handler so PDF4LLM can open HWPX/DOCX.
- Optional `MUPDF_OFFICE_KEY`.

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.Office/sample.hwpx` |
| Output | `Output/MuPDF.NET.Office/05-WithPdf4Llm/sample.hwpx.md` |
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
dotnet run --project MuPDF.NET.Office/05-WithPdf4Llm
```

## APIs used

- `MuPDFOffice.Unlock`
- `MuPDF4LLM.SetUseLayout(false)`
- `MuPDF4LLM.ToMarkdown`
