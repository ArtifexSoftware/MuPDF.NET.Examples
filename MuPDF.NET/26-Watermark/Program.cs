using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.Watermark;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 26-Watermark");
        Watermark();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void Watermark()
    {
        string input = ExamplePaths.MuPdfNetInput("watermark.pdf");
        string stamp = ExamplePaths.MuPdfNetInput("watermark.png");
        string output = ExamplePaths.Output("MuPDF.NET", "26-Watermark", "watermarked.pdf");
        string beforePng = ExamplePaths.Output("MuPDF.NET", "26-Watermark", "before.png");
        string after0 = ExamplePaths.Output("MuPDF.NET", "26-Watermark", "page-0.png");
        string after1 = ExamplePaths.Output("MuPDF.NET", "26-Watermark", "page-1.png");
        var check = new ResultCheck("MuPDF.NET", "26-Watermark");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(beforePng);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                Watermark(input, stamp, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        using (var doc = Document.Open(output))
        {
            using (Page page = doc[0])
            using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
                pix.Save(after0);
            using (Page page = doc[1])
            using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
                pix.Save(after1);
        }

        check.Text(captured, "watermark.txt");
        check.Properties(PdfFingerprint.FromFile(output), "watermarked.summary.txt");
        check.FileSha256(beforePng, "before.png.sha256");
        check.FileSha256(after0, "page-0.png.sha256");
        check.FileSha256(after1, "page-1.png.sha256");
        check.Finish();
    }

    // doc-start:Watermark
    /// <summary>Insert a stamp image as a watermark on every page</summary>
    static void Watermark(string inputPath, string stampPath, string outputPath)
    {
        using var doc = Document.Open(inputPath);
        // Insert once as a reusable xref, then place on each page.
        int imageXref = -1;
        for (int i = 0; i < doc.PageCount; i++)
        {
            using Page page = doc[i];
            Rect bounds = page.Rect;
            // Centered watermark rectangle (~40% of page width).
            float w = bounds.Width * 0.4f;
            float h = bounds.Height * 0.4f;
            float x0 = bounds.X0 + (bounds.Width - w) / 2f;
            float y0 = bounds.Y0 + (bounds.Height - h) / 2f;
            var rect = new Rect(x0, y0, x0 + w, y0 + h);

            if (imageXref < 0)
                imageXref = page.InsertImage(rect, filename: stampPath, keepProportion: true, overlay: "true");
            else
                page.InsertImage(rect, xref: imageXref, keepProportion: true, overlay: "true");
            Console.WriteLine($"page={i} xref={imageXref}");
        }

        Console.WriteLine($"pageCount={doc.PageCount}");
        Console.WriteLine($"xref={imageXref}");
        doc.Save(outputPath);
    }
    // doc-end:Watermark
}
