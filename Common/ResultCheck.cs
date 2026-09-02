using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MuPDF.NET.Examples.Common
{
    /// <summary>
    /// Compare example outputs to golden files under <c>{product}/{example}/Expected/</c>.
    /// Prints <c>PASS</c> / <c>FAIL</c>. Prefer <c>run-all.ps1</c> for batch runs (Office
    /// natives may AV during process teardown after a successful PASS).
    /// </summary>
    public sealed class ResultCheck
    {
        readonly string _product;
        readonly string _example;
        readonly List<string> _failures = new();
        int _checks;

        public ResultCheck(string product, string exampleName)
        {
            _product = product;
            _example = exampleName;
            Directory.CreateDirectory(ExamplePaths.ExpectedDir(product, exampleName));
        }

        public string ExpectedPath(string fileName) =>
            ExamplePaths.ExpectedFile(_product, _example, fileName);

        /// <summary>UTF-8 text compare (normalizes line endings to LF).</summary>
        public void Text(string actual, string expectedFileName)
        {
            _checks++;
            string path = ExpectedPath(expectedFileName);
            string normalized = NormalizeText(actual);

            if (ExampleArgs.UpdateExpected)
            {
                File.WriteAllText(path, normalized, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                ConsoleEx.Info($"Updated expected: {path}");
                return;
            }

            if (!File.Exists(path))
            {
                _failures.Add($"Missing expected file: {path} (run with --update-expected)");
                return;
            }

            string expected = NormalizeText(File.ReadAllText(path));
            if (!string.Equals(normalized, expected, StringComparison.Ordinal))
            {
                _failures.Add(
                    $"{expectedFileName}: text mismatch (actual {normalized.Length} chars, expected {expected.Length} chars)");
            }
        }

        /// <summary>SHA-256 of a binary file vs <c>{name}.sha256</c> contents.</summary>
        public void FileSha256(string actualFilePath, string expectedShaFileName)
        {
            _checks++;
            string sha = Sha256Hex(actualFilePath);
            string path = ExpectedPath(expectedShaFileName);

            if (ExampleArgs.UpdateExpected)
            {
                File.WriteAllText(path, sha + "\n", new UTF8Encoding(false));
                ConsoleEx.Info($"Updated expected: {path}");
                return;
            }

            if (!File.Exists(path))
            {
                _failures.Add($"Missing expected file: {path} (run with --update-expected)");
                return;
            }

            string expected = NormalizeText(File.ReadAllText(path)).Trim();
            if (!string.Equals(sha, expected, StringComparison.OrdinalIgnoreCase))
            {
                _failures.Add($"{expectedShaFileName}: SHA-256 mismatch\n  actual:   {sha}\n  expected: {expected}");
            }
        }

        /// <summary>Key=value lines (order-insensitive).</summary>
        public void Properties(IDictionary<string, string> actual, string expectedFileName)
        {
            var sb = new StringBuilder();
            foreach (var kv in new SortedDictionary<string, string>(actual, StringComparer.Ordinal))
                sb.Append(kv.Key).Append('=').Append(kv.Value).Append('\n');
            Text(sb.ToString(), expectedFileName);
        }

        public void Equal<T>(T actual, T expected, string label)
        {
            _checks++;
            if (ExampleArgs.UpdateExpected)
                return;

            if (!EqualityComparer<T>.Default.Equals(actual, expected))
                _failures.Add($"{label}: expected {expected}, got {actual}");
        }

        /// <summary>Print summary. Sets <see cref="Environment.ExitCode"/> to 0 or 1.</summary>
        public void Finish()
        {
            if (ExampleArgs.UpdateExpected)
            {
                ConsoleEx.Info($"Baselines updated for {_product}/{_example} ({_checks} file(s)).");
                ConsoleEx.Done();
                Environment.ExitCode = 0;
                return;
            }

            if (_failures.Count == 0)
            {
                ConsoleEx.Info($"PASS — {_checks} check(s) matched Expected/ for {_product}/{_example}");
                ConsoleEx.Done();
                Environment.ExitCode = 0;
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"FAIL — {_failures.Count} check(s) failed for {_product}/{_example}:");
            foreach (string f in _failures)
                Console.WriteLine("  - " + f);
            Console.WriteLine("Refresh baselines after intentional changes: dotnet run --project ... -- --update-expected");
            Environment.ExitCode = 1;
        }

        public static string NormalizeText(string text)
        {
            if (text == null)
                return "";
            return text.Replace("\r\n", "\n").Replace('\r', '\n');
        }

        public static string Sha256Hex(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            byte[] hash = SHA256.HashData(stream);
            var sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public static string Sha256HexBytes(byte[] data)
        {
            byte[] hash = SHA256.HashData(data);
            var sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
