using System;

namespace MarkdownFormatter;

public class UnifyListCharacter : BaseFormatter
{
    public override void Format(List<string> lines, string filePath)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (IsCodeBlock(line))
                continue;
            
            var trimmedLine = line.TrimStart();
            if (trimmedLine.StartsWith("* ")
                || trimmedLine.StartsWith("+ "))
            {
                var startIndex = line.Length - trimmedLine.Length;
                lines[i] = string.Concat(line.AsSpan(0, startIndex), "-", line.AsSpan(startIndex + 1));
            }
        }
    }
}
