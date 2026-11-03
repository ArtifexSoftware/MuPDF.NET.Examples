# MuPDF.NET LLM / RAG samples

These console apps are **MuPDF.NET 4.0** samples (Markdown, layout JSON, OCR, tables, RAG). They live in this folder so Input/Expected paths stay stable. They do **not** need the optional `MuPDF.NET.PDF4LLM` NuGet stub — extract and `MuPDF4LLM.*` helpers ship in `MuPDF.NET.dll`.

```powershell
dotnet add package MuPDF.NET
```

Office/HWP files still need [MuPDF.NET.Office](https://www.nuget.org/packages/MuPDF.NET.Office) (`05-WithPdf4Llm` under `MuPDF.NET.Office/`).
