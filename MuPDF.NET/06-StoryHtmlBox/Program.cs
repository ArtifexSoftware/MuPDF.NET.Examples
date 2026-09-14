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

    /// <summary>
    /// Lay out HTML into a page rectangle via Story / InsertHtmlBox.
    /// </summary>
    static void StoryHtmlBox()
    {
        string output = ExamplePaths.Output("MuPDF.NET", "06-StoryHtmlBox", "story.pdf");
        var check = new ResultCheck("MuPDF.NET", "06-StoryHtmlBox");

        // HTML content for the Story engine (CSS inline styles are supported).
        const string html = """
<html><body style="font-family:sans-serif;font-size:14pt;color:#111;">
<h1>MuPDF.NET</h1>
<p>Story / <b>InsertHtmlbox</b> example for customer samples.</p>
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
            ConsoleEx.Info($"Inserted HTML box (spareHeight={spare:F1}, scale={scale:F3})");

            doc.Save(output);

            var props = PdfFingerprint.FromFile(output);
            props["scale"] = scale.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
            check.Properties(props, "story.summary.txt");
        }

        check.Finish();
    }
}
