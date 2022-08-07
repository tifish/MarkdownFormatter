namespace MarkdownFormatter.Formatters;

public class RemoveMultipleBlankLines : BaseFormatter
{
    public override void Format(List<string> lines, string filePath)
    {
        var isPrevLineBlank = false;
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (IsCodeBlock(line))
            {
                isPrevLineBlank = false;
                continue;
            }

            var trimmedLine = line.Trim();
            if (trimmedLine == "")
            {
                if (isPrevLineBlank)
                {
                    lines.RemoveAt(i);
                    i--;
                }
                else
                {
                    isPrevLineBlank = true;
                }
            }
            else
            {
                isPrevLineBlank = false;
            }
        }
    }
}
