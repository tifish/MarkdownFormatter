namespace MarkdownFormatter;

public class SurroundCodeBlockWithBlankLines : BaseFormatter
{
    public override void Format(List<string> lines, string filePath)
    {
        var inCodeBlock = false;

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];

            if (inCodeBlock)
            {
                if (!IsCodeBlockEnd(line))
                    continue;
                inCodeBlock = false;

                if (i >= lines.Count - 1)
                    continue;

                var nextLine = lines[i + 1].Trim();
                if (nextLine != "")
                    lines.Insert(i + 1, "");
            }
            else if (IsCodeBlockBegin(line) || IsCodeBlockEnd(line))
            {
                inCodeBlock = true;

                if (i == 0)
                    continue;

                var prevLine = lines[i - 1].Trim();
                if (prevLine == "")
                    continue;

                lines.Insert(i, "");
                i++;
            }
        }
    }
}
