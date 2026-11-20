using MuPDF.NET.Examples.Common;
using MuPDF.NET.Office;

namespace MuPDF.NET.Examples.Office.UnlockFonts;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.Office / 01-UnlockFonts");
        UnlockFonts();
    }

    // doc-start:UnlockFonts
    /// <summary>Unlock Office document support and inspect font search paths</summary>
    static void UnlockFonts()
    {
        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;

        // fontPathAuto: discover system font directories (Windows Fonts, fc-list on Linux, …).
        int flags = MuPDFOffice.Unlock(key, fontPathAuto: true);
        var fonts = MuPDFOffice.GetFontPath();
    }
    // doc-end:UnlockFonts
}
