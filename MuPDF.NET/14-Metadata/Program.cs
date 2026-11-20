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

    /// <summary>Read and update PDF document metadata</summary>
    static void Metadata()
    {
        string input = "sample.pdf";
        string output = "with-metadata.pdf";

        using (var doc = Document.Open(input))
        {
            // MetaData is a dictionary of standard PDF info keys (title, author, …).
            Dictionary<string, string> before = doc.MetaData;

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
                foreach (string key in saved.MetaData.Keys.OrderBy(k => k, StringComparer.Ordinal))
                {
                    if (key.Equals("creationDate", StringComparison.OrdinalIgnoreCase)
                        || key.Equals("modDate", StringComparison.OrdinalIgnoreCase)
                        || key.Equals("creationdate", StringComparison.OrdinalIgnoreCase)
                        || key.Equals("moddate", StringComparison.OrdinalIgnoreCase))
                        continue;
                }
            }

        }
    }
}
