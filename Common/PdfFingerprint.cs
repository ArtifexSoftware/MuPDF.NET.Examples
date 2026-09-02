using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MuPDF.NET;

namespace MuPDF.NET.Examples.Common
{
    /// <summary>
    /// Stable PDF fingerprints (page count + extracted text hash).
    /// Avoids brittle byte compares when MuPDF rewrites IDs/dates on Save.
    /// </summary>
    public static class PdfFingerprint
    {
        public static Dictionary<string, string> FromFile(string pdfPath)
        {
            using var doc = Document.Open(pdfPath);
            return FromDocument(doc);
        }

        public static Dictionary<string, string> FromDocument(Document doc)
        {
            var text = new StringBuilder();
            for (int i = 0; i < doc.PageCount; i++)
            {
                using Page page = doc[i];
                text.Append(page.GetText("text") ?? "");
                text.Append('\n');
            }

            string normalized = ResultCheck.NormalizeText(text.ToString());
            byte[] bytes = Encoding.UTF8.GetBytes(normalized);
            string hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

            return new Dictionary<string, string>
            {
                ["pageCount"] = doc.PageCount.ToString(),
                ["textSha256"] = hash,
            };
        }
    }
}
