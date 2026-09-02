# 07-AnnotationsRedact

Add a text note, a rectangle annotation, and apply a redaction.

## Sample method

`AnnotationsRedact()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/Blank.pdf` |
| Output | `Output/MuPDF.NET/07-AnnotationsRedact/annotated.pdf` |
| Expected | `Expected/annotated.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\07-AnnotationsRedact
```

## APIs used

- `Page.AddTextAnnot` / `AddRectAnnot` / `AddRedactAnnot`
- `Annot.Update`
- `Page.ApplyRedactions`
