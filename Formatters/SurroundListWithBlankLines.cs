namespace MarkdownFormatter;

public class SurroundListWithBlankLines : BaseFormatter
{
    public override void Format(List<string> lines)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (IsCodeBlock(line))
                continue;
            
            if (!IsList(line))
                continue;

            if (i < lines.Count - 1)
            {
                var nextLine = lines[i + 1].Trim();
                if (!IsList(nextLine) && nextLine != "")
                    lines.Insert(i + 1, "");
            }

            if (i > 0)
            {
                var prevLine = lines[i - 1].Trim();
                if (!IsList(prevLine) && prevLine != "")
                {
                    lines.Insert(i, "");
                    i++;
                }
            }
        }
    }
}
