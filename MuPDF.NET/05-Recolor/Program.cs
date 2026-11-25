using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.Recolor;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 05-Recolor");
        Recolor();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void Recolor()
    {
        string input = ExamplePaths.MuPdfNetInput("color-shapes.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "05-Recolor", "recolor.pdf");
        string beforePng = ExamplePaths.Output("MuPDF.NET", "05-Recolor", "before.png");
        string afterPng = ExamplePaths.Output("MuPDF.NET", "05-Recolor", "after.png");
        var check = new ResultCheck("MuPDF.NET", "05-Recolor");

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(beforePng);

        Recolor(input, output);

        using (var doc = Document.Open(output))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(afterPng);

        check.Properties(PdfFingerprint.FromFile(output), "recolor.summary.txt");
        check.FileSha256(beforePng, "before.png.sha256");
        check.FileSha256(afterPng, "after.png.sha256");
        check.Finish();
    }

    // doc-start:Recolor
    /// <summary>Recolor page 1 to CMYK (4 components) and save</summary>
    static void Recolor(string inputPath, string outputPath)
    {
        using (var doc = Document.Open(inputPath))
        {
            // Recolor page 0 → 4 components (DeviceCMYK / ICC-based CMYK).
            doc.Recolor(0, 4);
            doc.Save(outputPath);
        }
    }
    // doc-end:Recolor
}
