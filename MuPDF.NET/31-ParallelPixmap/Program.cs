using System.Text;
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

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ParallelPixmap()
    {
        string[] files =
        {
            ExamplePaths.MuPdfNetInput("parallel-a.pdf"),
            ExamplePaths.MuPdfNetInput("parallel-b.pdf"),
            ExamplePaths.MuPdfNetInput("parallel-c.pdf"),
        };
        string outputDir = Path.GetDirectoryName(
            ExamplePaths.Output("MuPDF.NET", "31-ParallelPixmap", "parallel-a.png"))!;
        var check = new ResultCheck("MuPDF.NET", "31-ParallelPixmap");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened {files.Length} files");

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                ParallelPixmap(files, outputDir);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        check.Text(captured, "parallel.txt");
        foreach (string file in files)
        {
            string stem = Path.GetFileNameWithoutExtension(file);
            check.FileSha256(Path.Combine(outputDir, stem + ".png"), stem + ".png.sha256");
        }
        check.Finish();
    }

    // doc-start:ParallelPixmap
    /// <summary>Render independent PDFs in parallel. Each worker opens its own Document</summary>
    static void ParallelPixmap(string[] inputPaths, string outputDir)
    {
        Directory.CreateDirectory(outputDir);

        // Cap workers to CPU count. Unbounded Task.Run can spike RAM on large pages.
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount)
        };

        Parallel.ForEach(inputPaths, options, inputPath =>
        {
            // One Document per worker. Do not share a Document across threads.
            using var doc = Document.Open(inputPath);
            using Page page = doc[0];
            using Pixmap pix = page.GetPixmap(matrix: new Matrix(2, 2));
            string outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".png");
            pix.Save(outputPath);
        });

        Console.WriteLine($"fileCount={inputPaths.Length}");
        foreach (string inputPath in inputPaths)
        {
            string stem = Path.GetFileNameWithoutExtension(inputPath);
            string outputPath = Path.Combine(outputDir, stem + ".png");
            using var pix = new Pixmap(outputPath);
            Console.WriteLine($"{stem} width={pix.Width} height={pix.Height} n={pix.N}");
        }
    }
    // doc-end:ParallelPixmap
}
