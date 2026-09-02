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

    /// <summary>
    /// Unlock Office document support and inspect font search paths.
    /// </summary>
    static void UnlockFonts()
    {
        var check = new ResultCheck("MuPDF.NET.Office", "01-UnlockFonts");

        // Optional key from MUPDF_OFFICE_KEY; null still enables restricted mode for samples.
        string? key = OfficeLicense.KeyFromEnvironment();

        // fontPathAuto: discover system font directories (Windows Fonts, fc-list on Linux, …).
        int flags = MuPDFOffice.Unlock(key, fontPathAuto: true);
        var fonts = MuPDFOffice.GetFontPath();

        ConsoleEx.Info($"Unlocked: {MuPDFOffice.IsUnlocked}");
        ConsoleEx.Info($"Key flags: 0x{flags:X}");
        ConsoleEx.Info($"Font directory count: {fonts.Count}");

        // Portable baseline only — absolute font paths and Windows-only dirs differ by OS.
        check.Properties(
            new Dictionary<string, string>
            {
                ["unlocked"] = MuPDFOffice.IsUnlocked ? "true" : "false",
                ["fontDirCountMin1"] = fonts.Count >= 1 ? "true" : "false",
            },
            "unlock.summary.txt");

        check.Finish();
    }
}
