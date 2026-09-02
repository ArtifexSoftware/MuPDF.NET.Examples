# 01-ToMarkdown

Convert a PDF to Markdown (classic RAG path with layout off).

## Sample method

`ToMarkdown()` in `Program.cs`.

## Package

- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/columns.pdf` |
| Output | `Output/MuPDF.NET.PDF4LLM/01-ToMarkdown/columns.md` |
| Expected | `Expected/columns.md` |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\01-ToMarkdown
```

## APIs used

- `MuPDF4LLM.SetUseLayout(false)`
- `MuPDF4LLM.ToMarkdown`
