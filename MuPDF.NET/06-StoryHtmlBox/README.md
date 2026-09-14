# 06-StoryHtmlBox

Lay out HTML into a page rectangle using Story / `InsertHtmlBox`.

## Sample method

`StoryHtmlBox()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | HTML string in code (no fixture file) |
| Output | `Output/MuPDF.NET/06-StoryHtmlBox/story.pdf` |
| Expected | `Expected/story.summary.txt` |

## Run

```powershell
dotnet run --project MuPDF.NET\06-StoryHtmlBox
```

## APIs used

- `Document.NewPage`
- `Page.InsertHtmlBox`
