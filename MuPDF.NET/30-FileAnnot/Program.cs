using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.FileAnnot;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 30-FileAnnot");
        FileAnnot();
    }

    /// <summary>Attach a file as a page annotation (paperclip), distinct from EmbeddedFiles</summary>
    static void FileAnnot()
    {
        string blank = "Blank.pdf";
        string note = "note.txt";
        string output = "with-file-annot.pdf";

        byte[] payload = File.ReadAllBytes(note);

        using (var doc = Document.Open(blank))
        using (Page page = doc[0])
        {
            // Page-level file attachment annotation (vs Document.AddEmbeddedFile).
            Annot annot = page.AddFileAnnot(
                point: new Point(72, 72),
                buffer_: payload,
                filename: "note.txt",
                uFileName: "note.txt",
                desc: "Example file annotation");
            annot.Update();

            var fileInfo = annot.GetFileInfo();
            doc.Save(output, garbage: 3, deflate: 1);
        }

        using (var doc = Document.Open(output))
        using (Page page = doc[0])
        {
            var annots = page.Annots().ToList();
        }
    }
}
