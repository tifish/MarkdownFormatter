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

            var newLine = AddSpaces(line);
            if (newLine != "")
                lines[i] = newLine;
        }
    }

    private static readonly Regex CjkRegex = new(
        @"["
        + @"\p{IsHangulJamo}"
        + @"\p{IsCJKRadicalsSupplement}"
        + @"\p{IsEnclosedCJKLettersandMonths}"
        + @"\p{IsCJKCompatibility}"
        + @"\p{IsCJKUnifiedIdeographsExtensionA}"
        + @"\p{IsCJKUnifiedIdeographs}"
        + @"\p{IsHangulSyllables}"
        // + @"\p{IsCJKCompatibilityForms}"
        + @"]"
    );

    private static bool IsCjkCharacter(char character)
    {
        return CjkRegex.IsMatch(character.ToString());
    }

    private static readonly Regex CjkPunctuationRegex = new(
        @"["
        + @"\p{IsCJKSymbolsandPunctuation}" // 中文标点
        + @"\p{IsHalfwidthandFullwidthForms}" // 半角和全角字符
        + @"]"
    );

    private static bool IsCjkPunctuation(char character)
    {
        return CjkPunctuationRegex.IsMatch(character.ToString());
    }

    private static readonly Regex LatinGroupRegex = new(
        @"[!-/:-@\[-`\{-~]*[a-zA-Z\d]+[!-/:-@\[-`\{-~]*"
    );

    private static readonly Regex[] QuotedWithSpacesRegexList =
    {
        new(@"<\w+://[^>]*>"),
        new(@"`[^`]*`"),
    };

    private static readonly Regex[] QuotedRegexList =
    {
        new(@"<\w+://[^>]*>"),
        new(@"`[^`]*`"),
        new(@"\[[^]]*\]\([^)]*\)"),
        // new(@"\*\*\*[^*]\*\*\*`"),
        // new(@"\*\*[^*]\*\*`"),
        // new(@"\*[^*]\*`"),
        new(@"""[^""]*"""),
        new(@"“[^”]*”"),
        new(@"《[^》]*》"),
        new(@"【[^】]*】"),
    };

    private static readonly List<int> InsertSpacePositions = new();
    private static readonly StringBuilder AddSpacesStringBuilder = new();

    public static string AddSpaces(string text)
    {
        InsertSpacePositions.Clear();

        // Add spaces for latins and number begin or end with punctuation
        var latinMatches = LatinGroupRegex.Matches(text);
        if (latinMatches.Count == 0)
            return "";

        var quotedMatches = QuotedRegexList.SelectMany(regex => regex.Matches(text)).ToList();

        foreach (Match latinMatch in latinMatches)
        {
            var begin = latinMatch.Index;
            var next = latinMatch.Index + latinMatch.Length;
            var end = next - 1;

            // Skip quoted text
            if (quotedMatches.Any(m => m.Index <= begin && begin < m.Index + m.Length
                                       || m.Index <= end && end < m.Index + m.Length))
                continue;

            if (begin > 0
                && text[begin] is not (']' or ')' or '}' or '>' or '*' or '.' or '"')
                && IsCjkCharacter(text[begin - 1]))
                InsertSpacePositions.Add(begin);

            if (next <= text.Length - 1
                && text[next - 1] is not ('[' or '(' or '{' or '<' or '*' or '.' or '"')
                && IsCjkCharacter(text[next]))
                InsertSpacePositions.Add(next);
        }

        // Add spaces for `xxx` and <http://xxx>
        var quotedWithSpacesMatches = QuotedWithSpacesRegexList.SelectMany(
            regex => regex.Matches(text)).ToList();

        foreach (var quotedWithSpacesMatch in quotedWithSpacesMatches)
        {
            var begin = quotedWithSpacesMatch.Index;
            var next = quotedWithSpacesMatch.Index + quotedWithSpacesMatch.Length;
            var end = next - 1;

            if (begin > 0
                && (char.IsLetterOrDigit(text[begin - 1]) || IsCjkCharacter(text[begin - 1])))
                InsertSpacePositions.Add(begin);

            if (next <= text.Length - 1
                && (char.IsLetterOrDigit(text[next]) || IsCjkCharacter(text[next])))
                InsertSpacePositions.Add(next);
        }

        // Insert spaces
        if (InsertSpacePositions.Count == 0)
            return "";

        InsertSpacePositions.Sort();

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
