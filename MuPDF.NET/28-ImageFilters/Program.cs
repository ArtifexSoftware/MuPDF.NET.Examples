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

    /// <summary>
    /// Apply a median filter pipeline to a pixmap and save the result.
    /// </summary>
    static void ImageFilters()
    {
        string input = ExamplePaths.MuPdfNetInput("apple.png");
        string output = ExamplePaths.Output("MuPDF.NET", "28-ImageFilters", "filtered.png");
        var check = new ResultCheck("MuPDF.NET", "28-ImageFilters");

        using var source = new Pixmap(input);
        int width = source.Width;
        int height = source.Height;

        var pipeline = new ImageFilterPipeline();
        // Light denoise — stable, docs-friendly filter.
        pipeline.AddMedian(3);

        // ApplyImageFilters may consume/dispose the source pixmap — do not use source afterward.
        using Pixmap filtered = Pixmap.ApplyImageFilters(source, pipeline);
        filtered.Save(output);

        ConsoleEx.Info($"Filtered {width}x{height} → {output}");
        check.FileSha256(output, "filtered.png.sha256");
        check.Equal(filtered.Width, width, "width preserved");
        check.Equal(filtered.Height, height, "height preserved");
        check.Finish();
    }
}
