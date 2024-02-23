using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace WPF_ThemeResource.Generator
{
    internal class Program
    {
        private const string ThemeResourceKeyTemplate =
@"
namespace WPF_ThemeResource
{
    public enum ThemeResourceKey
    {
{0}
    }
}
";

        private const string ThemeResourceKeyFile = @"..\..\..\WPF-ThemeResource\ThemeResourceKey.cs";

        private static readonly string[] ResourceFiles = new string[]
            {
                @"..\..\..\WPF-ThemeResource\Themes\Common.xaml",
                @"..\..\..\WPF-ThemeResource\Themes\Light.xaml",
                @"..\..\..\WPF-ThemeResource\Themes\Dark.xaml",
            };

        private static List<string> AdditionalThemeResourceKeys = new List<string>()
        {
            "SystemAccentColor",
            "SystemAccentColorPrimary",
            "SystemAccentColorSecondary",
            "SystemAccentColorTertiary",
            "SystemAccentBrush",
            "SystemAccentColorPrimaryBrush",
            "SystemAccentColorSecondaryBrush",
            "SystemAccentColorTertiaryBrush",
            "AccentTextFillColorPrimaryBrush",
            "AccentTextFillColorSecondaryBrush",
            "AccentTextFillColorTertiaryBrush",
            "AccentFillColorSelectedTextBackgroundBrush",
            "AccentFillColorDefaultBrush",
            "AccentFillColorSecondaryBrush",
            "AccentFillColorTertiaryBrush",
            "AccentFillColorDisabledBrush",
            "SystemFillColorAttentionBrush"
        };

        static void Main(string[] args)
        {
            // Extract keys
            var keys = new HashSet<string>();

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var relativePath in ResourceFiles) 
            {
                var absolutePath = Path.Combine(basePath, relativePath);

                var xml = File.ReadAllText(absolutePath);

                var regex = new Regex("x:Key=\"(.*?)\"");
                var matches = regex.Matches(xml);

                foreach (Match match in matches)
                {
                    var group = match.Groups[1].Value;
                    if (group[0] != '{')
                    {
                        // Ignore "{x:Static.." keys
                        keys.Add(group);
                    }
                }
            }

            foreach (var key in AdditionalThemeResourceKeys)
            {
                keys.Add(key);
            }

            // Generate ThemeResourceKey.cs file
            var keyLines = string.Join($",{Environment.NewLine}", keys.ToList().OrderBy(x => x));
            var themeResourceKeyFilePath = Path.Combine(basePath, ThemeResourceKeyFile);
            var themeResourceKeyFileContent = ThemeResourceKeyTemplate.Replace("{0}", keyLines);

            File.WriteAllText(themeResourceKeyFilePath, themeResourceKeyFileContent);
        }
    }
}
