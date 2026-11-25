using System.Text;
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

    // help:skip — calls the documented overload, then checks the harness baseline.
    static void UnlockFonts()
    {
        string output = ExamplePaths.Output("MuPDF.NET.Office", "01-UnlockFonts", "unlock.txt");
        var check = new ResultCheck("MuPDF.NET.Office", "01-UnlockFonts");
        var sb = new StringBuilder();

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                UnlockFonts(output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        var dump = new StringBuilder();
        foreach (string line in captured.Split('\n'))
        {
            // Unlock may print a restricted/trial banner; it is not part of the listing.
            if (line.StartsWith("MuPDF.NET Office:", StringComparison.Ordinal))
                continue;
            dump.Append(line).Append('\n');
        }
        captured = dump.ToString();
        Console.Write(captured);

        check.Text(captured, "unlock-dump.txt");
        if (File.Exists(output))
            check.Text(File.ReadAllText(output), "unlock.txt");
        check.Finish();
    }

    // doc-start:UnlockFonts
    /// <summary>Unlock Office document support and inspect font search paths</summary>
    static void UnlockFonts(string outputPath)
    {
        // Empty / unset MUPDF_OFFICE_KEY = restricted mode (first 3 pages). Never print the key.
        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;

        // fontPathAuto: discover system font directories (Windows Fonts, fc-list on Linux, …).
        // Unlock returns 0 in restricted mode. GetFontPath strings are machine-specific.
        MuPDFOffice.Unlock(key, fontPathAuto: true);
        var fonts = MuPDFOffice.GetFontPath();

        var listing = new StringBuilder();
        listing.AppendLine($"unlocked={MuPDFOffice.IsUnlocked}");
        listing.AppendLine($"fontDirCountMin1={fonts.Count >= 1}");

        string text = listing.ToString();
        File.WriteAllText(outputPath, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        Console.Write(text);
    }
    // doc-end:UnlockFonts
}
