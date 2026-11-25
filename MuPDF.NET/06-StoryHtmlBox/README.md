# 06-StoryHtmlBox

Lay out HTML into a page rectangle using Story / `InsertHtmlBox`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | HTML string in code (no fixture file) |
| Output | `Output/MuPDF.NET/06-StoryHtmlBox/story.pdf`, `story.png` |
| Expected | `story-info.txt`, `story.summary.txt`, `story.png.sha256` |

## Run

```powershell
dotnet run --project MuPDF.NET\06-StoryHtmlBox
```

## APIs used

- `Document.Open`
- `Document.NewPage`
- `Page.InsertHtmlBox`
- `Document.Save`
