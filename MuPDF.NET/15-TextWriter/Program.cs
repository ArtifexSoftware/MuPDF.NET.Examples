using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.TextWriterSample;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 15-TextWriter");
        TextWriterHello();
    }

    /// <summary>Write text onto a new page with TextWriter + Font</summary>
    static void TextWriterHello()
    {
        string output = "hello.pdf";

        using (var doc = Document.Open())
        {
            using Page page = doc.NewPage();

            // TextWriter lays out text into a rectangle; Font("helv") is the built-in Helvetica.
            var writer = new MuPDF.NET.TextWriter(page.Rect);
            writer.FillTextbox(
                new Rect(72, 72, 500, 200),
                "Hello from MuPDF.NET TextWriter!",
                new Font(fontName: "helv"));
            writer.WriteText(page);

            doc.Save(output);
        }
    }
}
