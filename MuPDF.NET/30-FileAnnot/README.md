# 30-FileAnnot

Attach a file as a **page annotation** (`Page.AddFileAnnot`). For document-level EmbeddedFiles, see [`13-EmbeddedFiles`](../13-EmbeddedFiles/).

## Sample method

`FileAnnot()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/file-annot.pdf`, `spare-list.txt` |
| Output | `Output/MuPDF.NET/30-FileAnnot/with-file-annot.pdf`, `before.png`, `after.png` |
| Expected | `file-annot.txt` + PDF fingerprint + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\30-FileAnnot
```

Recreate the fixtures: `python Test/create_file_annot.py` from `D:\Artifex\Work`.

## APIs used

- `Page.AddFileAnnot`
- `Annot.GetFileInfo` / `GetFile`
- `Page.Annots`

## Related

- [`13-EmbeddedFiles`](../13-EmbeddedFiles/) — document-level EmbeddedFiles name tree
- [`07-AnnotationsRedact`](../07-AnnotationsRedact/) — text, rect, and redact annotations
