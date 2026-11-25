# 15-TextWriter

Write text onto a new page with `TextWriter` and a built-in font.

## Sample method

`WriteTextPage()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Output | `Output/MuPDF.NET/15-TextWriter/textwriter.pdf` |
| Expected | `Expected/textwriter.txt`, `Expected/textwriter.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\15-TextWriter
```

## APIs used

- `TextWriter.FillTextbox` / `Append` / `WriteText`
- `Font`
