# 01-Basic

Open a PDF, read document and page information, and write a full copy.

Each feature is its own method in `Program.cs` (`DocumentIdentity`, `ReadMetadata`, `PageProperties`, `SaveCopy`, and the others). `OpenSave()` calls them in order.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## APIs used

- `Document.Open`
- `Document.Save`
- Document and page property reads (one method per feature)
