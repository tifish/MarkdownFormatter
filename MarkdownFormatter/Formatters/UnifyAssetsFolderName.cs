using System.Text.RegularExpressions;

namespace MarkdownFormatter.Formatters;

public class UnifyAssetsFolderName : BaseFormatter
{
    private static readonly Regex ImageRegex = new(@"!\[([^\]]+)\]\((([^\\/]+)\.assets/([^\)]+))\)");

    public override void Format(List<string> lines, string filePath)
    {
        var fileNameNoExt = Path.GetFileNameWithoutExtension(filePath);

        RETRY:
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (IsCodeBlock(line))
                continue;

            var match = ImageRegex.Match(line);
            while (match.Success)
            {
                var parentFolder = Path.GetDirectoryName(filePath)!;
                var assetPath = match.Groups[2].Value.Replace("%20", " ");
                var fullAssetPath = Path.Combine(parentFolder, assetPath);
                if (File.Exists(fullAssetPath))
                {
                    var assetsFolderBaseName = match.Groups[3].Value.Replace("%20", " ");
                    if (assetsFolderBaseName != fileNameNoExt)
                    {
                        var wrongAssetsFolder = Path.Combine(parentFolder, assetsFolderBaseName + ".assets");
                        var rightAssetsFolder = Path.ChangeExtension(filePath, ".assets");
                        Directory.Move(wrongAssetsFolder, rightAssetsFolder);

                        var imageReplaceRegex = new Regex($@"(!\[[^\]]+\]\(){Regex.Escape(match.Groups[3].Value)}(\.assets/[^\)]+\))");
                        for (var lineNum = 0; lineNum < lines.Count; lineNum++)
                            lines[lineNum] = imageReplaceRegex.Replace(lines[lineNum], $@"$1{fileNameNoExt}$2");

                        goto RETRY;
                    }
                }
                else
                {
                    Console.Error.WriteLine($"Cannot find asset \"{assetPath}\" in \"{filePath}\" line {i + 1}");
                }

                match = ImageRegex.Match(line, match.Index + match.Length);
            }
        }
    }
}
