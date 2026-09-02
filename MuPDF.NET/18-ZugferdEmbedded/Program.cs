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

    /// <summary>
    /// Extract ZUGFeRD / Factur-X XML from a PDF and re-embed it.
    /// </summary>
    static void ZugferdEmbedded()
    {
        string pdfPath = ExamplePaths.MuPdfNetInput("zugferd-muster-rechnung.pdf");
        string xmlPath = ExamplePaths.MuPdfNetInput("zugferd-muster-rechnung.xml");
        string extractedOut = ExamplePaths.Output("MuPDF.NET", "18-ZugferdEmbedded", "extracted-factur-x.xml");
        string outputPdf = ExamplePaths.Output("MuPDF.NET", "18-ZugferdEmbedded", "zugferd-with-xml.pdf");
        var check = new ResultCheck("MuPDF.NET", "18-ZugferdEmbedded");

        const string embName = "factur-x.xml";
        byte[] xmlBytes = File.ReadAllBytes(xmlPath);

        // --- Extract any existing embedded files from the sample invoice PDF ---
        using (var doc = Document.Open(pdfPath))
        {
            ConsoleEx.Info($"Opened: {pdfPath}");
            ConsoleEx.Info($"EmbeddedFileCount: {doc.EmbeddedFileCount}");

            foreach (string name in doc.GetEmbeddedFileNames())
            {
                byte[] data = doc.GetEmbeddedFile(name);
                // Prefer Factur-X / ZUGFeRD XML by name when present.
                if (name.Contains("factur", StringComparison.OrdinalIgnoreCase)
                    || name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    File.WriteAllBytes(extractedOut, data);
                    ConsoleEx.Info($"Extracted '{name}' → {extractedOut} ({data.Length} bytes)");
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

            ConsoleEx.Info($"Added '{embName}' xref={xref}");
            doc.Save(outputPdf, garbage: 4, deflate: 1);
        }

        // Round-trip check + Expected dump.
        using (var verify = Document.Open(outputPdf))
        {
            byte[] extracted = verify.GetEmbeddedFile(embName);
            bool match = extracted.AsSpan().SequenceEqual(xmlBytes);

            var sb = new StringBuilder();
            sb.Append("embeddedCount=").Append(verify.EmbeddedFileCount).Append('\n');
            sb.Append("facturX=").Append(embName).Append('\n');
            sb.Append("xmlSha256=").Append(ResultCheck.Sha256HexBytes(extracted)).Append('\n');
            sb.Append("roundTripMatch=").Append(match ? "true" : "false").Append('\n');

            string text = sb.ToString();
            File.WriteAllText(ExamplePaths.Output("MuPDF.NET", "18-ZugferdEmbedded", "zugferd.txt"), text);
            ConsoleEx.Info(match ? "Round-trip OK" : "Round-trip MISMATCH");
            check.Text(text, "zugferd.txt");
            check.Properties(PdfFingerprint.FromFile(outputPdf), "zugferd-with-xml.summary.txt");
        }

        check.Finish();
    }
}
