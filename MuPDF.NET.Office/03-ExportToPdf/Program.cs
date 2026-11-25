using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;
using MuPDF.NET.Office;

namespace MuPDF.NET.Examples.Office.ExportToPdf;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET.Office / 03-ExportToPdf");
        ExportToPdf();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ExportToPdf()
    {
        string input = ExamplePaths.OfficeInput("pier-yard.docx");
        string output = ExamplePaths.Output("MuPDF.NET.Office", "03-ExportToPdf", "pier-yard.pdf");
        string listing = ExamplePaths.Output("MuPDF.NET.Office", "03-ExportToPdf", "export.txt");
        var check = new ResultCheck("MuPDF.NET.Office", "03-ExportToPdf");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                ExportToPdf(input, output, listing);
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

        RenderPages(input, "docx-page");
        RenderPages(output, "pdf-page");

        check.Text(captured, "export-dump.txt");
        if (File.Exists(listing))
            check.Text(File.ReadAllText(listing), "export.txt");
        check.Finish();

        static void RenderPages(string path, string prefix)
        {
            using var doc = Document.Open(path);
            for (int i = 0; i < doc.PageCount; i++)
            {
                string png = ExamplePaths.Output("MuPDF.NET.Office", "03-ExportToPdf", $"{prefix}-{i}.png");
                using Page page = doc[i];
                using Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2));
                pix.Save(png);
            }
        }
    }

    // doc-start:ExportToPdf
    /// <summary>Export an Office document to PDF</summary>
    static void ExportToPdf(string inputPath, string outputPath, string listingPath)
    {
        // Empty / unset MUPDF_OFFICE_KEY = restricted mode (first 3 pages). Never print the key.
        string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
        if (string.IsNullOrEmpty(key))
            key = null;
        MuPDFOffice.Unlock(key, fontPathAuto: true);

        // When outputPath is set, ToPdf writes the file and returns null.
        // PDF bytes embed OS fonts, so the listing is page count + collapsed text.
        MuPDFOffice.ToPdf(inputPath, outputPath);

        var listing = new StringBuilder();
        using (var doc = Document.Open(outputPath))
        {
            listing.AppendLine($"pageCount={doc.PageCount}");
            for (int i = 0; i < doc.PageCount; i++)
            {
                using Page page = doc[i];
                listing.AppendLine($"page{i}={CollapseText(page.GetText())}");
            }
        }

        string text = listing.ToString();
        File.WriteAllText(listingPath, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
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
    // doc-end:ExportToPdf
}
