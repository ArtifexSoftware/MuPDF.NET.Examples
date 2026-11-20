using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.GetDrawings;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 22-GetDrawings");
        GetDrawings();
    }

    /// <summary>Draw vector shapes, then extract path info with &lt;see cref="Page.GetDrawings"/&gt;</summary>
    static void GetDrawings()
    {
        string drawn = "drawn.pdf";
        string redrawn = "redrawn.pdf";

        // 1) Create a page with known vector content.
        using (var doc = Document.Open())
        {
            using Page page = doc.NewPage();
            page.DrawLine(new Point(72, 100), new Point(500, 100), width: 1f, dashes: "[5] 0");
            page.DrawRect(new Rect(72, 180, 220, 280), color: new[] { 0f, 0f, 1f }, width: 1.5f);
            page.DrawCircle(
                new Point(350, 230),
                radius: 40,
                color: new[] { 0f, 0f, 0f },
                fill: new[] { 0f, 0.6f, 0f },
                width: 1f);
            doc.Save(drawn);
        }

        // 2) Extract drawings and rewrite a subset onto a blank page.
        using (var src = Document.Open(drawn))
        using (Page page = src[0])
        {
            List<PathInfo> paths = page.GetDrawings();

            var typeCounts = paths
                .GroupBy(p => p.Type ?? "?")
                .OrderBy(g => g.Key, StringComparer.Ordinal);

            using var outDoc = Document.Open();
            using Page outPage = outDoc.NewPage();
            int redrawnRects = 0;
            foreach (PathInfo path in paths)
            {
                if (path.Rect == null || path.Rect.IsEmpty)
                    continue;
                // Stroke each path's bounding box so the sample stays short and stable.
                outPage.DrawRect(path.Rect, color: new[] { 1f, 0f, 0f }, width: 0.5f);
                redrawnRects++;
            }
            outDoc.Save(redrawn);
        }
    }
}
