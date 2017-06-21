using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbox360TitleParser.Common;
using System.Threading.Tasks;
using System.IO;

namespace Xbox360TitleParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Result r = new Result();

            string path = Directory.GetCurrentDirectory(); // path = "S:\\360\\Games";

            if (args.Count() > 0)
            {
                path = args[0];
            }
            
            Console.WriteLine("Scanning directory '{0}' for TitleIds...", path);

            List<GameAsset> gameAssets = DirectoryScanner.GetGameAssets(path, out r);

            Console.WriteLine("Found {0} possible TitleId(s)", gameAssets.Count);

            if (gameAssets.Count > 0)
            {
                Console.WriteLine("Processing...");
                Parallel.ForEach<GameAsset>(gameAssets, ga => r.Append(ga.Process(path)));
                Console.WriteLine("Done");
            }
            if (!r.Ok || r.ContainsWarnings)
            {
                Console.WriteLine("There were {0}:", r.Ok ? "warnings" : "errors");
                Console.WriteLine(r.ToString(true, true));
                //Console.WriteLine(r.ToString());
            }

            
            
            Console.WriteLine();
            Console.WriteLine("Press key to exit");
            Console.ReadKey(true);
        }
    }
}
