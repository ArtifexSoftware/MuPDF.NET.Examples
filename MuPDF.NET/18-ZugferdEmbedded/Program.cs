using System.Linq;
using System.Security.Cryptography;
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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ZugferdEmbedded()
    {
        string input = ExamplePaths.MuPdfNetInput("zugferd.pdf");
        string xmlPath = ExamplePaths.MuPdfNetInput("factur-x.xml");
        string output = ExamplePaths.Output("MuPDF.NET", "18-ZugferdEmbedded", "zugferd-with-xml.pdf");
        string extractedOut = ExamplePaths.Output("MuPDF.NET", "18-ZugferdEmbedded", "extracted-factur-x.xml");
        string png = ExamplePaths.Output("MuPDF.NET", "18-ZugferdEmbedded", "zugferd-with-xml.png");
        var check = new ResultCheck("MuPDF.NET", "18-ZugferdEmbedded");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                ZugferdEmbedded(input, xmlPath, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        using (var doc = Document.Open(output))
        {
            byte[] extracted = doc.GetEmbeddedFile("factur-x.xml");
            File.WriteAllBytes(extractedOut, extracted);
            using Page page = doc[0];
            using Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2));
            pix.Save(png);
        }

        check.Text(captured, "zugferd.txt");
        check.Properties(PdfFingerprint.FromFile(output), "zugferd-with-xml.summary.txt");
        check.FileSha256(png, "zugferd-with-xml.png.sha256");
        check.Finish();
    }

    // doc-start:ZugferdEmbedded
    /// <summary>Embed Factur-X / ZUGFeRD XML into an invoice PDF</summary>
    static void ZugferdEmbedded(string inputPath, string xmlPath, string outputPath)
    {
        byte[] xmlBytes = File.ReadAllBytes(xmlPath);
        const string embName = "factur-x.xml";

        using (var doc = Document.Open(inputPath))
        {
            Console.WriteLine("---before---");
            DumpEmbedded(doc);

            // Replace any prior Factur-X attachment with the same logical name.
            if (doc.GetEmbeddedFileNames().Contains(embName))
                doc.DeleteEmbeddedFile(embName);

            int xref = doc.AddEmbeddedFile(
                name: embName,
                buffer: xmlBytes,
                filename: embName,
                uFileName: embName,
                desc: "Factur-X / ZUGFeRD XML invoice");

            Console.WriteLine("---after---");
            Console.WriteLine($"xref={xref}");
            DumpEmbedded(doc);

            doc.Save(outputPath, garbage: 4, deflate: 1);
        }

        using (var doc = Document.Open(outputPath))
        {
            byte[] extracted = doc.GetEmbeddedFile(embName);
            bool match = extracted.AsSpan().SequenceEqual(xmlBytes);
            Console.WriteLine($"roundTripMatch={match.ToString().ToLowerInvariant()}");
        }

        static void DumpEmbedded(Document doc)
        {
            var names = doc.GetEmbeddedFileNames();
            Console.WriteLine($"embeddedCount={names.Count}");
            foreach (string name in names.OrderBy(n => n, StringComparer.Ordinal))
            {
                byte[] data = doc.GetEmbeddedFile(name);
                string sha = Convert.ToHexString(SHA256.HashData(data)).ToLowerInvariant();
                Console.WriteLine($"{name}\t{data.Length}\t{sha}");
            }
        }
    }
    // doc-end:ZugferdEmbedded
}
