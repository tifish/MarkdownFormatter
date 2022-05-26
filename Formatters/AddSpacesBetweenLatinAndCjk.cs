using System.Text;
using System.Text.RegularExpressions;

namespace MarkdownFormatter;

public class AddSpacesBetweenLatinAndCjk : BaseFormatter
{
    public override void Format(List<string> lines, string filePath)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (IsCodeBlock(line))
                continue;

            var newLine = AddSpaces(lines[i]);
            if (newLine != "")
                lines[i] = newLine;
        }
    }

    private static readonly Regex CjkRegex = new(
        @"["
        + @"\p{IsHangulJamo}"
        + @"\p{IsCJKRadicalsSupplement}"
        // + @"\p{IsCJKSymbolsandPunctuation}" // 中文标点
        + @"\p{IsEnclosedCJKLettersandMonths}"
        + @"\p{IsCJKCompatibility}"
        + @"\p{IsCJKUnifiedIdeographsExtensionA}"
        + @"\p{IsCJKUnifiedIdeographs}"
        + @"\p{IsHangulSyllables}"
        + @"\p{IsCJKCompatibilityForms}"
        // + @"\p{IsHalfwidthandFullwidthForms}" // 半角和全角字符 
        + @"]"
    );

    private static bool IsCjkCharacter(char character)
    {
        return CjkRegex.IsMatch(character.ToString());
    }

    private static readonly Regex LatinGroupRegex = new(
        @"[!-~]*[a-zA-Z\d][!-~]*"
    );

    private static readonly List<int> InsertSpacePositions = new();
    private static readonly StringBuilder AddSpacesStringBuilder = new();

    public static string AddSpaces(string text)
    {
        InsertSpacePositions.Clear();

        var match = LatinGroupRegex.Match(text);
        if (!match.Success)
            return "";
        while (match.Success)
        {
            var begin = match.Index;
            if (begin > 0
                && text[begin] is not ('*' or ']' or ')' or '}' or '>')
                && IsCjkCharacter(text[begin - 1]))
                InsertSpacePositions.Add(begin);

            var next = match.Index + match.Length;
            if (next <= text.Length - 1
                && text[next - 1] is not ('*' or '[' or '(' or '{' or '<')
                && IsCjkCharacter(text[next]))
                InsertSpacePositions.Add(next);

            match = LatinGroupRegex.Match(text, next);
        }

        if (InsertSpacePositions.Count == 0)
            return "";

        AddSpacesStringBuilder.Clear();
        var startIndex = 0;
        foreach (var position in InsertSpacePositions)
        {
            AddSpacesStringBuilder.Append(text.AsSpan(startIndex, position - startIndex));
            AddSpacesStringBuilder.Append(' ');
            startIndex = position;
        }

        AddSpacesStringBuilder.Append(text.AsSpan(startIndex));

        return AddSpacesStringBuilder.ToString();
    }
}
