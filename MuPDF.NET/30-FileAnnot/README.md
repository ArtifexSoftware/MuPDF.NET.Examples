# 30-FileAnnot

Attach a file as a **page annotation** (`Page.AddFileAnnot`). For document-level EmbeddedFiles, see [`13-EmbeddedFiles`](../13-EmbeddedFiles/).

## Sample method

`FileAnnot()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Blank.pdf`, `note.txt` |
| Output | `with-file-annot.pdf` |
| Expected | `file-annot.txt` + fingerprint |

## Run

```powershell
dotnet run --project MuPDF.NET\30-FileAnnot
```

## APIs used

- `Page.AddFileAnnot`
- `Page.GetAnnots`
