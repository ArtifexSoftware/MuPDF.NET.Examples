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

    /// <summary>
    /// Save an AES-256 encrypted PDF, open it with a password, then save decrypted.
    /// </summary>
    static void EncryptDecrypt()
    {
        string input = ExamplePaths.MuPdfNetInput("sample.pdf");
        string encrypted = ExamplePaths.Output("MuPDF.NET", "24-EncryptDecrypt", "encrypted.pdf");
        string decrypted = ExamplePaths.Output("MuPDF.NET", "24-EncryptDecrypt", "decrypted.pdf");
        var check = new ResultCheck("MuPDF.NET", "24-EncryptDecrypt");
        var report = new StringBuilder();
        const string userPassword = "user-secret";
        const string ownerPassword = "owner-secret";

        using (var doc = Document.Open(input))
        {
            doc.Save(
                encrypted,
                encryption: Constants.PDF_ENCRYPT_AES_256,
                ownerPW: ownerPassword,
                userPW: userPassword);
            report.Append("encrypted=ok\n");
        }

        using (var doc = Document.Open(encrypted))
        {
            report.Append("needsPass=").Append(doc.NeedsPass).Append('\n');
            report.Append("isEncrypted=").Append(doc.IsEncrypted).Append('\n');

            // Authenticate with the user password before reading / saving.
            int auth = doc.Authenticate(userPassword);
            report.Append("authenticate=").Append(auth).Append('\n');
            report.Append("pages=").Append(doc.PageCount).Append('\n');

            doc.Save(
                decrypted,
                encryption: Constants.PDF_ENCRYPT_NONE,
                garbage: 3,
                deflate: 1);
            report.Append("decrypted=ok\n");
        }

        using (var doc = Document.Open(decrypted))
        {
            report.Append("decrypted.needsPass=").Append(doc.NeedsPass).Append('\n');
            report.Append("decrypted.isEncrypted=").Append(doc.IsEncrypted).Append('\n');
            report.Append("decrypted.pages=").Append(doc.PageCount).Append('\n');
        }

        check.Text(report.ToString(), "encrypt-decrypt.txt");
        // Encrypted PDFs are binary-unstable; fingerprint the decrypted output only.
        check.Properties(PdfFingerprint.FromFile(decrypted), "decrypted.summary.txt");
        check.Finish();
    }
}
