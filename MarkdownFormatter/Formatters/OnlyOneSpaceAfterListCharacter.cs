using System;

namespace MarkdownFormatter;

public class OnlyOneSpaceAfterListCharacter : BaseFormatter
{
    public override void Format(List<string> lines, string filePath)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (IsCodeBlock(line))
                continue;

            var trimmedLine = line.TrimStart();
            if (!trimmedLine.StartsWith("- "))
                continue;

            var startIndex = line.IndexOf("- ", StringComparison.Ordinal);
            var extraSpaceStartIndex = startIndex + 2;
            var extraSpaceEndIndex = extraSpaceStartIndex;
            while (extraSpaceEndIndex < line.Length && line[extraSpaceEndIndex] == ' ')
            {
                extraSpaceEndIndex++;
            }

            if (extraSpaceStartIndex == extraSpaceEndIndex)
                continue;

            lines[i] = string.Concat(line.AsSpan(0, extraSpaceStartIndex), line.AsSpan(extraSpaceEndIndex));
        }
    }
}
