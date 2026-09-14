# 16-DrawShapes

Draw dashed lines, a rectangle, and a filled circle.

## Sample method

`DrawShapes()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Output | `Output/MuPDF.NET/16-DrawShapes/shapes.pdf` |
| Expected | `Expected/shapes.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\16-DrawShapes
```

## APIs used

- `Page.DrawLine`
- `Page.DrawRect`
- `Page.DrawCircle`
