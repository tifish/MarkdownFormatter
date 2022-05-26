namespace MarkdownFormatter;

public class EndWithSingleNewLine : BaseFormatter
{
    public override void Format(List<string> lines, string filePath)
    {
        while (lines.Count > 0 && lines[^1].Trim() == "")
            lines.RemoveAt(lines.Count - 1);
    }
}
