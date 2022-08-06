namespace MarkdownFormatter;

public class RemoveTrailingSpaces : BaseFormatter
{
    public override void Format(List<string> lines, string filePath)
    {
        for (var i = 0; i < lines.Count; i++)
            lines[i] = lines[i].TrimEnd();
    }
}
