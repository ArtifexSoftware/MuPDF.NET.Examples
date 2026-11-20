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

    /// <summary>Save an AES-256 encrypted PDF, open it with a password, then save decrypted</summary>
    static void EncryptDecrypt()
    {
        string input = "sample.pdf";
        string encrypted = "encrypted.pdf";
        string decrypted = "decrypted.pdf";
        const string userPassword = "user-secret";
        const string ownerPassword = "owner-secret";

        using (var doc = Document.Open(input))
        {
            doc.Save(
                encrypted,
                encryption: Constants.PDF_ENCRYPT_AES_256,
                ownerPW: ownerPassword,
                userPW: userPassword);
        }

        using (var doc = Document.Open(encrypted))
        {

            // Authenticate with the user password before reading / saving.
            int auth = doc.Authenticate(userPassword);

            doc.Save(
                decrypted,
                encryption: Constants.PDF_ENCRYPT_NONE,
                garbage: 3,
                deflate: 1);
        }

        using (var doc = Document.Open(decrypted))
        {
        }

        // Encrypted PDFs are binary-unstable; fingerprint the decrypted output only.
    }
}
