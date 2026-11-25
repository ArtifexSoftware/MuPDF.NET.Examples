using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.ImageFilters;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 28-ImageFilters");
        ImageFilters();
    }

    // help:skip — resolves fixtures, calls the documented overload, then checks the harness baseline.
    static void ImageFilters()
    {
        string input = ExamplePaths.MuPdfNetInput("image-filters.png");
        string output = ExamplePaths.Output("MuPDF.NET", "28-ImageFilters", "filtered.png");
        string sourcePng = ExamplePaths.Output("MuPDF.NET", "28-ImageFilters", "source.png");
        var check = new ResultCheck("MuPDF.NET", "28-ImageFilters");
        var sb = new StringBuilder();

        ConsoleEx.Info($"Opened: {input}");
        File.Copy(input, sourcePng, overwrite: true);

        var previous = Console.Out;
        using (var capture = new StringWriter(sb))
        {
            Console.SetOut(capture);
            try
            {
                ImageFilters(input, output);
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        string captured = ResultCheck.NormalizeText(sb.ToString());
        Console.Write(captured);

        check.Text(captured, "image-filters.txt");
        check.FileSha256(sourcePng, "source.png.sha256");
        check.FileSha256(output, "filtered.png.sha256");
        check.Finish();
    }

    // doc-start:ImageFilters
    /// <summary>Apply a median filter pipeline to a pixmap and save the result</summary>
    static void ImageFilters(string inputPath, string outputPath)
    {
        using var source = new Pixmap(inputPath);
        int width = source.Width;
        int height = source.Height;

        var pipeline = new ImageFilterPipeline();
        // Light denoise — stable, docs-friendly filter.
        pipeline.AddMedian(3);

        // ApplyImageFilters may consume/dispose the source pixmap — do not use source afterward.
        using Pixmap filtered = Pixmap.ApplyImageFilters(source, pipeline);
        Console.WriteLine($"width={width}");
        Console.WriteLine($"height={height}");
        Console.WriteLine($"n={filtered.N}");
        Console.WriteLine($"filterCount={pipeline.Filters.Count}");
        Console.WriteLine($"filter={pipeline.Filters[0].Type}");
        filtered.Save(outputPath);
    }
    // doc-end:ImageFilters
}
