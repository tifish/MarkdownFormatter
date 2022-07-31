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
    private static readonly Regex LinkRegex = new(
        @"\[[^]]*\]\([^)]*\)"
    );

    private static readonly List<int> InsertSpacePositions = new();
    private static readonly StringBuilder AddSpacesStringBuilder = new();

    public static string AddSpaces(string text)
    {
        var linkMatches = LinkRegex.Matches(text);

        InsertSpacePositions.Clear();

        var latinMatches = LatinGroupRegex.Matches(text);
        if (latinMatches.Count == 0)
            return "";

        foreach (Match latinMatch in latinMatches)
        {
            var begin = latinMatch.Index;
            var next = latinMatch.Index + latinMatch.Length;

            // Skip links
            if (linkMatches.Any(m => begin >= m.Index && begin < m.Index + m.Length))
                continue;

            if (begin > 0
                && text[begin] is not ('*' or ']' or ')' or '}' or '>' or '.')
                && IsCjkCharacter(text[begin - 1]))
                InsertSpacePositions.Add(begin);

            if (next <= text.Length - 1
                && text[next - 1] is not ('*' or '[' or '(' or '{' or '<' or '.')
                && IsCjkCharacter(text[next]))
                InsertSpacePositions.Add(next);
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
