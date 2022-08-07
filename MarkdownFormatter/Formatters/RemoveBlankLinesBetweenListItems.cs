namespace MarkdownFormatter.Formatters;

public class RemoveBlankLinesBetweenListItems : BaseFormatter
{
    public override void Format(List<string> lines, string filePath)
    {
        var prevListLineNum = -1;
        var deleteLineNums = new List<int>();

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (IsCodeBlock(line))
            {
                prevListLineNum = -1;
                continue;
            }

            var trimmedLine = line.Trim();

            if (IsList(trimmedLine))
            {
                if (prevListLineNum == -1)
                {
                    prevListLineNum = i;
                }
                else
                {
                    if (prevListLineNum < i - 1)
                        for (var j = prevListLineNum + 1; j < i; j++)
                            deleteLineNums.Add(j);

                    prevListLineNum = i;
                }
            }
            else if (trimmedLine != "")
            {
                prevListLineNum = -1;
            }
        }

        for (var i = deleteLineNums.Count - 1; i >= 0; i--)
            lines.RemoveAt(deleteLineNums[i]);
    }
}
