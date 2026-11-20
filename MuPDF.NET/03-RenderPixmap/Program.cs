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

    /// <summary>Render page 1 to a PNG pixmap at 2× zoom</summary>
    static void RenderPixmap()
    {
        string input = "sample.pdf";
        string output = "page-1.png";

        // Open document and first page.
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        // Matrix(2,2) = 2× resolution (about 144 dpi from a 72 dpi page).
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
        {

            // Save the rendered bitmap as PNG.
            pix.Save(output);
        }
    }
}
