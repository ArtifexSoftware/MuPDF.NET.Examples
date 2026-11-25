# 01-UnlockFonts

Unlock Office document support and inspect font search directories.

`MuPDFOffice.Unlock` enables DOCX/HWPX (and other Office) handling on the shared MuPDF context. Empty / unset `MUPDF_OFFICE_KEY` is restricted mode (first 3 pages). The sample never prints the key. `fontPathAuto` is `true` so Unlock adds system font directories. `GetFontPath` returns those directories; they are machine-specific, so the listing only records whether at least one directory was registered.

## Sample method

`UnlockFonts()` in `Program.cs`.

## Package

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)

## Prerequisites

- Windows x64, Linux x64/arm64, or macOS (Office natives via RID packages).
- Optional `MUPDF_OFFICE_KEY`. When it is empty, `Unlock` runs in restricted mode.

## Input / output

| | Path |
|--|------|
| Output | `Output/MuPDF.NET.Office/01-UnlockFonts/unlock.txt` |
| Expected | `unlock.txt` + dump |

## Run

```powershell
dotnet run --project MuPDF.NET.Office\01-UnlockFonts
```

Expected checks only portable keys (`unlocked`, `fontDirCountMin1`) so the same baseline passes on Windows, Linux, and macOS.

## APIs used

- `MuPDFOffice.Unlock` (`fontPathAuto` is `true`)
- `MuPDFOffice.GetFontPath`
- `MuPDFOffice.IsUnlocked`
