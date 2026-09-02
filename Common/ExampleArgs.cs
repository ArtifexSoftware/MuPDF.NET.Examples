using System;

namespace MuPDF.NET.Examples.Common
{
    /// <summary>Command-line flags shared by all examples.</summary>
    public static class ExampleArgs
    {
        /// <summary>
        /// When true, write current results into <c>Expected/</c> instead of comparing.
        /// Pass <c>--update-expected</c> after a trusted NuGet upgrade to refresh baselines.
        /// </summary>
        public static bool UpdateExpected { get; private set; }

        public static void Parse(string[] args)
        {
            foreach (string a in args)
            {
                if (string.Equals(a, "--update-expected", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(a, "-u", StringComparison.OrdinalIgnoreCase))
                {
                    UpdateExpected = true;
                }
                else if (string.Equals(a, "--help", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(a, "-h", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Options:");
                    Console.WriteLine("  --update-expected, -u   Refresh Expected/ baselines from this run");
                    Console.WriteLine("  --help, -h              Show help");
                    Environment.Exit(0);
                }
            }
        }
    }
}
