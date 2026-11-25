using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.Metadata;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 14-Metadata");
        Metadata();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void Metadata()
    {
        string input = ExamplePaths.MuPdfNetInput("basic.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "14-Metadata", "with-metadata.pdf");
        string png = ExamplePaths.Output("MuPDF.NET", "14-Metadata", "with-metadata.png");
        var check = new ResultCheck("MuPDF.NET", "14-Metadata");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                Metadata(input, output);
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
            doc.Authenticate("user");
            using Page page = doc[0];
            using Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2));
            pix.Save(png);
        }

        check.Text(captured, "metadata.txt");
        check.Properties(PdfFingerprint.FromFile(output), "with-metadata.summary.txt");
        check.FileSha256(png, "with-metadata.png.sha256");
        check.Finish();
    }

    // doc-start:Metadata
    /// <summary>Read and update PDF document metadata</summary>
    static void Metadata(string inputPath, string outputPath)
    {
        using (var doc = Document.Open(inputPath))
        {
            // basic.pdf is AES-256; authenticate before reading the Info dictionary.
            doc.Authenticate("user");

            Console.WriteLine("---before---");
            DumpMetadata(doc.Metadata);

            // SetMetadata updates the PDF Info keys that are present in the dictionary.
            doc.SetMetadata(new Dictionary<string, string>
            {
                ["title"] = "MuPDF.NET.Examples — Metadata",
                ["author"] = "Artifex",
                ["subject"] = "14-Metadata sample",
                ["creator"] = "MuPDF.NET.Examples",
            });

            Console.WriteLine("---after---");
            DumpMetadata(doc.Metadata);

            doc.Save(outputPath);
        }

        static void DumpMetadata(Dictionary<string, string> meta)
        {
            foreach (string key in meta.Keys.OrderBy(k => k, StringComparer.Ordinal))
            {
                // creationDate / modDate are timestamps; omit them from the stable dump.
                if (key.Equals("creationDate", StringComparison.OrdinalIgnoreCase)
                    || key.Equals("modDate", StringComparison.OrdinalIgnoreCase))
                    continue;
                Console.WriteLine($"{key}={meta[key]}");
            }
        }
    }
    // doc-end:Metadata
}
