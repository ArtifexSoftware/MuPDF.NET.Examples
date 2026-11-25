# 01-Basic

Open a PDF, read document and page information, and write a full copy.

Each feature is its own method in `Program.cs` (`DocumentIdentity`, `ReadMetadata`, `PageProperties`, `SaveCopy`, and the others). `Basic()` opens `basic.pdf`, calls `Encryption()` before unlocking, then authenticates with user password `user` and runs the other methods.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## APIs used

- `Document.Open`
- `Document.Save`
- Document and page property reads (one method per feature)
