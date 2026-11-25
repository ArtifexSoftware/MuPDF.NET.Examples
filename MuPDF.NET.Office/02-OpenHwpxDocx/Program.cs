using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.Office;

namespace MuPDF.NET.Examples.Office.OpenHwpxDocx;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.Office / 02-OpenHwpxDocx");
        OpenHwpxDocx();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void OpenHwpxDocx()
    {
        string docx = ExamplePaths.OfficeInput("pier-notice.docx");
        string hwpx = ExamplePaths.OfficeInput("pier-notice.hwpx");
        string output = ExamplePaths.Output("MuPDF.NET.Office", "02-OpenHwpxDocx", "open.txt");
        string pngDocx = ExamplePaths.Output("MuPDF.NET.Office", "02-OpenHwpxDocx", "docx-page.png");
        string pngHwpx = ExamplePaths.Output("MuPDF.NET.Office", "02-OpenHwpxDocx", "hwpx-page.png");
        var check = new ResultCheck("MuPDF.NET.Office", "02-OpenHwpxDocx");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {docx}");
        ConsoleEx.Info($"Opened: {hwpx}");

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                OpenHwpxDocx(docx, hwpx, output);
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

        using (var d1 = Document.Open(docx))
        using (Page page = d1[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(pngDocx);

        using (var d2 = Document.Open(hwpx))
        using (Page page = d2[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(pngHwpx);

        check.Text(captured, "open-dump.txt");
        if (File.Exists(output))
            check.Text(File.ReadAllText(output), "open.txt");
        check.Finish();
    }

    // doc-start:OpenHwpxDocx
    /// <summary>Unlock Office, then open DOCX and HWPX via Document.Open</summary>
    static void OpenHwpxDocx(string docxPath, string hwpxPath, string outputPath)
    {
        // Empty / unset MUPDF_OFFICE_KEY = restricted mode (first 3 pages). Never print the key.
        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;
        MuPDFOffice.Unlock(key, fontPathAuto: true);

        var listing = new StringBuilder();
        using (var docx = Document.Open(docxPath))
        using (Page docxPage = docx[0])
        {
            listing.AppendLine($"docxPages={docx.PageCount}");
            listing.AppendLine($"docxPage0={CollapseText(docxPage.GetText())}");
        }

        using (var hwpx = Document.Open(hwpxPath))
        using (Page hwpxPage = hwpx[0])
        {
            listing.AppendLine($"hwpxPages={hwpx.PageCount}");
            listing.AppendLine($"hwpxPage0={CollapseText(hwpxPage.GetText())}");
        }

        string text = listing.ToString();
        File.WriteAllText(outputPath, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        Console.Write(text);
    }

    static string CollapseText(object? value)
    {
        string s = value?.ToString() ?? "";
        var sb = new StringBuilder(s.Length);
        bool space = false;
        foreach (char c in s)
        {
            if (char.IsWhiteSpace(c))
            {
                space = true;
                continue;
            }
            if (space && sb.Length > 0)
                sb.Append(' ');
            space = false;
            sb.Append(c);
        }
        return sb.ToString();
    }
    // doc-end:OpenHwpxDocx
}
