using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.ZugferdEmbedded;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 18-ZugferdEmbedded");
        ZugferdEmbedded();
    }

    /// <summary>Extract ZUGFeRD / Factur-X XML from a PDF and re-embed it</summary>
    static void ZugferdEmbedded()
    {
        string pdfPath = "zugferd-muster-rechnung.pdf";
        string xmlPath = "zugferd-muster-rechnung.xml";
        string extractedOut = "extracted-factur-x.xml";
        string outputPdf = "zugferd-with-xml.pdf";

        const string embName = "factur-x.xml";
        byte[] xmlBytes = File.ReadAllBytes(xmlPath);

        // --- Extract any existing embedded files from the sample invoice PDF ---
        using (var doc = Document.Open(pdfPath))
        {

            foreach (string name in doc.GetEmbeddedFileNames())
            {
                byte[] data = doc.GetEmbeddedFile(name);
                // Prefer Factur-X / ZUGFeRD XML by name when present.
                if (name.Contains("factur", StringComparison.OrdinalIgnoreCase)
                    || name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    File.WriteAllBytes(extractedOut, data);
                }
            }
        }

        // --- Embed (or replace) the standalone XML into a copy of the PDF ---
        using (var doc = Document.Open(pdfPath))
        {
            if (doc.GetEmbeddedFileNames().Contains(embName))
                doc.DeleteEmbeddedFile(embName);

            int xref = doc.AddEmbeddedFile(
                name: embName,
                buffer: xmlBytes,
                filename: embName,
                uFileName: embName,
                desc: "Factur-X / ZUGFeRD XML invoice");

            doc.Save(outputPdf, garbage: 4, deflate: 1);
        }

        // Round-trip check + Expected dump.
        using (var verify = Document.Open(outputPdf))
        {
            byte[] extracted = verify.GetEmbeddedFile(embName);
            bool match = extracted.AsSpan().SequenceEqual(xmlBytes);

        }
    }
}
