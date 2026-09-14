using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.RenderPixmap;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 03-RenderPixmap");
        RenderPixmap();
    }

    /// <summary>
    /// Render page 1 to a PNG pixmap at 2× zoom.
    /// </summary>
    static void RenderPixmap()
    {
        string input = ExamplePaths.MuPdfNetInput("sample.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "03-RenderPixmap", "page-1.png");
        var check = new ResultCheck("MuPDF.NET", "03-RenderPixmap");

        // Open document and first page.
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        // Matrix(2,2) = 2× resolution (about 144 dpi from a 72 dpi page).
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
        {
            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Page 1 size: {page.Rect.Width:0.#} x {page.Rect.Height:0.#}");

            // Save the rendered bitmap as PNG.
            pix.Save(output);
        }

        // Examples harness: compare PNG by SHA-256.
        check.FileSha256(output, "page-1.png.sha256");
        check.Finish();
    }
}
