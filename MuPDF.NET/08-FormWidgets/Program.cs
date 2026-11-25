using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.FormWidgets;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 08-FormWidgets");
        FormWidgets();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void FormWidgets()
    {
        string input = ExamplePaths.MuPdfNetInput("widgets.pdf");
        string png = ExamplePaths.Output("MuPDF.NET", "08-FormWidgets", "widgets.png");
        var check = new ResultCheck("MuPDF.NET", "08-FormWidgets");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                FormWidgets(input);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(png);

        check.Text(captured, "widgets.txt");
        check.FileSha256(png, "widgets.png.sha256");
        check.Finish();
    }

    // doc-start:FormWidgets
    /// <summary>List AcroForm widgets on page 1 (name, type, value)</summary>
    static void FormWidgets(string inputPath)
    {
        using (var doc = Document.Open(inputPath))
        using (Page page = doc[0])
        {
            // Widgets() walks the page's form fields.
            var widgets = page.Widgets().ToList();
            Console.WriteLine($"widgetCount={widgets.Count}");

            foreach (Widget w in widgets.OrderBy(w => w.FieldName ?? "", StringComparer.Ordinal))
            {
                // FieldTypeString is human-readable (Text, CheckBox, …).
                Console.WriteLine($"{w.FieldName}\t{w.FieldTypeString}\t{w.FieldValue}");
            }
        }
    }
    // doc-end:FormWidgets
}
