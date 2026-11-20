Unlock
======

Unlock Office document support and read the font directories MuPDF.NET.Office will search.

.. code-block:: cs

    using MuPDF.NET.Office;

    string? key = Environment.GetEnvironmentVariable("MUPDF_OFFICE_KEY")?.Trim();
    if (string.IsNullOrEmpty(key))
        key = null;
    int flags = MuPDFOffice.Unlock(key, fontPathAuto: true);
    var fonts = MuPDFOffice.GetFontPath();

API: `Unlock <https://mupdfnet.readthedocs.io/en/latest/office/index.html#MuPDFOffice.Unlock>`__, `GetFontPath <https://mupdfnet.readthedocs.io/en/latest/office/index.html#MuPDFOffice.GetFontPath>`__.

Unlock Office document support and inspect font search paths
-------------------------------------------------------------

.. literalinclude:: ../../MuPDF.NET.Office/01-UnlockFonts/Program.cs
   :language: cs
   :start-after: doc-start:UnlockFonts
   :end-before: doc-end:UnlockFonts
   :dedent: 4

API: `Unlock <https://mupdfnet.readthedocs.io/en/latest/office/index.html#MuPDFOffice.Unlock>`__, `GetFontPath <https://mupdfnet.readthedocs.io/en/latest/office/index.html#MuPDFOffice.GetFontPath>`__.

Result:

.. literalinclude:: ../../MuPDF.NET.Office/01-UnlockFonts/Expected/unlock.summary.txt
   :language: text
