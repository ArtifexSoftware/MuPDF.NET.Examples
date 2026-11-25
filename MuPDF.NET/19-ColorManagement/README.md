# 19-ColorManagement

ICC / print color: OutputIntent, DeviceN/spot separations, overprint, device RGB/Gray/CMYK, and rendering intents.

`color-managed.pdf` is a 2-page fixture with RGB patches, a `SpotRed` separation, overprint, an OutputIntent, a TOC, and an internal link. Display/proof ICC files stay under `Input/MuPDF.NET/color/`.

For simple device-colorspace conversion of page content, see [`05-Recolor`](../05-Recolor/).

## Sample methods

`Program.cs` splits the work like `01-Basic`:

- `Viewing` — 144 dpi RGB pixmap
- `DeviceColorspaces` — DeviceRGB / DeviceGray / DeviceCMYK
- `ObjectInspection` — catalog `OutputIntents` and xref objects
- `ZoomAndNavigation` — 2× zoom, links, TOC
- `IccProfiles` — load `NULL.icc` / `Proof.icc`
- `ColorManagedRendering` — display-profile render
- `OutputIntent` — `fz_document_output_intent`
- `DeviceNSeparations` — `SpotRed`
- `Overprint` — `fz_page_uses_overprint`
- `RenderingIntent` — perceptual vs relative `fz_convert_pixmap`
- `Prepress` — counts of OutputIntent / overprint / separation pages
- `IccSoftProof` — `fz_enable_icc` + proof profile
- helpers: `RenderIccPage`, `LoadIccColorspace`, `DumpColorspace`

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/color-managed.pdf`, `color/NULL.icc`, `color/Proof.icc` |
| Output | PNGs / PAM under `Output/MuPDF.NET/19-ColorManagement/` |
| Expected | `color-report.txt`, `device-rgb.png.sha256`, `device-gray.png.sha256`, `soft-proof.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\19-ColorManagement
```

Recreate the fixture with `python Test/create_color_managed_pdf.py`.

## APIs used

- `Page.GetPixmap` (`Colorspace.Rgb` / `Gray` / `Cmyk`)
- `Document.XrefGetKey` / `PdfCatalog`
- `FzDocument.fz_document_output_intent`
- `FzPage.fz_page_separations` / `fz_page_uses_overprint`
- `FzPixmap.fz_convert_pixmap` (rendering intent / overprint flags)

## Related

| Example | Topic |
|---------|--------|
| [`05-Recolor`](../05-Recolor/) | Device colorspace conversion via `Document.Recolor` |
