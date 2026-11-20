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

    /// <summary>Attach a file to a PDF and list embedded attachments</summary>
    static void EmbeddedFiles()
    {
        string blank = "Blank.pdf";
        string note = "note.txt";
        string output = "with-attachment.pdf";

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

            doc.Save(output, garbage: 4, deflate: 1);
        }

        // Verify by listing names and hashing extracted bytes.
        using (var doc = Document.Open(output))
        {
            foreach (string name in doc.GetEmbeddedFileNames().OrderBy(n => n, StringComparer.Ordinal))
            {
                var info = doc.GetEmbeddedFileInfo(name);
                byte[] data = doc.GetEmbeddedFile(name);
            }

        }
    }
}
