using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.ParallelPixmap;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 31-ParallelPixmap");
        ParallelPixmap();
    }

    /// <summary>
    /// Render independent PDFs in parallel. Each worker opens its own Document.
    /// </summary>
    static void ParallelPixmap()
    {
        string[] files =
        {
            ExamplePaths.MuPdfNetInput("sample.pdf"),
            ExamplePaths.MuPdfNetInput("Blank.pdf"),
            ExamplePaths.MuPdfNetInput("Color.pdf"),
        };
        var check = new ResultCheck("MuPDF.NET", "31-ParallelPixmap");

        // Cap workers to CPU count. Unbounded Task.Run can spike RAM on large pages.
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount)
        };

        ConsoleEx.Info($"Rendering {files.Length} files, MaxDegreeOfParallelism={options.MaxDegreeOfParallelism}");

        Parallel.ForEach(files, options, file =>
        {
            // One Document per worker. Do not share a Document across threads.
            using var doc = Document.Open(file);
            using Page page = doc[0];
            using Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2));

            string output = ExamplePaths.Output(
                "MuPDF.NET", "31-ParallelPixmap", Path.GetFileNameWithoutExtension(file) + ".png");
            pix.Save(output);
        });

        foreach (string file in files)
        {
            string stem = Path.GetFileNameWithoutExtension(file);
            string output = ExamplePaths.Output("MuPDF.NET", "31-ParallelPixmap", stem + ".png");
            check.FileSha256(output, stem + ".png.sha256");
        }

        check.Finish();
    }
}
