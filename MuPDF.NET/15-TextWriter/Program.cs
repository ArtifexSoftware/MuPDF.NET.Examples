using System.Globalization;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.TextWriterSample;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 15-TextWriter");
        WriteTextPage();
    }

    // help:skip — resolves output paths, calls the documented overload, then checks the harness baseline.
    static void WriteTextPage()
    {
        string output = ExamplePaths.Output("MuPDF.NET", "15-TextWriter", "textwriter.pdf");
        string png = ExamplePaths.Output("MuPDF.NET", "15-TextWriter", "textwriter.png");
        var check = new ResultCheck("MuPDF.NET", "15-TextWriter");
        var sb = new StringBuilder();

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                WriteTextPage(output);
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

        check.Text(captured, "textwriter.txt");
        check.Properties(PdfFingerprint.FromFile(output), "textwriter.summary.txt");
        check.FileSha256(png, "textwriter.png.sha256");
        check.Finish();
    }

    // doc-start:WriteTextPage
    /// <summary>Write text onto a new page with TextWriter and Font</summary>
    static void WriteTextPage(string outputPath)
    {
        using (var doc = Document.Open())
        {
            using Page page = doc.NewPage(width: 500, height: 280);
            var helv = new Font(fontName: "helv");
            var hebo = new Font(fontName: "hebo");

            // TextWriter lays out spans against the page rectangle, then WriteText paints them.
            var writer = new TextWriter(page.Rect);
            writer.FillTextbox(
                new Rect(36, 36, 464, 80),
                "MuPDF.NET TextWriter",
                hebo,
                fontSize: 18,
                align: Constants.TextAlignCenter);
            writer.FillTextbox(
                new Rect(36, 100, 464, 200),
                "FillTextbox wraps a paragraph inside a rectangle.\nAppend writes a span from a Point.",
                helv,
                fontSize: 12);
            writer.Append(new Point(36, 240), "Append starts at a Point.", helv, fontSize: 12);
            writer.WriteText(page);

            Point lp = writer.LastPoint;
            Rect tr = writer.TextRect;
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"lastPoint={lp.X:0.##},{lp.Y:0.##}"));
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"textRect={tr.X0:0.##},{tr.Y0:0.##},{tr.X1:0.##},{tr.Y1:0.##}"));
            Console.WriteLine("---text---");
            Console.WriteLine(page.GetText("text") ?? "");

            doc.Save(outputPath);
        }
    }
    // doc-end:WriteTextPage
}
