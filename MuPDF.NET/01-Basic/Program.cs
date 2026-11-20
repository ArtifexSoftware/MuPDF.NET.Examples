using System.Text;
using MuPDF.NET;
using MuPDF.NET.Examples.Common;

namespace MuPDF.NET.Examples.MuPDFNet.Basic;

internal static class Program
{
    static void Main(string[] args)
    {
        ExampleArgs.Parse(args);
        ConsoleEx.Title("MuPDF.NET / 01-Basic");
        OpenSave();
    }

    // help:skip — calls the feature segments below, then checks the harness baseline.
    static void OpenSave()
    {
        string input = ExamplePaths.MuPdfNetInput("sample.pdf");
        string output = ExamplePaths.Output("MuPDF.NET", "01-Basic", "sample-copy.pdf");
        string infoPath = ExamplePaths.Output("MuPDF.NET", "01-Basic", "document-info.txt");
        var check = new ResultCheck("MuPDF.NET", "01-Basic");
        var sb = new StringBuilder();

        using (var doc = Document.Open(input))
        {
            ConsoleEx.Info($"Opened: {input}");
            var previous = Console.Out;
            using var capture = new StringWriter(sb);
            Console.SetOut(capture);
            try
            {
                DocumentIdentity(doc);
                Encryption(doc);
                DocumentFlags(doc);
                PdfStructure(doc);
                ViewerSettings(doc);
                Permissions(doc);
                FormFonts(doc);
                EmbeddedNames(doc);
                AnnotsAndLinks(doc);
                MarkInfo(doc);
                ReadMetadata(doc);
                XmlMetadata(doc);
                PageLabels(doc);
                TableOfContents(doc);
                NamedDestinations(doc);
                OptionalContent(doc);
                Trailer(doc);
                PageProperties(doc);
            }
            finally
            {
                Console.SetOut(previous);
            }

            SaveCopy(doc, output);
            File.WriteAllText(infoPath, sb.ToString());
        }

        Console.Write(sb.ToString());

        check.Text(sb.ToString(), "document-info.txt");
        check.Properties(PdfFingerprint.FromFile(output), "sample-copy.summary.txt");
        check.Finish();
    }

    // doc-start:DocumentIdentity
    /// <summary>Open a PDF and read its page count, kind, and file name</summary>
    static void DocumentIdentity(Document doc)
    {
        Console.WriteLine($"doc.PageCount = {doc.PageCount}");
        Console.WriteLine($"doc.IsPdf = {doc.IsPdf}");
    }
    // doc-end:DocumentIdentity

    // doc-start:Encryption
    /// <summary>See whether the PDF is encrypted or still needs a password</summary>
    static void Encryption(Document doc)
    {
        Console.WriteLine($"doc.IsEncrypted = {doc.IsEncrypted}");
        Console.WriteLine($"doc.NeedsPass = {doc.NeedsPass}");
    }
    // doc-end:Encryption

    // doc-start:DocumentFlags
    /// <summary>Read document state flags</summary>
    static void DocumentFlags(Document doc)
    {
        Console.WriteLine($"doc.IsReflowable = {doc.IsReflowable}");
        Console.WriteLine($"doc.IsDirty = {doc.IsDirty}");
        Console.WriteLine($"doc.IsFormPdf = {doc.IsFormPdf}");
        Console.WriteLine($"doc.IsRepaired = {doc.IsRepaired}");
        Console.WriteLine($"doc.IsFastWebaccess = {doc.IsFastWebaccess}");
        Console.WriteLine($"doc.IsClosed = {doc.IsClosed}");
        Console.WriteLine($"doc.CanSaveIncrementally() = {doc.CanSaveIncrementally()}");
        Console.WriteLine($"doc.JournalIsEnabled = {doc.JournalIsEnabled}");
    }
    // doc-end:DocumentFlags

    // doc-start:PdfStructure
    /// <summary>Read PDF structure: chapters, versions, xref length, and catalog</summary>
    static void PdfStructure(Document doc)
    {
        Console.WriteLine($"doc.ChapterCount = {doc.ChapterCount}");
        for (int c = 0; c < doc.ChapterCount; c++)
            Console.WriteLine($"doc.ChapterPageCount({c}) = {doc.ChapterPageCount(c)}");
        Console.WriteLine($"doc.VersionCount = {doc.VersionCount}");
        Console.WriteLine($"doc.XrefLength = {doc.XrefLength}");
        Console.WriteLine($"doc.PdfCatalog = {doc.PdfCatalog}");
    }
    // doc-end:PdfStructure

    // doc-start:ViewerSettings
    /// <summary>Read catalog viewer settings</summary>
    static void ViewerSettings(Document doc)
    {
        Console.WriteLine($"doc.Language = {doc.Language}");
        Console.WriteLine($"doc.PageLayout = {doc.PageLayout}");
        Console.WriteLine($"doc.PageMode = {doc.PageMode}");
        Console.WriteLine($"doc.NeedAppearances = {doc.NeedAppearances}");
        Console.WriteLine($"doc.GetSigFlags() = {doc.GetSigFlags()}");
    }
    // doc-end:ViewerSettings

