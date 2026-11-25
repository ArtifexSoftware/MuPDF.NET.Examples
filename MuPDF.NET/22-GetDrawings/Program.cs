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

    // help:skip — resolves output paths, calls the documented overload, then checks the harness baseline.
    static void GetDrawings()
    {
        string drawn = ExamplePaths.Output("MuPDF.NET", "22-GetDrawings", "drawn.pdf");
        string redrawn = ExamplePaths.Output("MuPDF.NET", "22-GetDrawings", "redrawn.pdf");
        string drawnPng = ExamplePaths.Output("MuPDF.NET", "22-GetDrawings", "drawn.png");
        string redrawnPng = ExamplePaths.Output("MuPDF.NET", "22-GetDrawings", "redrawn.png");
        var check = new ResultCheck("MuPDF.NET", "22-GetDrawings");
        var sb = new StringBuilder();

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                GetDrawings(drawn, redrawn);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        using (var doc = Document.Open(drawn))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(drawnPng);

        using (var doc = Document.Open(redrawn))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(redrawnPng);

        check.Text(captured, "drawings.txt");
        check.Properties(PdfFingerprint.FromFile(drawn), "drawn.summary.txt");
        check.Properties(PdfFingerprint.FromFile(redrawn), "redrawn.summary.txt");
        check.FileSha256(drawnPng, "drawn.png.sha256");
        check.FileSha256(redrawnPng, "redrawn.png.sha256");
        check.Finish();
    }

    // doc-start:GetDrawings
    /// <summary>Draw vector shapes, then extract path info with Page.GetDrawings</summary>
    static void GetDrawings(string drawnPath, string redrawnPath)
    {
        using (var doc = Document.Open())
        {
            using Page page = doc.NewPage(width: 540, height: 320);
            page.DrawLine(new Point(72, 100), new Point(468, 100), width: 1f, dashes: "[5] 0");
            page.DrawRect(new Rect(72, 180, 220, 280), color: new[] { 0f, 0f, 1f }, width: 1.5f);
            page.DrawCircle(
                new Point(350, 230),
                radius: 40,
                color: new[] { 0f, 0f, 0f },
                fill: new[] { 0f, 0.6f, 0f },
                width: 1f);
            doc.Save(drawnPath);
        }

        using (var src = Document.Open(drawnPath))
        using (Page page = src[0])
        {
            List<PathInfo> paths = page.GetDrawings();
            Console.WriteLine($"pathCount={paths.Count}");
            foreach (PathInfo path in paths)
            {
                string itemTypes = path.Items == null
                    ? ""
                    : string.Join(",", path.Items.Select(i => i.Type));
                string rect = path.Rect == null
                    ? "none"
                    : $"{path.Rect.X0:0},{path.Rect.Y0:0},{path.Rect.X1:0},{path.Rect.Y1:0}";
                Console.WriteLine($"seq={path.SeqNo} type={path.Type} items={itemTypes} rect={rect}");
            }

            using var outDoc = Document.Open();
            using Page outPage = outDoc.NewPage(width: 540, height: 320);
            int redrawnRects = 0;
            foreach (PathInfo path in paths)
            {
                Rect box = path.Rect;
                if (box == null)
                    continue;
                // A horizontal/vertical stroke has a degenerate bbox (IsEmpty). Draw that
                // segment instead of skipping it so every GetDrawings path is visible.
                if (box.IsEmpty)
                    outPage.DrawLine(
                        new Point(box.X0, box.Y0),
                        new Point(box.X1, box.Y1),
                        color: new[] { 1f, 0f, 0f },
                        width: 0.5f);
                else
                    outPage.DrawRect(box, color: new[] { 1f, 0f, 0f }, width: 0.5f);
                redrawnRects++;
            }
            Console.WriteLine($"redrawnRects={redrawnRects}");
            outDoc.Save(redrawnPath);
        }
    }
    // doc-end:GetDrawings
}
