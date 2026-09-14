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

    /// <summary>
    /// Read and update PDF document metadata.
    /// </summary>
    static void Metadata()
    {
        string input = ExamplePaths.MuPdfNetInput("sample.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "14-Metadata", "with-metadata.pdf");
        var check = new ResultCheck("MuPDF.NET", "14-Metadata");

        using (var doc = Document.Open(input))
        {
            // MetaData is a dictionary of standard PDF info keys (title, author, …).
            Dictionary<string, string> before = doc.MetaData;

            var sb = new StringBuilder();
            sb.Append("---before---\n");
            foreach (string key in before.Keys.OrderBy(k => k, StringComparer.Ordinal))
                sb.Append(key).Append('=').Append(before[key] ?? "").Append('\n');

            // SetMetadata replaces/merges the document info dictionary.
            doc.SetMetadata(new Dictionary<string, string>
            {
                ["title"] = "MuPDF.NET.Examples — Metadata",
                ["author"] = "Artifex",
                ["subject"] = "14-Metadata sample",
                ["creator"] = "MuPDF.NET.Examples",
            });

            doc.Save(output);

            // Re-read from the saved file for a stable dump.
            using (var saved = Document.Open(output))
            {
                sb.Append("---after---\n");
                foreach (string key in saved.MetaData.Keys.OrderBy(k => k, StringComparer.Ordinal))
                {
                    // Skip volatile date fields for Expected/ baselines.
                    if (key.Equals("creationDate", StringComparison.OrdinalIgnoreCase)
                        || key.Equals("modDate", StringComparison.OrdinalIgnoreCase)
                        || key.Equals("creationdate", StringComparison.OrdinalIgnoreCase)
                        || key.Equals("moddate", StringComparison.OrdinalIgnoreCase))
                        continue;
                    sb.Append(key).Append('=').Append(saved.MetaData[key] ?? "").Append('\n');
                }
            }

            string text = sb.ToString();
            File.WriteAllText(ExamplePaths.Output("MuPDF.NET", "14-Metadata", "metadata.txt"), text);
            ConsoleEx.Info($"Opened: {input}");
            ConsoleEx.Info($"Wrote: {output}");
            check.Text(text, "metadata.txt");
            check.Properties(PdfFingerprint.FromFile(output), "with-metadata.summary.txt");
        }

        check.Finish();
    }
}
