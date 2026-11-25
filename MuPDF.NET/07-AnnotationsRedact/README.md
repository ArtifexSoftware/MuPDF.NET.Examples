# 07-AnnotationsRedact

Add a text note, a rectangle annotation, and apply a redaction.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/Blank.pdf` |
| Output | `Output/MuPDF.NET/07-AnnotationsRedact/annotated.pdf`, `annotated.png` |
| Expected | `annot-info.txt`, `annotated.summary.txt`, `annotated.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\07-AnnotationsRedact
```

## APIs used

- `Page.AddTextAnnot` / `AddRectAnnot` / `AddRedactAnnot`
- `Annot.SetInfo` / `SetColors` / `Update`
- `Page.ApplyRedactions`
- `Page.Annots`