    // doc-start:Permissions
    /// <summary>Read the permission bitmask</summary>
    static void Permissions(Document doc)
    {
        int perm = doc.Permissions;
        Console.WriteLine($"doc.Permissions = {perm}");
        Console.WriteLine($"Constants.PDF_PERM_PRINT = {(perm & Constants.PDF_PERM_PRINT) != 0}");
        Console.WriteLine($"Constants.PDF_PERM_MODIFY = {(perm & Constants.PDF_PERM_MODIFY) != 0}");
        Console.WriteLine($"Constants.PDF_PERM_COPY = {(perm & Constants.PDF_PERM_COPY) != 0}");
        Console.WriteLine($"Constants.PDF_PERM_ANNOTATE = {(perm & Constants.PDF_PERM_ANNOTATE) != 0}");
        Console.WriteLine($"Constants.PDF_PERM_FORM = {(perm & Constants.PDF_PERM_FORM) != 0}");
        Console.WriteLine($"Constants.PDF_PERM_ACCESSIBILITY = {(perm & Constants.PDF_PERM_ACCESSIBILITY) != 0}");
        Console.WriteLine($"Constants.PDF_PERM_ASSEMBLE = {(perm & Constants.PDF_PERM_ASSEMBLE) != 0}");
        Console.WriteLine($"Constants.PDF_PERM_PRINT_HQ = {(perm & Constants.PDF_PERM_PRINT_HQ) != 0}");
    }
    // doc-end:Permissions

    // doc-start:FormFonts
    /// <summary>List AcroForm font names</summary>
    static void FormFonts(Document doc)
    {
        List<string> formFonts = doc.FormFonts;
        Console.WriteLine($"doc.FormFonts = {(formFonts == null ? "(null)" : formFonts.Count + " item(s)")}");
        if (formFonts != null)
        {
            foreach (string name in formFonts)
                Console.WriteLine($"  {name}");
        }
    }
    // doc-end:FormFonts

    // doc-start:EmbeddedNames
    /// <summary>List embedded file names</summary>
    static void EmbeddedNames(Document doc)
    {
        Console.WriteLine($"doc.EmbeddedFileCount = {doc.EmbeddedFileCount}");
        foreach (string name in doc.GetEmbeddedFileNames())
            Console.WriteLine($"doc.GetEmbeddedFileNames(): {name}");
    }
    // doc-end:EmbeddedNames

    // doc-start:AnnotsAndLinks
    /// <summary>Check whether the PDF has any annotations or links</summary>
    static void AnnotsAndLinks(Document doc)
    {
        Console.WriteLine($"doc.HasAnnots() = {doc.HasAnnots()}");
        Console.WriteLine($"doc.HasLinks() = {doc.HasLinks()}");
    }
    // doc-end:AnnotsAndLinks

    // doc-start:MarkInfo
    /// <summary>Read MarkInfo flags from the catalog</summary>
    static void MarkInfo(Document doc)
    {
        Console.WriteLine($"doc.MarkInfo.Count = {doc.MarkInfo.Count}");
        foreach (var kv in doc.MarkInfo)
            Console.WriteLine($"doc.MarkInfo[\"{kv.Key}\"] = {kv.Value}");
    }
    // doc-end:MarkInfo

    // doc-start:ReadMetadata
    /// <summary>Read the standard PDF Info dictionary</summary>
    static void ReadMetadata(Document doc)
    {
        Dictionary<string, string> meta = doc.Metadata;
        Console.WriteLine($"doc.Metadata.Count = {meta.Count}");
        foreach (var kv in meta)
            Console.WriteLine($"doc.Metadata[\"{kv.Key}\"] = {kv.Value}");
    }
    // doc-end:ReadMetadata

    // doc-start:XmlMetadata
    /// <summary>Read the XMP metadata stream</summary>
    static void XmlMetadata(Document doc)
    {
        Console.WriteLine($"doc.XrefXmlMetadata = {doc.XrefXmlMetadata}");
        string xml = doc.GetXmlMetadata();
        Console.WriteLine($"doc.GetXmlMetadata() = {xml}");
    }
    // doc-end:XmlMetadata

    // doc-start:PageLabels
    /// <summary>Read page label rules</summary>
    static void PageLabels(Document doc)
    {
        List<Dictionary<string, object>> labels = doc.GetPageLabels();
        Console.WriteLine($"doc.GetPageLabels().Count = {labels.Count}");
        foreach (Dictionary<string, object> rule in labels)
        {
            foreach (var kv in rule)
                Console.WriteLine($"  {kv.Key} = {kv.Value}");
        }
    }
    // doc-end:PageLabels

    // doc-start:TableOfContents
    /// <summary>Read the table of contents</summary>
    static void TableOfContents(Document doc)
    {
        TocResult toc = doc.GetToc(simple: true);
        Console.WriteLine($"doc.GetToc().Count = {toc.Count}");
        foreach (var item in toc)
            Console.WriteLine($"  level={item.level} title={item.title} page={item.page}");
    }
    // doc-end:TableOfContents

