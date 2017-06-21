using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xbox360TitleParser.Common
{
    public class DirectoryScanner
    {
        public static List<GameAsset> GetGameAssets(string dir, out Result r)
        {
            r = new Result();
            List<GameAsset> gameAssets = new List<GameAsset>();

            if (String.IsNullOrEmpty(dir) || !Directory.Exists(dir))
            {
                r.AddError(String.Format("Invalid directory '{0}'", dir));
                return gameAssets;
            }
            
            foreach (string subDir in new DirectoryInfo(dir).GetDirectories().Select(di => di.Name).ToList())
            {
                if (TitleIdValidator.IsValid(subDir))
                {
                    // Add GameAsset with list of content based on the subfolders
                    gameAssets.Add(new GameAsset(subDir,
                        SpecialDirectories.GetSpecialNames(new DirectoryInfo(Path.Combine(dir, subDir)).GetDirectories().Select(di => di.Name).ToList())));

                    r.AddInfo(String.Format("GameAsset added for valid TitleId '{0}'", subDir));
                }
                else
                {
                    r.AddWarning(String.Format("No GameAsset added for invalid TitleId '{0}'", subDir));
                }
            }

            return gameAssets;
        }
    }
}
