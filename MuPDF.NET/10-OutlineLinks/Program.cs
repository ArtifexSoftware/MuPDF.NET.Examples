using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.OutlineLinks;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 10-OutlineLinks");
        OutlineLinks();
    }

    /// <summary>
    /// Build a TOC (bookmarks) and an internal go-to link.
    /// </summary>
    static void OutlineLinks()
    {
        string output = ExamplePaths.Output("MuPDF.NET", "10-OutlineLinks", "outline-links.pdf");
        var check = new ResultCheck("MuPDF.NET", "10-OutlineLinks");

        using (var doc = Document.Open())
        {
            // Page 1: heading + blue hot-spot rectangle (link target area).
            using (Page p0 = doc.NewPage())
            {
                var linkFrom = new Rect(72, 200, 200, 240);
                var writer = new MuPDF.NET.TextWriter(p0.Rect);
                writer.FillTextbox(new Rect(72, 72, 500, 120), "Chapter 1 — Overview", new Font(fontName: "helv"));
                writer.FillTextbox(linkFrom, "Go to Chapter 2", new Font(fontName: "helv"), fontSize: 11);
                writer.WriteText(p0);
                p0.DrawRect(linkFrom, color: new[] { 0f, 0f, 1f });
            }

            // Page 2: second chapter.
            using (Page p1 = doc.NewPage())
            {
                var writer = new MuPDF.NET.TextWriter(p1.Rect);
                writer.FillTextbox(new Rect(72, 72, 500, 120), "Chapter 2 — Details", new Font(fontName: "helv"));
                writer.WriteText(p1);
                p1.DrawRect(new Rect(72, 200, 200, 240), color: new[] { 0f, 0.5f, 0f });
            }

            // TOC rows: [level, title, 1-based page]. Level 2 nests under the previous level 1.
            doc.SetToc(new List<object>
            {
                new List<object> { 1, "Overview", 1 },
                new List<object> { 1, "Details", 2 },
                new List<object> { 2, "Details subsection", 2 },
            });

            // Internal link: click the blue rect on page 1 → jump to page 2 (0-based page index).
            using (Page p0 = doc[0])
            {
                p0.InsertLink(new Dictionary<string, object>
                {
                    ["kind"] = Constants.LinkGoto,
                    ["from"] = new Rect(72, 200, 200, 240), // same rect as "Go to Chapter 2" label
                    ["page"] = 1,
                    ["to"] = new Point(72, 72),
                });
            }

            doc.Save(output);
        }

        // Re-open and dump TOC + links for the Expected/ baseline.
        using (var doc = Document.Open(output))
        {
            var sb = new StringBuilder();
            foreach (var item in doc.GetToc(simple: true))
                sb.Append(item.level).Append('\t').Append(item.title).Append('\t').Append(item.page).Append('\n');

            sb.Append("---links---\n");
            for (int i = 0; i < doc.PageCount; i++)
            {
                using Page page = doc[i];
                foreach (LinkInfo link in page.GetLinks().OrderBy(l => l.From?.Y0 ?? 0).ThenBy(l => l.From?.X0 ?? 0))
                {
                    sb.Append("p").Append(i + 1)
                        .Append('\t').Append((int)link.Kind)
                        .Append('\t').Append(link.Page)
                        .Append('\t').Append(FormatRect(link.From))
                        .Append('\n');
                }
            }

            string text = sb.ToString();
            File.WriteAllText(ExamplePaths.Output("MuPDF.NET", "10-OutlineLinks", "outline-links.txt"), text);
            ConsoleEx.Info($"Wrote: {output}");
            check.Text(text, "outline-links.txt");
            check.Properties(PdfFingerprint.FromFile(output), "outline-links.summary.txt");
        }

        check.Finish();
    }

    static string FormatRect(Rect? r)
    {
        if (r == null)
            return "";
        return string.Create(System.Globalization.CultureInfo.InvariantCulture,
            $"{r.X0:0.##},{r.Y0:0.##},{r.X1:0.##},{r.Y1:0.##}");
    }
}
