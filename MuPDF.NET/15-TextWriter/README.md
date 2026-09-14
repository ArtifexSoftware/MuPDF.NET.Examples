# 15-TextWriter

Write text onto a new page with `TextWriter` and a built-in font.

## Sample method

`TextWriterHello()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Output | `Output/MuPDF.NET/15-TextWriter/hello.pdf` |
| Expected | `Expected/hello.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\15-TextWriter
```

## APIs used

- `TextWriter.FillTextbox` / `WriteText`
- `Font`
