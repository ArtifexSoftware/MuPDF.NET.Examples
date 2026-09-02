using System;
using System.Linq;
using System.Reflection;
using MuPDF.NET;

namespace MuPDF.NET.Examples.Common
{
    public static class ConsoleEx
    {
        public static void Title(string text)
        {
            PrintPackageVersions();
            Console.WriteLine();
            Console.WriteLine("=== " + text + " ===");
        }

        /// <summary>
        /// Print Artifex package versions used by this process.
        /// Always prints MuPDF.NET + MuPDF; also Office / PDF4LLM when referenced.
        /// </summary>
        public static void PrintPackageVersions()
        {
            var v = Constants.Version;
            Console.WriteLine($"MuPDF     {v.MuPdfVersion}");
            Console.WriteLine($"MuPDF.NET {v.MuPdfNetVersion}");

            string? office = TryGetPackageVersion("MuPDF.NET.Office");
            if (office != null)
                Console.WriteLine($"MuPDF.NET.Office {office}");

            string? pdf4llm = TryGetPackageVersion("MuPDF.NET.PDF4LLM");
            if (pdf4llm != null)
                Console.WriteLine($"MuPDF.NET.PDF4LLM {pdf4llm}");
        }

        public static void Info(string text) => Console.WriteLine(text);

        public static void Done(string? path = null)
        {
            if (!string.IsNullOrEmpty(path))
                Console.WriteLine("Wrote: " + path);
            Console.WriteLine("Done.");
        }

        static string? TryGetPackageVersion(string assemblyName)
        {
            try
            {
                Assembly? asm = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => string.Equals(
                        a.GetName().Name, assemblyName, StringComparison.OrdinalIgnoreCase));
                if (asm == null)
                    asm = Assembly.Load(assemblyName);

                string? informational = asm
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    ?.InformationalVersion;
                if (!string.IsNullOrWhiteSpace(informational))
                    return informational;

                return asm.GetName().Version?.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
