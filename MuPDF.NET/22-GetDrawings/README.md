# 22-GetDrawings

Draw vector shapes, extract path dictionaries with `Page.GetDrawings`, then redraw path bounds.

## Sample method

`GetDrawings()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Output | `Output/MuPDF.NET/22-GetDrawings/drawn.pdf`, `redrawn.pdf` |
| Expected | `drawings.txt` + PDF fingerprints |

## Run

```powershell
dotnet run --project MuPDF.NET\22-GetDrawings
```

## APIs used

- `Page.DrawLine` / `DrawRect` / `DrawCircle`
- `Page.GetDrawings`
