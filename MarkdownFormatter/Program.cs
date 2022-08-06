using System.Globalization;

namespace MarkdownFormatter;

class Program
{
    private static void Main(string[] args)
    {
        foreach (var arg in args)
            if (File.Exists(arg))
                MarkdownFormatter.Format(arg);
            else if (Directory.Exists(arg))
                foreach (var file in Directory.GetFiles(arg, "*.md", SearchOption.AllDirectories))
                    MarkdownFormatter.Format(file);
    }
}
