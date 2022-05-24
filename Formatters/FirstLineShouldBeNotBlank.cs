namespace MarkdownFormatter;

public class FirstLineShouldBeNotBlank : BaseFormatter
{
    public override void Format(List<string> lines)
    {
        while (lines.Count > 0 && lines[0].Trim() == "")
            lines.RemoveAt(0);
    }
}
