# 08-FormWidgets

List AcroForm widgets on page 1 (field name, type, value).

## Sample method

`FormWidgets()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/Widget.pdf` |
| Expected | `Expected/widgets.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\08-FormWidgets
```

## APIs used

- `Page.Widgets`
- `Widget.FieldName` / `FieldTypeString` / `FieldValue`
