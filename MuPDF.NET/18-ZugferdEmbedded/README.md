# 18-ZugferdEmbedded

Extract and re-embed ZUGFeRD / Factur-X XML using PDF EmbeddedFiles.

## Sample method

`ZugferdEmbedded()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `zugferd-muster-rechnung.pdf`, `zugferd-muster-rechnung.xml` |
| Output | `extracted-factur-x.xml`, `zugferd-with-xml.pdf` |
| Expected | `zugferd.txt`, `zugferd-with-xml.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\18-ZugferdEmbedded
```

## APIs used

- `Document.GetEmbeddedFile` / `AddEmbeddedFile` / `DeleteEmbeddedFile`
