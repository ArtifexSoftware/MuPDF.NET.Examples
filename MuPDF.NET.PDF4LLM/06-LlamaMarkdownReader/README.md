# 06-LlamaMarkdownReader

Load LlamaIndex-compatible documents (one per page) via `PDFMarkdownReader`.

## Sample method

`LlamaMarkdownReader()` in `Program.cs`.

## Package

- [MuPDF.NET.PDF4LLM](https://www.nuget.org/packages/MuPDF.NET.PDF4LLM)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET.PDF4LLM/columns.pdf` |
| Output | `Output/MuPDF.NET.PDF4LLM/06-LlamaMarkdownReader/llama-docs.txt` |
| Expected | `Expected/llama-docs.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET.PDF4LLM\06-LlamaMarkdownReader
```

## APIs used

- `MuPDF4LLM.SetUseLayout(false)` — classic RAG markdown (stable Expected/)
- `PDFMarkdownReader.LoadData`
- `LlamaIndexDocument.Text` / `ExtraInfo`
