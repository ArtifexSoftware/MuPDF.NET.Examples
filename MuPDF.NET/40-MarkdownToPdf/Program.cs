using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.Llm.MarkdownToPdf;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 40-MarkdownToPdf");
        MarkdownToPdf();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void MarkdownToPdf()
    {
        string input = ExamplePaths.MuPdfNetInput("north-pier.md");
        string output = ExamplePaths.Output("MuPDF.NET", "40-MarkdownToPdf", "north-pier.pdf");
        string png = ExamplePaths.Output("MuPDF.NET", "40-MarkdownToPdf", "page.png");
        var check = new ResultCheck("MuPDF.NET", "40-MarkdownToPdf");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                MarkdownToPdf(input, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        using (var doc = Document.Open(output))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(png);

        check.Text(captured, "markdown-to-pdf.txt");
        check.Properties(PdfFingerprint.FromFile(output), "north-pier.summary.txt");
        check.FileSha256(png, "page.png.sha256");
        check.Finish();
    }

    // doc-start:MarkdownToPdf
    /// <summary>Render a Markdown file to PDF via Utils.MarkdownToPdf (Story)</summary>
    static void MarkdownToPdf(string inputPath, string outputPath)
    {
        // Compact page so the Story layout matches the docs image (default is A4).
        var pageRect = new Rect(0, 0, 500, 400);
        float[] margins = { 36, 36, 36, 36 };

        // When outputPath is set, the PDF is written to disk (return value is null).
        Utils.MarkdownToPdf(
            inputPath,
            pageRect: pageRect,
            margins: margins,
            outputPath: outputPath);

        using var doc = Document.Open(outputPath);
        using Page page = doc[0];
        Console.WriteLine($"pageCount={doc.PageCount}");
        Console.WriteLine($"width={page.Rect.Width}");
        Console.WriteLine($"height={page.Rect.Height}");
    }
    // doc-end:MarkdownToPdf
}
