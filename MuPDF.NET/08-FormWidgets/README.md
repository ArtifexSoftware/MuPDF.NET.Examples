# 08-FormWidgets

List AcroForm widgets on page 1 (field name, type, value).

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/widgets.pdf` |
| Output | `Output/MuPDF.NET/08-FormWidgets/widgets.png` |
| Expected | `widgets.txt`, `widgets.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\08-FormWidgets
```

## APIs used

- `Page.Widgets`
- `Widget.FieldName` / `FieldTypeString` / `FieldValue`

## Related

- [`39-GetKeyValues`](../39-GetKeyValues/) — same PDF, `Utils.GetKeyValues` returns field name → value bags
