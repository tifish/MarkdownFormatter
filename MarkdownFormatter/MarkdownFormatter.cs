using System.Text;
using MarkdownFormatter.Formatters;

namespace MarkdownFormatter;

public static class MarkdownFormatter
{
    static MarkdownFormatter()
    {
        Formatters = new List<BaseFormatter>
        {
            new FirstLineShouldBeNotBlank(),
            new EndWithSingleNewLine(),
            new RemoveTrailingSpaces(),
            new RemoveMultipleBlankLines(),
            new SurroundHeadingsWithBlankLines(),
            new UnifyListCharacter(),
            new OnlyOneSpaceAfterListCharacter(),
            new RemoveBlankLinesBetweenListItems(),
            new SurroundListWithBlankLines(),
            new SurroundCodeBlockWithBlankLines(),
            new SurroundCodeWithOnlyOneBacktick(),
            new AddSpacesBetweenLatinAndCjk(),
            new UnifyAssetsFolderName(),
        };
    }

    private static readonly List<BaseFormatter> Formatters;

    public static void Format(string markdownFile)
    {
        FormatFileName(ref markdownFile);

        var lines = File.ReadAllLines(markdownFile).ToList();
        Format(lines, markdownFile);
        File.WriteAllLines(markdownFile, lines, new UTF8Encoding(true));
    }

    private static void FormatFileName(ref string markdownFile)
    {
        var fileNameNoExt = Path.GetFileNameWithoutExtension(markdownFile);
        fileNameNoExt = AddSpacesBetweenLatinAndCjk.AddSpaces(fileNameNoExt);
        if (fileNameNoExt == "")
            return;

        var dir = Path.GetDirectoryName(markdownFile);
        var newMarkdownFile = string.Concat(
            dir == null ? "" : dir + @"\", fileNameNoExt, Path.GetExtension(markdownFile));
        File.Move(markdownFile, newMarkdownFile);
        markdownFile = newMarkdownFile;
    }

    public static void Format(List<string> lines, string filePath)
    {
        foreach (var formatter in Formatters)
        {
            formatter.Reset();
            formatter.Format(lines, filePath);
        }
    }
}
