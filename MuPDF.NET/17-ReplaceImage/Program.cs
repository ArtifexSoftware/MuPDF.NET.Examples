using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.ReplaceImage;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 17-ReplaceImage");
        ReplaceImage();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ReplaceImage()
    {
        string input = ExamplePaths.MuPdfNetInput("images.pdf");
        string replacement = ExamplePaths.MuPdfNetInput("circle.png");
        string output = ExamplePaths.Output("MuPDF.NET", "17-ReplaceImage", "replaced.pdf");
        string beforePng = ExamplePaths.Output("MuPDF.NET", "17-ReplaceImage", "before.png");
        string afterPng = ExamplePaths.Output("MuPDF.NET", "17-ReplaceImage", "after.png");
        var check = new ResultCheck("MuPDF.NET", "17-ReplaceImage");
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
                ReplaceImage(input, replacement, output);
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
            pix.Save(afterPng);

        check.Text(captured, "replaced.txt");
        check.Properties(PdfFingerprint.FromFile(output), "replaced.summary.txt");
        check.FileSha256(beforePng, "before.png.sha256");
        check.FileSha256(afterPng, "after.png.sha256");
        check.Finish();
    }

    // doc-start:ReplaceImage
    /// <summary>Replace the first image on a page</summary>
    static void ReplaceImage(string inputPath, string replacementPath, string outputPath)
    {
        using (var doc = Document.Open(inputPath))
        using (Page page = doc[0])
        {
            // images.pdf has two rasters; replace the first xref (triangle.png).
            int xref = page.GetImages(full: true)[0].Xref;

            Console.WriteLine("---before---");
            DumpImage(page, xref);

            // ReplaceImage copies the new image onto the same xref (draw matrix stays).
            page.ReplaceImage(xref, filename: replacementPath);

            Console.WriteLine("---after---");
            DumpImage(page, xref);

            doc.Save(outputPath);
        }

        static void DumpImage(Page page, int xref)
        {
            Entry img = page.GetImages(full: true).First(i => i.Xref == xref);
            Console.WriteLine($"xref={img.Xref} name={img.Name} width={img.Width} height={img.Height} bpc={img.Bpc} cs={img.CsName}");
        }
    }
    // doc-end:ReplaceImage
}
