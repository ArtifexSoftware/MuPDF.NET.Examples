Pages
=====

Merge a page from one PDF into another, then copy the first page into a new document.

.. code-block:: cs

    using MuPDF.NET;

    using (var docA = Document.Open(@"D:\sample.pdf"))
    using (var docB = Document.Open(@"D:\blank.pdf"))
    {
        docA.InsertPdf(docB, fromPage: 0, toPage: 0, startAt: 1);
        docA.Save(@"D:\merged.pdf");
    }

    using (var src = Document.Open(@"D:\merged.pdf"))
    using (var one = new Document())
    {
        one.InsertPdf(src, fromPage: 0, toPage: 0);
        one.Save(@"D:\first-page.pdf");
    }

API: `Document.Open <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.Document>`__, `InsertPdf <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.InsertPdf>`__, `Save <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.Save>`__.

Merge pages from two PDFs, then extract the first page
-------------------------------------------------------

.. literalinclude:: ../../MuPDF.NET/02-PagesMergeSplit/Program.cs
   :language: cs
   :start-after: doc-start:PagesMergeSplit
   :end-before: doc-end:PagesMergeSplit
   :dedent: 4

API: `Document.Open <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.Document>`__, `InsertPdf <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.InsertPdf>`__, `Save <https://mupdfnet.readthedocs.io/en/latest/classes/Document.html#Document.Save>`__.

Result of ``merged.pdf``:

.. literalinclude:: ../../MuPDF.NET/02-PagesMergeSplit/Expected/merged.summary.txt
   :language: text

Result of ``first-page.pdf``:

.. literalinclude:: ../../MuPDF.NET/02-PagesMergeSplit/Expected/first-page.summary.txt
   :language: text
