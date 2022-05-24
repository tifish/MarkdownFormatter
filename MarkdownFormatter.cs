using System.Text;

namespace MarkdownFormatter;

public static class MarkdownFormatter
{
    static MarkdownFormatter()
    {
        Formatters = new List<BaseFormatter>
        {
            new FirstLineShouldBeNotBlank(),
            new EndWithSingleNewLine(),
            new RemoveMultipleBlankLines(),
            new SurroundHeadingsWithBlankLines(),
            new UnifyListCharacter(),
            new RemoveBlankLinesBetweenListItems(),
            new SurroundCodeBlockWithBlankLines(),
            new SurroundCodeWithOnlyOneBacktick(),
            new AddSpacesBetweenLatinAndCjk(),
        };
    }

    private static readonly List<BaseFormatter> Formatters;

    public static void Format(string markdownFile)
    {
        var lines = File.ReadAllLines(markdownFile).ToList();
        Format(lines);
        File.WriteAllLines(markdownFile, lines, new UTF8Encoding(true));

        FormatFileName(markdownFile);
    }

    private static void FormatFileName(string markdownFile)
    {
        var fileNameNoExt = Path.GetFileNameWithoutExtension(markdownFile);
        fileNameNoExt = AddSpacesBetweenLatinAndCjk.AddSpaces(fileNameNoExt);
        if (fileNameNoExt == "")
            return;

        var dir = Path.GetDirectoryName(markdownFile);
        var newMarkdownFile = string.Concat(
            dir == null ? "" : dir + @"\", fileNameNoExt, Path.GetExtension(markdownFile));
        File.Move(markdownFile, newMarkdownFile);
    }

    public static void Format(List<string> lines)
    {
        foreach (var formatter in Formatters)
        {
            formatter.Reset();
            formatter.Format(lines);
        }
    }
}
