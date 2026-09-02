# 10-OutlineLinks

Build a table of contents (bookmarks) and an internal go-to link.

## Sample method

`OutlineLinks()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | Created in code (two pages + text) |
| Output | `Output/MuPDF.NET/10-OutlineLinks/outline-links.pdf` |
| Expected | `outline-links.txt`, `outline-links.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\10-OutlineLinks
```

## APIs used

- `TextWriter` / `Font`
- `Document.SetToc` / `GetToc`
- `Page.InsertLink` / `GetLinks`
