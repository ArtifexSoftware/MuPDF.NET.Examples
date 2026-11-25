using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.EncryptDecrypt;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 24-EncryptDecrypt");
        EncryptDecrypt();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void EncryptDecrypt()
    {
        string input = ExamplePaths.MuPdfNetInput("encrypt-decrypt.pdf");
        string encrypted = ExamplePaths.Output("MuPDF.NET", "24-EncryptDecrypt", "encrypted.pdf");
        string decrypted = ExamplePaths.Output("MuPDF.NET", "24-EncryptDecrypt", "decrypted.pdf");
        string originalPng = ExamplePaths.Output("MuPDF.NET", "24-EncryptDecrypt", "original.png");
        string decryptedPng = ExamplePaths.Output("MuPDF.NET", "24-EncryptDecrypt", "decrypted.png");
        var check = new ResultCheck("MuPDF.NET", "24-EncryptDecrypt");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        using (var doc = Document.Open(input))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(originalPng);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                EncryptDecrypt(input, encrypted, decrypted);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        using (var doc = Document.Open(decrypted))
        using (Page page = doc[0])
        using (Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2)))
            pix.Save(decryptedPng);

        check.Text(captured, "encrypt-decrypt.txt");
        // Encrypted PDFs are binary-unstable; fingerprint the decrypted output only.
        check.Properties(PdfFingerprint.FromFile(decrypted), "decrypted.summary.txt");
        check.FileSha256(originalPng, "original.png.sha256");
        check.FileSha256(decryptedPng, "decrypted.png.sha256");
        check.Finish();
    }

    // doc-start:EncryptDecrypt
    /// <summary>Save an AES-256 encrypted PDF, open it with a password, then save decrypted</summary>
    static void EncryptDecrypt(string inputPath, string encryptedPath, string decryptedPath)
    {
        const string userPassword = "user-secret";
        const string ownerPassword = "owner-secret";

        using (var doc = Document.Open(inputPath))
        {
            Console.WriteLine($"before.needsPass={doc.NeedsPass}");
            Console.WriteLine($"before.isEncrypted={doc.IsEncrypted}");
            doc.Save(
                encryptedPath,
                encryption: Constants.PDF_ENCRYPT_AES_256,
                ownerPW: ownerPassword,
                userPW: userPassword);
            Console.WriteLine("encrypted=ok");
        }

        using (var doc = Document.Open(encryptedPath))
        {
            Console.WriteLine($"needsPass={doc.NeedsPass}");
            Console.WriteLine($"isEncrypted={doc.IsEncrypted}");

            // Authenticate with the user password before reading / saving.
            int auth = doc.Authenticate(userPassword);
            Console.WriteLine($"authenticate={auth}");
            Console.WriteLine($"pages={doc.PageCount}");
            Console.WriteLine($"encryption={EncryptionName(doc)}");

            doc.Save(
                decryptedPath,
                encryption: Constants.PDF_ENCRYPT_NONE,
                garbage: 3,
                deflate: 1);
            Console.WriteLine("decrypted=ok");
        }

        using (var doc = Document.Open(decryptedPath))
        {
            Console.WriteLine($"after.needsPass={doc.NeedsPass}");
            Console.WriteLine($"after.isEncrypted={doc.IsEncrypted}");
            Console.WriteLine($"after.pages={doc.PageCount}");
            Console.WriteLine($"after.encryption={EncryptionName(doc)}");
        }

        static string EncryptionName(Document doc)
        {
            if (doc.Metadata != null && doc.Metadata.TryGetValue("encryption", out string? value)
                && !string.IsNullOrEmpty(value))
                return value;
            return "none";
        }
    }
    // doc-end:EncryptDecrypt
}
