using System;
using System.Collections.Generic;
using System.Linq;

namespace Xbox360TitleParser.Common
{
    public class SpecialDirectories
    {
        private static Dictionary<string, string> contentDirectories = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "00007000", "GOD" },
            { "00004000", "NXE" },
            { "00005000", "Xbox" },
            { "000D0000", "XBLA" },
            { "00080000", "Demo" },
            { "00000002", "DLC" },
            { "000B0000", "TU" },
            { "000E0000", "XNA" }
        };

        public static List<string> GetSpecialDirectories()
        {
            return contentDirectories.Keys.ToList();
        }

        public static List<string> GetSpecialNames(List<string> directories)
        {
            return contentDirectories.Where(cd =>
                GetSpecialDirectories().Intersect(directories, StringComparer.OrdinalIgnoreCase).Contains(cd.Key, StringComparer.OrdinalIgnoreCase))
                .Select(cd => cd.Value).ToList();
        }

        public static string GetSpecialName(string directory)
        {
            return IsSpecialDirectory(directory) ? contentDirectories[directory.ToUpper()] : null;
        }

        public static bool IsSpecialDirectory(string directory)
        {
            return contentDirectories.ContainsKey(directory);
        }
    }
}
