namespace MarkdownFormatter.Formatters;

public class EndWithSingleNewLine : BaseFormatter
{
    public static bool Processed { get;private set; }

    public EndWithSingleNewLine()
    {
        Processed = true;
    }

    public override void Format(List<string> lines, string filePath)
    {
        while (lines.Count > 0 && lines[^1].Trim() == "")
            lines.RemoveAt(lines.Count - 1);
    }
}
