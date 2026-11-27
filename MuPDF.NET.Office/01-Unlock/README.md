# 01-Unlock

Unlock Office document support and inspect font search directories.

`MuPDFOffice.Unlock` enables DOCX/HWPX (and other Office) handling on the shared MuPDF context. Empty / unset `MUPDF_OFFICE_KEY` is restricted mode (first 3 pages). The sample never prints the key. `fontPathAuto` is `true` so Unlock adds system font directories. `GetFontPath` returns those directories; the listing prints each path. Paths are machine-specific, so Expected only records `unlocked` and whether at least one directory was registered.

## Sample method

`Unlock()` in `Program.cs`.

## Package

- [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office)

## Prerequisites

- Windows x64, Linux x64/arm64, or macOS (Office natives via RID packages).
- Optional `MUPDF_OFFICE_KEY`. When it is empty, `Unlock` runs in restricted mode.

## Input / output

| | Path |
|--|------|
| Output | `Output/MuPDF.NET.Office/01-Unlock/unlock.txt` |
| Expected | `unlock.txt` + dump |

## Run

```powershell
dotnet run --project MuPDF.NET.Office\01-Unlock
```

Expected checks only portable keys (`unlocked`, `fontDirCountMin1`) so the same baseline passes on Windows, Linux, and macOS.

## APIs used

- `MuPDFOffice.Unlock` (`fontPathAuto` is `true`)
- `MuPDFOffice.GetFontPath`
- `MuPDFOffice.IsUnlocked`
