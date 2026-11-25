using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.StoryHtmlBox;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 06-StoryHtmlBox");
        StoryHtmlBox();
    }

    // help:skip — resolves output paths, calls the documented overload, then checks the harness baseline.
    static void StoryHtmlBox()
    {
        string output = ExamplePaths.Output("MuPDF.NET", "06-StoryHtmlBox", "story.pdf");
        string png = ExamplePaths.Output("MuPDF.NET", "06-StoryHtmlBox", "story.png");
        var check = new ResultCheck("MuPDF.NET", "06-StoryHtmlBox");
        var sb = new StringBuilder();

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                StoryHtmlBox(output);
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

        check.Text(captured, "story-info.txt");
        check.Properties(PdfFingerprint.FromFile(output), "story.summary.txt");
        check.FileSha256(png, "story.png.sha256");
        check.Finish();
    }

    // doc-start:StoryHtmlBox
    /// <summary>Lay out HTML into a page rectangle via Story / InsertHtmlBox</summary>
    static void StoryHtmlBox(string outputPath)
    {
        // HTML content for the Story engine (CSS inline styles are supported).
        const string html = """
        <html><body style="font-family:sans-serif;font-size:14pt;color:#111;">
        <h1>MuPDF.NET</h1>
        <p>Story / <b>Page.InsertHtmlBox</b> example for customer samples.</p>
        <ul>
        <li>Headings and paragraphs</li>
        <li>Bold inline markup</li>
        <li>Simple lists</li>
        </ul>
        </body></html>
        """;

        // Create an empty PDF and a default A4-ish page.
        using (var doc = Document.Open())
        {
            using Page page = doc.NewPage();

            // Target rectangle in PDF points (72 pt = 1 inch).
            var rect = new Rect(72, 72, 520, 720);

            // scaleLow: 0 = allow shrinking so content fits the box.
            (float spare, float scale) = page.InsertHtmlBox(rect, html, scaleLow: 0f);
            Console.WriteLine($"spare={spare}");
            Console.WriteLine($"scale={scale}");

            doc.Save(outputPath);
        }
    }
    // doc-end:StoryHtmlBox
}
