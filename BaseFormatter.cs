using System.Text.RegularExpressions;

namespace MarkdownFormatter;

public abstract class BaseFormatter
{
    public abstract void Format(List<string> lines, string filePath);

    public virtual void Reset()
    {
        _inCodeBlock = false;
    }

    protected bool IsList(string line)
    {
        return IsUnorderedList(line) || IsOrderedList(line);
    }

    protected bool IsUnorderedList(string line)
    {
        var trimmedLine = line.TrimStart();
        return trimmedLine.StartsWith("- ")
               || trimmedLine.StartsWith("* ")
               || trimmedLine.StartsWith("+ ");
    }

    private static readonly Regex OrderedListPrefixRegex = new(@"^\d+\. ");

    protected bool IsOrderedList(string line)
    {
        var trimmedLine = line.TrimStart();
        return OrderedListPrefixRegex.IsMatch(trimmedLine);
    }

    private static readonly Regex HeadingPrefixRegex = new(@"^#+ ");

    protected bool IsHeading(string line)
    {
        return HeadingPrefixRegex.IsMatch(line);
    }

    protected bool IsCodeBlockBegin(string line)
    {
        var trimmedLine = line.Trim();
        return trimmedLine.StartsWith("```")
               && trimmedLine.Length > 3
               && trimmedLine.IndexOf("```", 3, StringComparison.Ordinal) == -1;
    }

    protected bool IsCodeBlockEnd(string line)
    {
        return line.Trim() == "```";
    }

    private bool _inCodeBlock;

    protected bool IsCodeBlock(string line)
    {
        if (_inCodeBlock)
        {
            if (IsCodeBlockEnd(line))
                _inCodeBlock = false;
            return true;
        }

        if (IsCodeBlockBegin(line) || IsCodeBlockEnd(line))
        {
            _inCodeBlock = true;
            return true;
        }

        return false;
    }
}
