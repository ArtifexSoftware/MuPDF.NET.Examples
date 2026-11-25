# 18-ZugferdEmbedded

Embed Factur-X / ZUGFeRD XML into an invoice PDF with PDF EmbeddedFiles.

`zugferd.pdf` is a one-page invoice fixture with no attachments. The sample adds `factur-x.xml` as `factur-x.xml`, then extracts it again to confirm a round-trip.

To attach a generic file (not ZUGFeRD), see [`13-EmbeddedFiles`](../13-EmbeddedFiles/).

## Sample method

`ZugferdEmbedded()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/zugferd.pdf`, `factur-x.xml` |
| Output | `Output/MuPDF.NET/18-ZugferdEmbedded/zugferd-with-xml.pdf`, `extracted-factur-x.xml` |
| Expected | `zugferd.txt`, `zugferd-with-xml.summary.txt`, `zugferd-with-xml.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\18-ZugferdEmbedded
```

Recreate the fixtures with `python Test/create_zugferd_pdf.py`.

## APIs used

- `Document.GetEmbeddedFileNames`
- `Document.DeleteEmbeddedFile`
- `Document.AddEmbeddedFile`
- `Document.GetEmbeddedFile`

## Related

| Example | Topic |
|---------|--------|
| [`13-EmbeddedFiles`](../13-EmbeddedFiles/) | Attach a generic file to a PDF |
