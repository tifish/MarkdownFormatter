namespace MarkdownFormatter;

public class SurroundHeadingsWithBlankLines : BaseFormatter
{
    public override void Format(List<string> lines)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (IsCodeBlock(line))
                continue;
            
            if (!IsHeading(line))
                continue;

            if (i < lines.Count - 1 && lines[i + 1].Trim() != "")
                lines.Insert(i + 1, "");

            if (i > 0 && lines[i - 1].Trim() != "")
            {
                lines.Insert(i, "");
                i++;
            }
        }
    }
}
