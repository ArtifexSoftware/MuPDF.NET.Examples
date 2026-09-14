# 13-EmbeddedFiles

Attach a text file to a PDF and list embedded attachments.

## Sample method

`EmbeddedFiles()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/Blank.pdf`, `note.txt` |
| Output | `Output/MuPDF.NET/13-EmbeddedFiles/with-attachment.pdf` |
| Expected | `embedded.txt`, `with-attachment.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\13-EmbeddedFiles
```

## APIs used

- `Document.AddEmbeddedFile` / `DeleteEmbeddedFile`
- `Document.GetEmbeddedFileNames` / `GetEmbeddedFile` / `GetEmbeddedFileInfo`
