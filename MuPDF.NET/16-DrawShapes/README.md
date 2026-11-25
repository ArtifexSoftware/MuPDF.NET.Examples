# 16-DrawShapes

Draw several vector shapes on a new page: line, rectangle, circle, oval, polyline, Bézier, zigzag, and sector.

## Sample method

`DrawShapes()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Output | `Output/MuPDF.NET/16-DrawShapes/shapes.pdf` |
| Expected | `Expected/shapes.txt`, `Expected/shapes.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\16-DrawShapes
```

## APIs used

- `Page.DrawLine` / `DrawRect` / `DrawCircle` / `DrawOval`
- `Page.DrawPolyline` / `DrawBezier` / `DrawZigzag` / `DrawSector`
