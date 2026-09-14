using System;
using System.IO;

namespace MuPDF.NET.Examples.Common
{
    /// <summary>
    /// Resolves shared <c>Input/</c> and <c>Output/</c> folders for example projects.
    /// </summary>
    public static class ExamplePaths
    {
        static readonly Lazy<string> RootLazy = new(FindRoot);

        /// <summary>Solution root (folder that contains <c>Input/</c> and <c>Output/</c>).</summary>
        public static string Root => RootLazy.Value;

        public static string InputRoot => Path.Combine(Root, "Input");
        public static string OutputRoot => Path.Combine(Root, "Output");

        public static string MuPdfNetInput(string fileName) =>
            Require(Path.Combine(InputRoot, "MuPDF.NET", fileName));

        public static string Pdf4LlmInput(string fileName) =>
            Require(Path.Combine(InputRoot, "MuPDF.NET.PDF4LLM", fileName));

        public static string OfficeInput(string fileName) =>
            Require(Path.Combine(InputRoot, "MuPDF.NET.Office", fileName));

        /// <summary>
        /// Output path under <c>Output/{product}/{exampleName}/</c>. Creates the directory.
        /// </summary>
        public static string Output(string product, string exampleName, string fileName)
        {
            string dir = Path.Combine(OutputRoot, product, exampleName);
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, fileName);
        }

        /// <summary>Directory <c>{product}/{exampleName}/Expected/</c> under the solution root.</summary>
        public static string ExpectedDir(string product, string exampleName) =>
            Path.Combine(Root, product, exampleName, "Expected");

        public static string ExpectedFile(string product, string exampleName, string fileName) =>
            Path.Combine(ExpectedDir(product, exampleName), fileName);

        static string Require(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Example input not found: {path}", path);
            return path;
        }

        static string FindRoot()
        {
            string? dir = AppContext.BaseDirectory;
            for (int i = 0; i < 10 && !string.IsNullOrEmpty(dir); i++)
            {
                if (Directory.Exists(Path.Combine(dir, "Input"))
                    && Directory.Exists(Path.Combine(dir, "Output")))
                {
                    return dir;
                }
                dir = Directory.GetParent(dir)?.FullName;
            }

            throw new DirectoryNotFoundException(
                "Could not locate MuPDF.NET.Examples root (expected Input/ and Output/). " +
                "Run examples from the built project under this solution.");
        }
    }
}
