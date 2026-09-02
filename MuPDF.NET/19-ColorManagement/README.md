# 19-ColorManagement

**ICC / print** color samples (soft proof, separations, overprint, device RGB/Gray/CMYK renders).

For simple device-colorspace conversion of page content, see [`05-Recolor`](../05-Recolor/) (`Document.Recolor`).

- High-DPI viewer render  
- DeviceRGB / DeviceGray / DeviceCMYK  
- PDF object / OutputIntents inspection  
- Zoom + links / TOC  
- ICC display & proof profiles  
- Color-managed rendering  
- Output Intent  
- DeviceN / spot separations  
- Overprint simulation parameters  
- Rendering intent + black-point / overprint flags  
- Prepress analysis foundation  
- ICC soft proof  

## Sample methods

`ColorManagement()` in `Program.cs` orchestrates the run. Copy any `Demo*` method (and the helpers at the bottom) into your project.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET) (managed API + `mupdf` low-level types)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/color/test.pdf`, `NULL.icc`, `Proof.icc` |
| Output | PNGs / PAM under `Output/MuPDF.NET/19-ColorManagement/` |
| Expected | `color-report.txt` + SHA-256 files for key renders |

## Run

```powershell
dotnet run --project MuPDF.NET\19-ColorManagement
```

## Related

| Example | Topic |
|---------|--------|
| [`05-Recolor`](../05-Recolor/) | Device colorspace conversion via `Document.Recolor` |
