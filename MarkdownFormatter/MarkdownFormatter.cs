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
            new RemoveTrailingSpaces(),
            new RemoveMultipleBlankLines(),

            new SurroundHeadingsWithBlankLines(),
            new UnifyListCharacter(),
            new OnlyOneSpaceAfterListCharacter(),
            new RemoveBlankLinesBetweenListItems(),
            new SurroundListWithBlankLines(),
            new SurroundCodeBlockWithBlankLines(),
            new SurroundCodeWithOnlyOneBacktick(),

            new UnifyAssetsFolderName(),

            new AddSpacesBetweenLatinAndCjk(),

            // new EndWithSingleNewLine(),
        };
    }

    private static readonly List<BaseFormatter> Formatters;

    public static void Format(string markdownFile)
    {
        FormatFileName(ref markdownFile);

        bool hasEndOfFileReturn;
        var lines = new List<string>();

        using (var stream = new FileStream(markdownFile, FileMode.Open))
        {
            if (stream.Length == 0)
                return;

            stream.Seek(-1, SeekOrigin.End);
            var lastByte = stream.ReadByte();
            hasEndOfFileReturn = lastByte == '\n';
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);

            var line = reader.ReadLine();
            while (line != null)
            {
                lines.Add(line);
                line = reader.ReadLine();
            }
        }

        if (lines.Count == 0)
            return;

        Format(lines, markdownFile);

        using (var writer = new StreamWriter(markdownFile, new UTF8Encoding(true),
                   new FileStreamOptions { Mode = FileMode.Open, Access = FileAccess.Write }))
        {
            for (var i = 0; i < lines.Count - 1; i++)
                writer.WriteLine(lines[i]);

            if (hasEndOfFileReturn || EndWithSingleNewLine.Processed)
                writer.WriteLine(lines[^1]);
            else
                writer.Write(lines[^1]);
        }
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
