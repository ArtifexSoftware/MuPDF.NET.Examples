using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.EmbeddedFiles;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 13-EmbeddedFiles");
        EmbeddedFiles();
    }

    /// <summary>
    /// Attach a file to a PDF and list embedded attachments.
    /// </summary>
    static void EmbeddedFiles()
    {
        string blank = ExamplePaths.MuPdfNetInput("Blank.pdf");
        string note = ExamplePaths.MuPdfNetInput("note.txt");
        string output = ExamplePaths.Output("MuPDF.NET", "13-EmbeddedFiles", "with-attachment.pdf");
        var check = new ResultCheck("MuPDF.NET", "13-EmbeddedFiles");

        byte[] payload = File.ReadAllBytes(note);
        const string embName = "note.txt";

        using (var doc = Document.Open(blank))
        {
            // Replace any prior attachment with the same logical name.
            if (doc.GetEmbeddedFileNames().Contains(embName))
                doc.DeleteEmbeddedFile(embName);

            // AddEmbeddedFile stores bytes in the PDF EmbeddedFiles name tree.
            int xref = doc.AddEmbeddedFile(
                name: embName,
                buffer: payload,
                filename: embName,
                uFileName: embName,
                desc: "Example attachment");

            ConsoleEx.Info($"Added embedded file '{embName}' (xref={xref}, {payload.Length} bytes)");
            doc.Save(output, garbage: 4, deflate: 1);
        }

        // Verify by listing names and hashing extracted bytes.
        using (var doc = Document.Open(output))
        {
            var sb = new StringBuilder();
            sb.Append("embeddedCount=").Append(doc.EmbeddedFileCount).Append('\n');
            foreach (string name in doc.GetEmbeddedFileNames().OrderBy(n => n, StringComparer.Ordinal))
            {
                var info = doc.GetEmbeddedFileInfo(name);
                byte[] data = doc.GetEmbeddedFile(name);
                sb.Append(name)
                    .Append('\t').Append(info.GetValueOrDefault("filename"))
                    .Append('\t').Append(info.GetValueOrDefault("size") ?? data.Length)
                    .Append('\t').Append(ResultCheck.Sha256HexBytes(data))
                    .Append('\n');
            }

            string text = sb.ToString();
            File.WriteAllText(ExamplePaths.Output("MuPDF.NET", "13-EmbeddedFiles", "embedded.txt"), text);
            ConsoleEx.Info($"EmbeddedFileCount: {doc.EmbeddedFileCount}");
            check.Text(text, "embedded.txt");
            check.Properties(PdfFingerprint.FromFile(output), "with-attachment.summary.txt");
        }

        check.Finish();
    }
}
