using System.Linq;
using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;
using mupdf;

namespace MuPDF.NET.Examples.MuPDFNet.ColorManagement;

/// <summary>
/// Color / print samples ported from the Artifex ColorDemo project:
/// device colorspaces, ICC profiles, soft proofing, output intents,
/// DeviceN / spot separations, overprint, and rendering intents.
/// </summary>
internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 19-ColorManagement");
        ColorManagement();
    }

    // help:skip — calls the Demo* segments below.
    static void ColorManagement()
    {
        string inputPdf = ExamplePaths.MuPdfNetInput(Path.Combine("color", "test.pdf"));
        string displayProfile = ExamplePaths.MuPdfNetInput(Path.Combine("color", "NULL.icc"));
        string proofProfile = ExamplePaths.MuPdfNetInput(Path.Combine("color", "Proof.icc"));
        string outputDir = Path.GetDirectoryName(
            ExamplePaths.Output("MuPDF.NET", "19-ColorManagement", "_"))!;
        ConsoleEx.Info($"Input PDF:       {inputPdf}");
        ConsoleEx.Info($"Display profile: {displayProfile}");
        ConsoleEx.Info($"Proof profile:   {proofProfile}");
        ConsoleEx.Info($"Output folder:   {outputDir}");

        DemoPdfViewing(inputPdf, outputDir);
        DemoDeviceColorspaces(inputPdf, outputDir);
        DemoPdfObjectInspection(inputPdf);
        DemoZoomAndNavigation(inputPdf, outputDir);
        DemoIccProfiles(displayProfile, proofProfile);
        DemoColorManagedRendering(inputPdf, displayProfile, outputDir);
        DemoOutputIntent(inputPdf);
        DemoDeviceNSpotColorsAndSeparations(inputPdf, outputDir);
        DemoOverprint(inputPdf);
        DemoRenderingIntentAndColorParameters(inputPdf, displayProfile, proofProfile, outputDir);
        DemoPrepressAnalysis(inputPdf);
        DemoSoftProof(inputPdf, displayProfile, proofProfile, outputDir);
    }

    /// <summary>High-performance first-page rendering suitable for a viewer</summary>
    static void DemoPdfViewing(string inputPdf, string outputDir)
    {
        using var document = Document.Open(inputPdf);
        using var page = document[0];

        string output = Path.Combine(outputDir, "viewer-page-1-144dpi.png");
        using var pixmap = page.GetPixmap(dpi: 144, cs: Colorspace.Rgb, alpha: false);
        pixmap.Save(output);
    }

    /// <summary>DeviceRGB, DeviceGray, and DeviceCMYK output rendering</summary>
    static void DemoDeviceColorspaces(string inputPdf, string outputDir)
    {
        using var document = Document.Open(inputPdf);
        using var page = document[0];

        using (var rgb = page.GetPixmap(dpi: 96, cs: Colorspace.Rgb))
        {
            string path = Path.Combine(outputDir, "device-rgb.png");
            rgb.Save(path);
        }

        using (var gray = page.GetPixmap(dpi: 96, cs: Colorspace.Gray))
        {
            string path = Path.Combine(outputDir, "device-gray.png");
            gray.Save(path);
        }

        using (var cmyk = page.GetPixmap(dpi: 96, cs: Colorspace.Cmyk))
        {
            // PAM supports CMYK samples; PNG is RGB/Gray only.
            string path = Path.Combine(outputDir, "device-cmyk.pam");
            cmyk.Save(path);
        }
    }

    /// <summary>PDF catalog, trailer, xref, dictionary, and stream inspection</summary>
    static void DemoPdfObjectInspection(string inputPdf)
    {
        using var document = Document.Open(inputPdf);
        if (!document.IsPdf)
        {
            return;
        }

        var outputIntents = document.XrefGetKey(document.PdfCatalog, "OutputIntents");

        int objectCount = Math.Max(0, Math.Min(8, document.XrefLength - 1));
        for (int xref = 1; xref <= objectCount; xref++)
        {
            var type = document.XrefGetKey(xref, "Type");
            var subtype = document.XrefGetKey(xref, "Subtype");
        }
    }

    /// <summary>Matrix zoom, page iteration, links, and outline navigation</summary>
    static void DemoZoomAndNavigation(string inputPdf, string outputDir)
    {
        using var document = Document.Open(inputPdf);
        using var page = document[0];
        using var zoomed = page.GetPixmap(matrix: new Matrix(2, 2), cs: Colorspace.Rgb);
        string output = Path.Combine(outputDir, "zoomed-page-1.png");
        zoomed.Save(output);
    }

    /// <summary>Create MuPDF ICC colorspaces from display/proof profile files</summary>
    static void DemoIccProfiles(string displayProfile, string proofProfile)
    {
        using (var display = LoadIccColorspace(displayProfile))
            AppendNativeColorspace(new StringBuilder(), "display", display);

        using (var proof = LoadIccColorspace(proofProfile))
            AppendNativeColorspace(new StringBuilder(), "proof", proof);
    }

    /// <summary>Render page 1 directly into a display ICC colorspace</summary>
    static void DemoColorManagedRendering(string inputPdf, string displayProfile, string outputDir)
    {
        string output = Path.Combine(outputDir, "display-profile-render.png");
        RenderIccPage(inputPdf, displayProfile, proofProfile: null, output);
    }

    /// <summary>Read the PDF Output Intent semantically through MuPDF</summary>
    static void DemoOutputIntent(string inputPdf)
    {
        using var document = new FzDocument(inputPdf);
        using var outputIntent = document.fz_document_output_intent();

        if (outputIntent == null || outputIntent.m_internal_value() == 0)
        {
            return;
        }

        AppendNativeColorspace(new StringBuilder(), "outputIntent", outputIntent);
    }

    /// <summary>Enumerate DeviceN/spot plates and render all plates composited to RGB</summary>
    static void DemoDeviceNSpotColorsAndSeparations(string inputPdf, string outputDir)
    {
        using var document = new FzDocument(inputPdf);
        using var page = document.fz_load_page(0);
        using var separations = page.fz_page_separations();

        if (separations == null || separations.m_internal_value() == 0)
        {
            return;
        }

        int count = separations.fz_count_separations();
        for (int i = 0; i < count; i++)
        {
            separations.fz_set_separation_behavior(i, fz_separation_behavior.FZ_SEPARATION_COMPOSITE);
        }

        using var matrix = new FzMatrix(1, 0, 0, 1, 0, 0);
        using var rgb = new FzColorspace(FzColorspace.Fixed.Fixed_RGB);
        using var pixmap = page.fz_new_pixmap_from_page_with_separations(matrix, rgb, separations, alpha: 0);
        string output = Path.Combine(outputDir, "separations-composited.png");
        pixmap.fz_save_pixmap_as_png(output);
    }

    /// <summary>Detect overprint use and show the parameters used for simulation</summary>
    static void DemoOverprint(string inputPdf)
    {
        using var document = new FzDocument(inputPdf);
        using var page = document.fz_load_page(0);
        bool usesOverprint = page.fz_page_uses_overprint() != 0;

        using var parameters = new FzColorParams
        {
            op = 1,
            opm = 1,
            bp = 1,
            ri = 1, // FZ_RI_RELATIVE_COLORIMETRIC
        };
    }

    /// <summary>Apply color parameters to a CMYK-to-display conversion and save comparable images</summary>
    static void DemoRenderingIntentAndColorParameters(string inputPdf, string displayProfile, string proofProfile, string outputDir)
    {
        using var document = new FzDocument(inputPdf);
        using var page = document.fz_load_page(0);
        using var matrix = new FzMatrix(1, 0, 0, 1, 0, 0);
        using var cmyk = new FzColorspace(FzColorspace.Fixed.Fixed_CMYK);
        using var display = LoadIccColorspace(displayProfile);
        using var proof = LoadIccColorspace(proofProfile);
        using var defaults = new FzDefaultColorspaces();
        using var source = page.fz_new_pixmap_from_page(matrix, cmyk, alpha: 0);

        using var perceptualParameters = new FzColorParams
        {
            ri = 0, // FZ_RI_PERCEPTUAL
            bp = 0,
            op = 0,
            opm = 0,
        };
        using var perceptual = source.fz_convert_pixmap(
            display, proof, defaults, perceptualParameters, keep_alpha: 0);
        string perceptualOutput = Path.Combine(outputDir, "rendering-intent-perceptual.png");
        perceptual.fz_save_pixmap_as_png(perceptualOutput);

        using var relativeParameters = new FzColorParams
        {
            ri = 1, // FZ_RI_RELATIVE_COLORIMETRIC
            bp = 1,
            op = 1,
            opm = 1,
        };
        using var relative = source.fz_convert_pixmap(
            display, proof, defaults, relativeParameters, keep_alpha: 0);
        string relativeOutput = Path.Combine(outputDir, "rendering-intent-relative-bpc-overprint.png");
        relative.fz_save_pixmap_as_png(relativeOutput);
    }

    /// <summary>Show the primitives available for future prepress checks</summary>
    static void DemoPrepressAnalysis(string inputPdf)
    {
        using var document = new FzDocument(inputPdf);
        using var outputIntent = document.fz_document_output_intent();
        bool hasOutputIntent = outputIntent != null && outputIntent.m_internal_value() != 0;
        int pagesWithOverprint = 0;
        int pagesWithSeparations = 0;
        int pageCount = document.fz_count_pages();

        for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
        {
            using var page = document.fz_load_page(pageNumber);
            if (page.fz_page_uses_overprint() != 0)
                pagesWithOverprint++;

            using var separations = page.fz_page_separations();
            if (separations != null
                && separations.m_internal_value() != 0
                && separations.fz_count_separations() > 0)
            {
                pagesWithSeparations++;
            }
        }
    }

    /// <summary>Render page 1 through an optional proof profile into an optional display profile</summary>
    static void DemoSoftProof(string inputPdf, string displayProfile, string proofProfile, string outputDir)
    {
        // ICC enable/disable affects MuPDF's shared context. A production wrapper
        // should serialize this setting with all rendering operations.
        mupdf.mupdf.fz_enable_icc();

        string output = Path.Combine(outputDir, "soft-proof.png");
        RenderIccPage(inputPdf, displayProfile, proofProfile, output);
    }

    // ── helpers (also useful to copy) ─────────────────────────────────

    static void RenderIccPage(
        string inputPdf,
        string displayProfile,
        string? proofProfile,
        string output)
    {
        using var document = new FzDocument(inputPdf);
        using var page = document.fz_load_page(0);
        using var display = LoadIccColorspace(displayProfile);
        using var proof = proofProfile == null ? null : LoadIccColorspace(proofProfile);
        using var pageBounds = page.fz_bound_page();
        using var transform = new FzMatrix(2, 0, 0, 2, 0, 0);
        using var transformedBounds = new FzRect(pageBounds, transform);
        using var pixelBounds = new FzIrect(transformedBounds);
        using var noSeparations = new FzSeparations(0);
        using var pixmap = display.fz_new_pixmap_with_bbox(pixelBounds, noSeparations, alpha: 0);
        pixmap.fz_clear_pixmap_with_value(255);

        // Draw device converts document colors into the display ICC space,
        // optionally applying the proof ICC profile as the intermediate proof space.
        using var device = proof == null
            ? new FzDevice(transform, pixmap, pixelBounds)
            : new FzDevice(transform, pixmap, pixelBounds, proof);
        using var identity = new FzMatrix(1, 0, 0, 1, 0, 0);
        using var cookie = new FzCookie();
        page.fz_run_page(device, identity, cookie);

        pixmap.fz_save_pixmap_as_png(output);
    }

    static FzColorspace LoadIccColorspace(string profilePath)
    {
        // FZ_COLORSPACE_NONE (0) asks MuPDF to infer the profile type.
        using var profile = new FzBuffer(profilePath);
        return new FzColorspace(
            (SWIGTYPE_fz_colorspace_type)0,
            flags: 0,
            name: Path.GetFileName(profilePath),
            profile);
    }

    static void AppendNativeColorspace(StringBuilder report, string label, FzColorspace colorspace)
    {
        report.Append(label)
            .Append(" name=").Append(colorspace.fz_colorspace_name())
            .Append(" n=").Append(colorspace.fz_colorspace_n())
            .Append(" icc=").Append(colorspace.fz_colorspace_is_icc() != 0)
            .Append(" deviceN=").Append(colorspace.fz_colorspace_is_device_n() != 0)
            .Append('\n');
        for (int i = 0; i < colorspace.fz_colorspace_n(); i++)
            report.Append(label).Append(".c").Append(i).Append('=').Append(colorspace.fz_colorspace_colorant(i)).Append('\n');
    }

    static void Section(string title, StringBuilder report)
    {
        report.Append("--- ").Append(title).Append(" ---\n");
        ConsoleEx.Info(title);
    }
}
