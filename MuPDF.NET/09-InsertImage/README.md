# 09-InsertImage

**Insert** a new image onto a page (`Page.InsertImage`).

To **swap** an existing image xref, see [`17-ReplaceImage`](../17-ReplaceImage/).

## Sample method

`InsertImage()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/logo.png` |
| Output | `Output/MuPDF.NET/09-InsertImage/with-logo.pdf` |
| Expected | `Expected/with-logo.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\09-InsertImage
```

## APIs used

- `Document.NewPage`
- `Page.InsertImage`

## Related

| Example | Topic |
|---------|--------|
| [`17-ReplaceImage`](../17-ReplaceImage/) | Replace an existing image by xref |
