using System.Text;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.Office;

namespace MuPDF.NET.Examples.Office.Unlock;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.Office / 01-Unlock");
        Unlock();
    }

    // help:skip — calls the documented overload, then checks the harness baseline.
    static void Unlock()
    {
        string output = ExamplePaths.Output("MuPDF.NET.Office", "01-Unlock", "unlock.txt");
        var check = new ResultCheck("MuPDF.NET.Office", "01-Unlock");
        var sb = new StringBuilder();

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                Unlock(output);
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

        // Font directories are machine-specific. Expected keeps portable keys only.
        var portable = new StringBuilder();
        int fontDirs = 0;
        foreach (string line in captured.Split('\n'))
        {
            if (line.StartsWith("unlocked=", StringComparison.Ordinal))
                portable.Append(line).Append('\n');
            else if (line.StartsWith("fontPath=", StringComparison.Ordinal))
                fontDirs++;
        }
        portable.Append($"fontDirCountMin1={fontDirs >= 1}\n");
        string portableText = portable.ToString();
        check.Text(portableText, "unlock-dump.txt");
        check.Text(portableText, "unlock.txt");
        check.Finish();
    }

    // doc-start:Unlock
    /// <summary>Unlock Office document support and inspect font search paths</summary>
    static void Unlock(string outputPath)
    {
        // Empty / unset MUPDF_OFFICE_KEY = restricted mode (first 3 pages). Never print the key.
        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;

        // fontPathAuto: discover system font directories (Windows Fonts, fc-list on Linux, …).
        MuPDFOffice.Unlock(key, fontPathAuto: true);
        var fonts = MuPDFOffice.GetFontPath();

        var listing = new StringBuilder();
        listing.AppendLine($"unlocked={MuPDFOffice.IsUnlocked}");
        foreach (string dir in fonts)
            listing.AppendLine($"fontPath={dir}");

        string text = listing.ToString();
        File.WriteAllText(outputPath, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        Console.Write(text);
    }
    // doc-end:Unlock
}
