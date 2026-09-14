using System;

namespace MuPDF.NET.Examples.Common
{
    /// <summary>
    /// Office license helpers. Prefer <c>MUPDF_OFFICE_KEY</c>; empty = restricted mode.
    /// </summary>
    public static class OfficeLicense
    {
        public static string? KeyFromEnvironment()
        {
            string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY");
            return string.IsNullOrWhiteSpace(key) ? null : key.Trim();
        }
    }
}
