# 05-Recolor

Convert page content to another **device colorspace** with `Document.Recolor` (here: 4 components → CMYK).

After recolor, `GetPageImages().CsName` is typically `ICCBased` (not `DeviceCMYK`). Use `ExtractImage` for component count (`ColorSpace == 4`) and a richer `CsName` that includes the ICC profile. Do **not** use `AltCsName` for this check — it is usually empty, and `??` does not fall through `""`.

This is **not** ICC soft-proofing / print separations — for those see [`19-ColorManagement`](../19-ColorManagement/).

## Sample method

`Recolor()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/Color.pdf` |
| Output | `Output/MuPDF.NET/05-Recolor/recolor.pdf` |
| Expected | `Expected/recolor.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\05-Recolor
```

## APIs used

- `Document.GetPageImages`
- `Document.Recolor(page, components)` — `1`=Gray, `3`=RGB, `4`=CMYK

## Related

| Example | Topic |
|---------|--------|
| [`19-ColorManagement`](../19-ColorManagement/) | ICC profiles, soft proof, separations, overprint |
