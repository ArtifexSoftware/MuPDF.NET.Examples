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

    /// <summary>
    /// List AcroForm widgets on page 1 (name, type, value).
    /// </summary>
    static void FormWidgets()
    {
        string input = ExamplePaths.MuPdfNetInput("Widget.pdf");
        var check = new ResultCheck("MuPDF.NET", "08-FormWidgets");

        using var doc = Document.Open(input);
        using Page page = doc[0];

        // Widgets() walks the page's form fields.
        var widgets = page.Widgets().ToList();

        var sb = new StringBuilder();
        sb.Append("widgetCount=").Append(widgets.Count).Append('\n');
        foreach (Widget w in widgets.OrderBy(w => w.FieldName ?? "", StringComparer.Ordinal))
        {
            // FieldTypeString is human-readable (Text, CheckBox, …).
            sb.Append(w.FieldName ?? "(unnamed)")
                .Append('\t')
                .Append(w.FieldTypeString)
                .Append('\t')
                .Append(w.FieldValue ?? "")
                .Append('\n');
        }

        ConsoleEx.Info($"Opened: {input}");
        ConsoleEx.Info($"Widgets on page 1: {widgets.Count}");
        check.Text(sb.ToString(), "widgets.txt");
        check.Finish();
    }
}
