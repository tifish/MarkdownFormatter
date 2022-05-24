namespace MarkdownFormatter;

public class SurroundCodeWithOnlyOneBacktick : BaseFormatter
{
    public override void Format(List<string> lines)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (!line.Contains("```"))
                continue;
            if (IsCodeBlock(line))
                continue;

            lines[i] = line.Replace("```", "`");
        }
    }
}
