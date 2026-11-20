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

    /// <summary>Apply a median filter pipeline to a pixmap and save the result</summary>
    static void ImageFilters()
    {
        string input = "apple.png";
        string output = "filtered.png";

        using var source = new Pixmap(input);
        int width = source.Width;
        int height = source.Height;

        var pipeline = new ImageFilterPipeline();
        // Light denoise — stable, docs-friendly filter.
        pipeline.AddMedian(3);

        // ApplyImageFilters may consume/dispose the source pixmap — do not use source afterward.
        using Pixmap filtered = Pixmap.ApplyImageFilters(source, pipeline);
        filtered.Save(output);
    }
}
