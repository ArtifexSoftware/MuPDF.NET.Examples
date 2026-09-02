using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace MuPDF.NET.Examples.Common
{
    /// <summary>
    /// Portable checks for <c>MuPDFOffice.ToJson</c> output. Font names and
    /// glyph metrics differ by OS (e.g. Arial on Windows vs Liberation Serif on
    /// Linux), so baselines compare page size and extracted text only.
    /// </summary>
    public static class OfficeJsonFingerprint
    {
        public static Dictionary<string, string> FromJson(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            if (!root.TryGetProperty("pages", out JsonElement pages) ||
                pages.ValueKind != JsonValueKind.Array)
            {
                return new Dictionary<string, string>
                {
                    ["pageCount"] = "0",
                    ["pageWidth"] = "",
                    ["pageHeight"] = "",
                    ["texts"] = "",
                };
            }

            var texts = new StringBuilder();
            string width = "";
            string height = "";
            int pageIndex = 0;

            foreach (JsonElement page in pages.EnumerateArray())
            {
                if (string.IsNullOrEmpty(width) &&
                    page.TryGetProperty("width", out JsonElement w))
                    width = FormatNumber(w);

                if (string.IsNullOrEmpty(height) &&
                    page.TryGetProperty("height", out JsonElement h))
                    height = FormatNumber(h);

                if (!page.TryGetProperty("blocks", out JsonElement blocks) ||
                    blocks.ValueKind != JsonValueKind.Array)
                    continue;

                foreach (JsonElement block in blocks.EnumerateArray())
                {
                    if (!block.TryGetProperty("type", out JsonElement type) ||
                        !string.Equals(type.GetString(), "text", StringComparison.Ordinal))
                        continue;
                    if (!block.TryGetProperty("text", out JsonElement textEl))
                        continue;

                    if (texts.Length > 0)
                        texts.Append('|');
                    texts.Append(pageIndex + 1).Append(':').Append(textEl.GetString() ?? "");
                }

                pageIndex++;
            }

            return new Dictionary<string, string>
            {
                ["pageCount"] = pages.GetArrayLength().ToString(CultureInfo.InvariantCulture),
                ["pageWidth"] = width,
                ["pageHeight"] = height,
                ["texts"] = texts.ToString(),
            };
        }

        static string FormatNumber(JsonElement el)
        {
            if (el.ValueKind == JsonValueKind.Number && el.TryGetDouble(out double d))
                return d.ToString("0.0", CultureInfo.InvariantCulture);
            return el.ToString();
        }
    }
}
