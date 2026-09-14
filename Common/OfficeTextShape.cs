using System.Text;

namespace MuPDF.NET.Examples.Common
{
    /// <summary>
    /// Portable checks for CJK text that SmartOffice may fail to map to Unicode
    /// on some hosts (e.g. Windows HWPX → Arial/Liberation without Hangul glyphs,
    /// producing U+FFFD or NUL). Linux with Hangul-capable fonts extracts real
    /// syllables. Comparing character <em>shape</em> keeps baselines cross-platform.
    /// </summary>
    public static class OfficeTextShape
    {
        public static bool ContainsHangul(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            foreach (char c in text)
            {
                if (c >= '\uAC00' && c <= '\uD7A3')
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Map Hangul syllables, U+FFFD, and NUL to <c>H</c>; keep spaces/punctuation.
        /// </summary>
        public static string FromText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            var sb = new StringBuilder(text.Length);
            foreach (char c in text)
            {
                if ((c >= '\uAC00' && c <= '\uD7A3') || c == '\uFFFD' || c == '\0')
                    sb.Append('H');
                else
                    sb.Append(c);
            }
            return ResultCheck.NormalizeText(sb.ToString());
        }
    }
}
