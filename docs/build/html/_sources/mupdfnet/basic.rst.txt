Basic
=====

Open a PDF and read document and page information.

.. code-block:: cs

    using MuPDF.NET;

    using (var doc = Document.Open(@"D:\sample.pdf"))
    {
        DocumentIdentity(doc);
        ...
    }

API: `Document.Open <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.Document>`__.

Open a PDF and read its page count and kind
---------------------------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:DocumentIdentity
   :end-before: doc-end:DocumentIdentity
   :dedent: 4

API: `PageCount <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.PageCount>`__, `IsPdf <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.IsPDF>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 1-2

See whether the PDF is encrypted or still needs a password
-----------------------------------------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:Encryption
   :end-before: doc-end:Encryption
   :dedent: 4

API: `IsEncrypted <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.IsEncrypted>`__, `NeedsPass <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.NeedsPass>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 3-4

Read document state flags
--------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:DocumentFlags
   :end-before: doc-end:DocumentFlags
   :dedent: 4

API: `IsReflowable <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.IsReflowable>`__, `IsDirty <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.IsDirty>`__, `IsFormPdf <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.IsFormPDF>`__, `IsRepaired <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.IsRepaired>`__, `IsFastWebaccess <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.IsFastWebaccess>`__, `IsClosed <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.IsClosed>`__, `CanSaveIncrementally <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.CanSaveIncrementally>`__, `JournalIsEnabled <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.IsEnabledJournal>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 5-12

Read PDF structure: chapters, versions, xref length, and catalog
-----------------------------------------------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:PdfStructure
   :end-before: doc-end:PdfStructure
   :dedent: 4

API: `ChapterCount <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.ChapterCount>`__, `ChapterPageCount <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetChapterPageCount>`__, `VersionCount <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.VersionCount>`__, `XrefLength <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetXrefLength>`__, `PdfCatalog <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetPdfCatelog>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 13-17

Read catalog viewer settings
-----------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:ViewerSettings
   :end-before: doc-end:ViewerSettings
   :dedent: 4

API: `Language <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.Language>`__, `PageLayout <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.PageLayout>`__, `PageMode <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.PageMode>`__, `NeedAppearances <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.NeedAppearances>`__, `GetSigFlags <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetSigFlags>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 18-22

Read the permission bitmask
----------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:Permissions
   :end-before: doc-end:Permissions
   :dedent: 4

API: `Permissions <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.Permissions>`__, `PDF_PERM_PRINT <https://mupdfnet.readthedocs.io/en/latest/glossary/vars.html#PermissionCodes>`__, `PDF_PERM_MODIFY <https://mupdfnet.readthedocs.io/en/latest/glossary/vars.html#PermissionCodes>`__, `PDF_PERM_COPY <https://mupdfnet.readthedocs.io/en/latest/glossary/vars.html#PermissionCodes>`__, `PDF_PERM_ANNOTATE <https://mupdfnet.readthedocs.io/en/latest/glossary/vars.html#PermissionCodes>`__, `PDF_PERM_FORM <https://mupdfnet.readthedocs.io/en/latest/glossary/vars.html#PermissionCodes>`__, `PDF_PERM_ACCESSIBILITY <https://mupdfnet.readthedocs.io/en/latest/glossary/vars.html#PermissionCodes>`__, `PDF_PERM_ASSEMBLE <https://mupdfnet.readthedocs.io/en/latest/glossary/vars.html#PermissionCodes>`__, `PDF_PERM_PRINT_HQ <https://mupdfnet.readthedocs.io/en/latest/glossary/vars.html#PermissionCodes>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 23-31

List AcroForm font names
-------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:FormFonts
   :end-before: doc-end:FormFonts
   :dedent: 4

API: `FormFonts <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.FormFonts>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 32

List embedded file names
-------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:EmbeddedNames
   :end-before: doc-end:EmbeddedNames
   :dedent: 4

API: `EmbeddedFileCount <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetEmbfileCount>`__, `GetEmbeddedFileNames <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetEmbfileNames>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 33

Check whether the PDF has any annotations or links
---------------------------------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:AnnotsAndLinks
   :end-before: doc-end:AnnotsAndLinks
   :dedent: 4

API: `HasAnnots <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.HasAnnots>`__, `HasLinks <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.HasLinks>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 34-35

Read MarkInfo flags from the catalog
-------------------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:MarkInfo
   :end-before: doc-end:MarkInfo
   :dedent: 4

API: `MarkInfo <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.MarkInfo>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 36

Read the standard PDF Info dictionary
--------------------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:ReadMetadata
   :end-before: doc-end:ReadMetadata
   :dedent: 4

API: `Metadata <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.MetaData>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 37-48

Read the XMP metadata stream
-----------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:XmlMetadata
   :end-before: doc-end:XmlMetadata
   :dedent: 4

API: `XrefXmlMetadata <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.XrefXmlMetaData>`__, `GetXmlMetadata <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetXmlMetadata>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 49-95

Read page label rules
----------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:PageLabels
   :end-before: doc-end:PageLabels
   :dedent: 4

API: `GetPageLabels <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetPageLabels>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 96-105

Read the table of contents
---------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:TableOfContents
   :end-before: doc-end:TableOfContents
   :dedent: 4

API: `GetToc <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetToc>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 106-110

Resolve named destinations
---------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:NamedDestinations
   :end-before: doc-end:NamedDestinations
   :dedent: 4

API: `ResolveNames <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.ResolveNames>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 111

Read optional content groups and layer UI
------------------------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:OptionalContent
   :end-before: doc-end:OptionalContent
   :dedent: 4

API: `GetOcgs <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetOcgs>`__, `GetLayers <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetLayers>`__, `LayerUiConfigs <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.LayerUIConfigs>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 112-114

Read the PDF trailer dictionary
--------------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:Trailer
   :end-before: doc-end:Trailer
   :dedent: 4

API: `PdfTrailer <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetPdfTrailer>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 115-128

Write a full copy of the open PDF
----------------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:SaveCopy
   :end-before: doc-end:SaveCopy
   :dedent: 4

API: `Save <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.Save>`__.

Read each page's boxes, fonts, images, XObjects, and annotation xrefs
----------------------------------------------------------------------

.. literalinclude:: ../../MuPDF.NET/01-Basic/Program.cs
   :language: cs
   :start-after: doc-start:PageProperties
   :end-before: doc-end:PageProperties
   :dedent: 4

API: `LoadPage <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.LoadPage>`__, `GetLabel <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.GetLabel>`__, `Xref <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.Xref>`__, `Rotation <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.Rotation>`__, `Width <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.Rect>`__, `Height <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.Rect>`__, `Rect <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.Rect>`__, `MediaBox <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.MediaBox>`__, `CropBox <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.CropBox>`__, `BleedBox <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.BleedBox>`__, `TrimBox <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.TrimBox>`__, `ArtBox <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.ArtBox>`__, `GetPageFonts <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetPageFonts>`__, `GetPageImages <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetPageImages>`__, `GetPageXobjects <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.GetPageXObjects>`__, `GetPageAnnotXrefs <https://mupdfnet.readthedocs.io/en/latest/classes/Page.html#Page.GetAnnotXrefs>`__.

Result:

.. literalinclude:: ../../MuPDF.NET/01-Basic/Expected/document-info.txt
   :language: text
   :lines: 129-186