    // doc-start:NamedDestinations
    /// <summary>Resolve named destinations</summary>
    static void NamedDestinations(Document doc)
    {
        Dictionary<string, Dictionary<string, object>> names = doc.ResolveNames();
        Console.WriteLine($"doc.ResolveNames().Count = {names.Count}");
        foreach (var dest in names)
        {
            Console.WriteLine($"  {dest.Key}");
            foreach (var kv in dest.Value)
                Console.WriteLine($"    {kv.Key} = {kv.Value}");
        }
    }
    // doc-end:NamedDestinations

    // doc-start:OptionalContent
    /// <summary>Read optional content groups and layer UI</summary>
    static void OptionalContent(Document doc)
    {
        Dictionary<int, Dictionary<string, object>> ocgs = doc.GetOcgs();
        Console.WriteLine($"doc.GetOcgs().Count = {ocgs.Count}");
        foreach (var ocg in ocgs)
        {
            Console.WriteLine($"  xref={ocg.Key}");
            foreach (var kv in ocg.Value)
                Console.WriteLine($"    {kv.Key} = {kv.Value}");
        }
        List<Dictionary<string, object>> layers = doc.GetLayers();
        Console.WriteLine($"doc.GetLayers().Count = {layers.Count}");
        foreach (Dictionary<string, object> layer in layers)
        {
            foreach (var kv in layer)
                Console.WriteLine($"  {kv.Key} = {kv.Value}");
        }
        List<Dictionary<string, object>> layerUi = doc.LayerUiConfigs();
        Console.WriteLine($"doc.LayerUiConfigs().Count = {layerUi.Count}");
        foreach (Dictionary<string, object> ui in layerUi)
        {
            foreach (var kv in ui)
                Console.WriteLine($"  {kv.Key} = {kv.Value}");
        }
    }
    // doc-end:OptionalContent

    // doc-start:Trailer
    /// <summary>Read the PDF trailer dictionary</summary>
    static void Trailer(Document doc)
    {
        Console.WriteLine($"doc.PdfTrailer() = {doc.PdfTrailer()}");
    }
    // doc-end:Trailer

    // doc-start:SaveCopy
    /// <summary>Write a full copy of the open PDF</summary>
    static void SaveCopy(Document doc, string output)
    {
        doc.Save(output);
    }
    // doc-end:SaveCopy

    // doc-start:PageProperties
    /// <summary>Read each page's boxes, fonts, images, XObjects, and annotation xrefs</summary>
    static void PageProperties(Document doc)
    {
        for (int i = 0; i < doc.PageCount; i++)
        {
            using Page page = doc[i];
            Console.WriteLine($"doc[{i}].GetLabel() = {page.GetLabel()}");
            Console.WriteLine($"doc[{i}].Xref = {page.Xref}");
            Console.WriteLine($"doc[{i}].Rotation = {page.Rotation}");
            Console.WriteLine($"doc[{i}].Width = {page.Width}");
            Console.WriteLine($"doc[{i}].Height = {page.Height}");
            Console.WriteLine($"doc[{i}].Rect = {page.Rect}");
            Console.WriteLine($"doc[{i}].MediaBox = {page.MediaBox}");
            Console.WriteLine($"doc[{i}].CropBox = {page.CropBox}");
            Console.WriteLine($"doc[{i}].BleedBox = {page.BleedBox}");
            Console.WriteLine($"doc[{i}].TrimBox = {page.TrimBox}");
            Console.WriteLine($"doc[{i}].ArtBox = {page.ArtBox}");

            List<(int xref, string ext, string type, string baseName, string name, string encoding, int? referencer)> fonts =
                doc.GetPageFonts(i);
            Console.WriteLine($"doc.GetPageFonts({i}).Count = {fonts.Count}");
            foreach (var font in fonts)
                Console.WriteLine($"  xref={font.xref} ext={font.ext} type={font.type} baseName={font.baseName} name={font.name} encoding={font.encoding}");

            List<Entry> images = doc.GetPageImages(i);
            Console.WriteLine($"doc.GetPageImages({i}).Count = {images.Count}");
            foreach (Entry img in images)
                Console.WriteLine($"  xref={img.Xref} name={img.Name} width={img.Width} height={img.Height} bpc={img.Bpc} cs={img.CsName}");

            List<Dictionary<string, object>> xobjects = doc.GetPageXobjects(i);
            Console.WriteLine($"doc.GetPageXobjects({i}).Count = {xobjects.Count}");
            foreach (Dictionary<string, object> xo in xobjects)
            {
                foreach (var kv in xo)
                    Console.WriteLine($"  {kv.Key} = {kv.Value}");
            }

            List<(int xref, AnnotationType type, string id)> annots = doc.GetPageAnnotXrefs(i);
            Console.WriteLine($"doc.GetPageAnnotXrefs({i}).Count = {annots.Count}");
            foreach (var annot in annots)
                Console.WriteLine($"  xref={annot.xref} type={annot.type} id={annot.id}");
        }
    }
    // doc-end:PageProperties
}
