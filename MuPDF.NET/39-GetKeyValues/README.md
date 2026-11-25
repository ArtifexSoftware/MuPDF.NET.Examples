# 39-GetKeyValues

Extract AcroForm field names and values via `Utils.GetKeyValues`.

Uses the same one-page `widgets.pdf` as [08-FormWidgets](../08-FormWidgets/) (text, checkbox, radio, combo, list). `GetKeyValues` returns one dictionary entry per field name. The listing is a table with columns `name`, `field_value`, `pages`, and `terminal`. Radio buttons collapse to a single `plan` value. The listing is UTF-8 without a BOM.

## Sample method

`GetKeyValues()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/widgets.pdf` |
| Output | `Output/MuPDF.NET/39-GetKeyValues/keyvalues.txt`, `page.png` |
| Expected | `keyvalues.txt` + dump + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\39-GetKeyValues
```

Recreate the fixture: `python Test/create_widgets_pdf.py` from `D:\Artifex\Work`.

## APIs used

- `Utils.GetKeyValues` (`includeXrefs` is `false`)

## Related

- [`08-FormWidgets`](../08-FormWidgets/) — same PDF, `Page.Widgets` lists each widget instance (name, type, value)
