using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.Barcodes;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 12-Barcodes");
        WriteAndReadBarcode();
    }

    /// <summary>Write a QR code to a PDF, then read it back</summary>
    static void WriteAndReadBarcode()
    {
        const string payload = "MuPDF.NET.Examples";
        string output = "qr.pdf";

        // Create a small page and draw a QR into the given rectangle.
        using (var doc = Document.Open())
        {
            using Page page = doc.NewPage(width: 300, height: 300);
            var rect = new Rect(40, 40, 260, 260);
            page.WriteBarcode(rect, payload, BarcodeFormat.QR, forceFitToRect: true, pureBarcode: true);
            doc.Save(output);
        }

        // Re-open and decode barcodes on the page.
        using (var doc = Document.Open(output))
        using (Page page = doc[0])
        {
            List<Barcode> found = page.ReadBarcodes().ToList();
            foreach (Barcode b in found.OrderBy(b => b.Text ?? "", StringComparer.Ordinal))
            {
            }

        }
    }
}
