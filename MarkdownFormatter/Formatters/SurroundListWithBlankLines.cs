namespace MarkdownFormatter.Formatters;

public class SurroundListWithBlankLines : BaseFormatter
{
    public override void Format(List<string> lines, string filePath)
    {
        var isInListParagraph = false;

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];

            if (IsCodeBlock(line)
                || string.IsNullOrWhiteSpace(line)
                || IsHeading(line))
            {
                isInListParagraph = false;
                continue;
            }

            if (isInListParagraph)
                continue;

            if (!IsList(line))
                continue;

            isInListParagraph = true;

            if (i > 0 && !string.IsNullOrWhiteSpace(lines[i - 1]))
                lines.Insert(i, "");
        }
    }
}
