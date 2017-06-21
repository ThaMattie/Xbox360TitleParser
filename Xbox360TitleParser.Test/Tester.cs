using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Drawing;
using System.Net;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbox360TitleParser.Common;
using System.Threading.Tasks;

namespace Xbox360TitleParser.Test
{
    [TestClass]
    public class Tester
    {
        [TestMethod]
        public void TestCorrectTitleIdRegex()
        {
            string[] titleIds = new string[]
            {
                "00000000",
                "01234567",
                "89012345",
                "abcdefed",
                "ABCDEFED",
                "aAbBcCdD",
                "0a1b2c3d",
                "0E1F2A3B",
                "0aA1bB2c"
            };

            DirectoryScanner ds = new DirectoryScanner();
 //           CollectionAssert.AreEquivalent(titleIds, ds.GetTitleIds(titleIds), "All TitleIds should be found");
        }

        [TestMethod]
        public void TestIncorrectTitleIdRegex()
        {
            string[] titleIds = new string[]
            {
                "0123456",
                "012345678",
                "abcdefe",
                "abcdefedc",
                "ABCDEFE",
                "ABCDEFEDC",
                "abcdefgf",
                "ABCDEFGF",
                "0123456-",
                "-abcdefe",
                "abcd123",
                "7890EFECD"
            };

            DirectoryScanner ds = new DirectoryScanner();
 //           Assert.AreEqual(0, ds.GetTitleIds(titleIds).Count(), "No TitleIds should be found");
        }

        [TestMethod]
        public void GetMarketPlaceURLForValidTitleId()
        {
            string tId = "58410B5D"; // Burnout Crash, XBLA
            DirectoryScanner ds = new DirectoryScanner();
 //           ds.GetMarketPlaceURL(tId);
        }

        [TestMethod]
        public void GetMarketPlaceXMLForValidURL()
        {
            //Burnout Crash: XBLA (changed detail view from 5 to 3)
            string url = "http://catalog.xboxlive.com/Catalog/Catalog.asmx/Query?methodName=FindGames&Names=Locale&Values=en-US&Names=LegalLocale&Values=en-US&Names=Store&Values=1&Names=PageSize&Values=100&Names=PageNum&Values=1&Names=DetailView&Values=3&Names=OfferFilterLevel&Values=1&Names=MediaIds&Values=66acd000-77fe-1000-9115-d80258410B5D&Names=UserTypes&Values=2&Names=MediaTypes&Values=1&Names=MediaTypes&Values=21&Names=MediaTypes&Values=23&Names=MediaTypes&Values=37&Names=MediaTypes&Values=46";
            DirectoryScanner ds = new DirectoryScanner();
//            ds.GetMarketPlaceXML(url);
        }

        [TestMethod]
        public void GetAssetsFromValidMarketPlaceXML()
        {
            #region xml

            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                            <feed xmlns:live=""http://www.live.com/marketplace"" xmlns=""http://www.w3.org/2005/Atom"">
                              <live:totalItems>1</live:totalItems>
                              <live:numItems>1</live:numItems>
                              <title>FindGames Results</title>
                              <updated>2011-12-04T23:24:44.43Z</updated>
                              <entry live:itemNum=""1"" live:detailView=""3"">
                                <id>urn:uuid:66ACD000-77FE-1000-9115-D80258410B5D</id>

                                <updated>2011-12-04T23:24:44.43Z</updated>
                                <title>Burnout™ Crash!</title>
                                <content type=""text"">game content</content>
                                <live:media>
                                  <live:mediaType>23</live:mediaType>
                                  <live:gameTitleMediaId>urn:uuid:66ACD000-77FE-1000-9115-D80258410B5D</live:gameTitleMediaId>

                                  <live:reducedTitle>Burnout™ Crash!</live:reducedTitle>
                                  <live:reducedDescription>Criterion Games brings you a totally new take on the much loved Crash Mode in this spin off from the multi award Winning Burnout Series! Go on the wildest Road Trip of your life; causing huge pile-ups, blowing up buildings and unleashing disastrous Super Features! Use Autolog to compete with and challenge your friends! Anyone can crash, but can you do it in style?
                            </live:reducedDescription>
                                  <live:availabilityDate>2011-07-29T00:00:00</live:availabilityDate>
                                  <live:releaseDate>2011-09-20T00:00:00</live:releaseDate>
                                  <live:ratingId>20</live:ratingId>
                                  <live:developer>Criterion Games</live:developer>

                                  <live:publisher>Electronic Arts</live:publisher>
                                  <live:newestOfferStartDate>2011-10-12T09:00:00Z</live:newestOfferStartDate>
                                  <live:totalOfferCount>6</live:totalOfferCount>
                                  <live:titleId>1480657757</live:titleId>
                                  <live:effectiveTitleId>1480657757</live:effectiveTitleId>
                                  <live:gameReducedTitle>Burnout™ Crash!</live:gameReducedTitle>

                                  <live:ratingAggregate>3.50</live:ratingAggregate>
                                  <live:numberOfRatings>3286</live:numberOfRatings>
                                </live:media>
                                <live:categories>
                                  <live:category>
                                    <live:categoryId>3000</live:categoryId>
                                    <live:system>3000</live:system>

                                    <live:name>Game Genres</live:name>
                                  </live:category>
                                  <live:category>
                                    <live:categoryId>3002</live:categoryId>
                                    <live:system>3000</live:system>
                                    <live:name>Action &amp; Adventure</live:name>

                                  </live:category>
                                  <live:category>
                                    <live:categoryId>3027</live:categoryId>
                                    <live:system>3000</live:system>
                                    <live:name>Xbox LIVE Games</live:name>
                                  </live:category>
                                </live:categories>

                                <live:images>
                                  <live:image>
                                    <live:imageMediaId>urn:uuid:6AEF64DD-055A-4982-A6E5-934894EC16BE</live:imageMediaId>
                                    <live:imageMediaInstanceId>urn:uuid:F70619A1-9B7D-4F3D-AC4D-544A8A6EBB6E</live:imageMediaInstanceId>
                                    <live:imageMediaType>14</live:imageMediaType>
                                    <live:relationshipType>23</live:relationshipType>
                                    <live:format>5</live:format>

                                    <live:size>14</live:size>
                                    <live:fileUrl>http://download.xbox.com/content/images/66acd000-77fe-1000-9115-d80258410b5d/1033/tile.png</live:fileUrl>
                                  </live:image>
                                  <live:image>
                                    <live:imageMediaId>urn:uuid:E020F231-CB69-4377-8ECE-330EC3DBF7CC</live:imageMediaId>
                                    <live:imageMediaInstanceId>urn:uuid:F6A22A12-9608-48EC-9EE1-8F6A78ED7EFC</live:imageMediaInstanceId>
                                    <live:imageMediaType>14</live:imageMediaType>

                                    <live:relationshipType>33</live:relationshipType>
                                    <live:format>4</live:format>
                                    <live:size>23</live:size>
                                    <live:fileUrl>http://download.xbox.com/content/images/66acd000-77fe-1000-9115-d80258410b5d/1033/boxartlg.jpg</live:fileUrl>
                                  </live:image>
                                  <live:image>
                                    <live:imageMediaId>urn:uuid:8F37A0F9-C20C-44D6-8B04-153834E54F9D</live:imageMediaId>

                                    <live:imageMediaInstanceId>urn:uuid:59123114-1DF8-4DF9-AC2A-AD3465A5D466</live:imageMediaInstanceId>
                                    <live:imageMediaType>14</live:imageMediaType>
                                    <live:relationshipType>25</live:relationshipType>
                                    <live:format>4</live:format>
                                    <live:size>22</live:size>
                                    <live:fileUrl>http://download.xbox.com/content/images/66acd000-77fe-1000-9115-d80258410b5d/1033/background.jpg</live:fileUrl>

                                  </live:image>
                                  <live:image>
                                    <live:imageMediaId>urn:uuid:B4769417-A588-4FF6-8BDE-69618A3E7E31</live:imageMediaId>
                                    <live:imageMediaInstanceId>urn:uuid:05AEE2BF-8E14-4E5C-A720-E584C7F47B97</live:imageMediaInstanceId>
                                    <live:imageMediaType>14</live:imageMediaType>
                                    <live:relationshipType>27</live:relationshipType>
                                    <live:format>5</live:format>

                                    <live:size>15</live:size>
                                    <live:fileUrl>http://download.xbox.com/content/images/66acd000-77fe-1000-9115-d80258410b5d/1033/banner.png</live:fileUrl>
                                  </live:image>
                                  <live:image>
                                    <live:imageMediaId>urn:uuid:F136B580-8B2D-46AF-801F-91EA4F097756</live:imageMediaId>
                                    <live:imageMediaInstanceId>urn:uuid:98149B23-45A5-47CE-B0F6-EB76CD0A708E</live:imageMediaInstanceId>
                                    <live:imageMediaType>14</live:imageMediaType>

                                    <live:relationshipType>33</live:relationshipType>
                                    <live:format>4</live:format>
                                    <live:size>30</live:size>
                                    <live:fileUrl>http://download.xbox.com/content/images/66acd000-77fe-1000-9115-d80258410b5d/1033/boxartsm.jpg</live:fileUrl>
                                  </live:image>
                                </live:images>
                              </entry>

                            </feed>";

            #endregion xml;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            DirectoryScanner ds = new DirectoryScanner();
 //           ds.GetAssetsFromMarketPlaceXML(xmlDoc);
        }

        [TestMethod]
        public void GetAssetsFromTitleIds()
        {
            List<string> titleIds = new List<string>() { "54540890", "434607D6", "504807D1", "505407d2", "46530001", "584109B7", "4156081F", "584109A8", "584807E4", "584A07E4",
                                                        "584A07DC", "584A07D1", "58550440", "465607D4", "585502b2", "585502cc", "584108aa", "454108E4", "81312314", "58550222",
                                                        "58550189", "5855019b", "58410975", "5855029b", "585504b4", "58410841", "58410A3B", "5451083B", "54510852", "4D5309ED",
                                                        "4D530980", "97287552", "585501d6", "58550427", "585501c8", "585502b7", "585504a1", "5855017e", "5855037a", "584108db",
                                                        "5855012e", "58550219", "58550206", "58550180", "584109DB", "5855032b", "414B07D1", "58550166", "55530808", "58550378",
                                                        "5855012c", "585503fe", "585504b9", "585501a7", "58550431", "58550108", "465307da", "45410889", "4e4d07d1", "4E4D87E6",
                                                        "4E4D081C", "585504c7", "585508eb", "58550156", "58410886", "4B4E0819", "5855029d", "5855013a", "5855042a", "585503ca",
                                                        "5841083C", "58410A96", "58550373", "484507D2", "4E4D07D8", "58410A06", "494607D6", "584108f0", "58550297", "58550328",
                                                        "4B5907E0", "413307D3", "585707F0", "58550162", "585503dc", "4d530805", "4D5388DE", "585501b5", "5855036e", "45410916",
                                                        "5855010c", "58410AA3", "58410964", "58410A22", "4D4B07D2", "585508ee", "58410846", "5A440004", "58550319", "5855048e" };

            titleIds.Clear();
            titleIds.Add("5454084E");
            DirectoryScanner ds = new DirectoryScanner();
            foreach (string t in titleIds)
            {
                Debug.WriteLine("=====================================================================================================================");
                Debug.WriteLine("TitleId: " + t);

 //               string u = ds.GetMarketPlaceURL(t);
 //               Debug.WriteLine("Url: " + u);

                XmlDocument x = null;
                try
                {   
                    Debug.Write("Getting XML: ");
   //                 x = ds.GetMarketPlaceXML(u);
                    Debug.WriteLine("OK");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("---> Exception getting marketplace xml for " + t + " - " + ex.Message);
                    continue;
                }

                GameAsset ga = null;

                try
                {
                    Debug.Write("Getting Assets: ");
   //                 ga = ds.GetAssetsFromMarketPlaceXML(x);
                    Debug.WriteLine(ga != null ? "OK" : "Not found!!!!");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("---> Exception getting assets for " + t + " - " + ex.Message);
                    continue;
                }

                if (ga == null) continue;
                
                try
                {
                    Debug.Write("Getting Image: ");
                    if (ga.DownloadImage().Ok)
                    {
                        Debug.WriteLine("OK");
                    }
                    else
                    {
                        Debug.WriteLine("Failed");
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("---> Exception getting image for " + t + " - " + ex.Message);
                    continue;
                }

                try
                {
                    Debug.Write("Saving assets: ");
                    if (ga.SaveGameAssets("t:\\test\\", true, true).Ok)
                    {
                        Debug.WriteLine("OK");
                    }
                    else
                    {
                        Debug.WriteLine("Failed");
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("---> Exception saving assets for " + t + " - " + ex.Message);
                    continue;
                }
            }
        }


        [TestMethod]
        public void DownloadImage()
        {
            GameAsset game = new GameAsset("12345678", 1, "1 vs 100", "http://avatar.xboxlive.com/global/t.584807e4/icon/0/8000");
            Assert.IsTrue(game.DownloadImage().Ok, "Download failed");
            // Assert.IsTrue(game.SaveTitleImage("t:", true), "Save image failed");
        }

        [TestMethod]
        public void SaveGameAssets()
        {
            //DirectoryScanner ds = new DirectoryScanner();
            //XmlDocument x = ds.GetMarketPlaceXML(ds.GetMarketPlaceURL("565507f2"));
            //ds.GetAssetsFromMarketPlaceXML(x);
            GameAsset game = new GameAsset("58410B5D", 1, "1 vs 100", "http://avatar.xboxlive.com/global/t.584807e4/icon/0/8000");
            game.SaveGameAssets("T:\\Media\\Software\\360_XBLA_Burnout.Crash.XBLA.XBOX360-MoNGoLS", true, true);
            
        }

        [TestMethod]
        public void SpecialDirs()
        {
            List<string> test = SpecialDirectories.GetSpecialDirectories();
            List<string> test2 = new List<string>() { "sdlgjk", "00080f000", "0000800d0", "00d0d0000", "sdjfh" };
            List<string> test3 = test2.Intersect(test, StringComparer.OrdinalIgnoreCase).ToList();
            List<string> test4 = SpecialDirectories.GetSpecialNames(test2);
        }

        [TestMethod]
        public void TestResultClass()
        {
            Result r = new Result();
            Assert.IsTrue(r.Ok);
            Console.WriteLine(r);
            Console.WriteLine("-------------");

            r.AddInfo("Here is some info");
            Assert.IsTrue(r.Ok);
            Assert.IsTrue(r.ContainsInfo);
            Assert.IsFalse(r.ContainsWarnings);
            Assert.IsFalse(r.ContainsErrors);
            Console.WriteLine(r);
            Console.WriteLine("-------------");

            r = new Result();
            r.AddWarning("Here is a warning");
            Assert.IsTrue(r.Ok);
            Assert.IsFalse(r.ContainsInfo);
            Assert.IsTrue(r.ContainsWarnings);
            Assert.IsFalse(r.ContainsErrors);
            Console.WriteLine(r);
            Console.WriteLine("-------------");

            r = new Result();
            r.AddError("Here is an error");
            Assert.IsFalse(r.Ok);
            Assert.IsFalse(r.ContainsInfo);
            Assert.IsFalse(r.ContainsWarnings);
            Assert.IsTrue(r.ContainsErrors);
            Console.WriteLine(r);
            Console.WriteLine("-------------");

            r.Entries.Clear();
            Assert.IsTrue(r.Ok);
            r.AddInfo("Here is some info");
            r.AddWarning("Here is a warning");
            r.AddError("Here is an error");
            Assert.IsFalse(r.Ok);
            Assert.IsTrue(r.ContainsInfo);
            Assert.IsTrue(r.ContainsWarnings);
            Assert.IsTrue(r.ContainsErrors);
            Console.WriteLine(r);
            Console.WriteLine("-------------");

            
        }

        [TestMethod]
        public void TestDesktopIniLines()
        {
            DesktopIni di = new DesktopIni();
            Console.WriteLine(di);

            di.Name = "name";
            Console.WriteLine(di);
            Assert.AreEqual("name", di.Name);

            di.InfoTip = "info";
            di.IconFile = "icon";
            di.NoNetworkSharing = true;
            Console.WriteLine(di);

            DesktopIni di2 = new DesktopIni("name", "icon", "info");
            Console.WriteLine(di2);
            Assert.AreEqual(di.Name, di2.Name);
            Assert.AreEqual(di.InfoTip, di2.InfoTip);
            Assert.AreEqual(di.IconFile, di2.IconFile);
            Assert.AreEqual(di.ConfirmFileOperation, di2.ConfirmFileOperation);
            Assert.AreNotEqual(di.NoNetworkSharing, di2.NoNetworkSharing);
        }

        [TestMethod]
        public void ScanDirShouldReturnGameAssets()
        {
            Result r = new Result();
            List<GameAsset> gameAssets = DirectoryScanner.GetGameAssets("T:\\Media\\Software\\360", out r);
            Assert.IsTrue(r.Ok);
            Assert.AreEqual(18, gameAssets.Count);
        }

        [TestMethod]
        public void GetDifferentBoxArt()
        {
            List<string> titleIds = new List<string>() { "54540890", "434607D6", "504807D1", "505407d2", "46530001", "584109B7", "4156081F", "584109A8", "584807E4", "584A07E4",
                                                        "584A07DC", "584A07D1", "58550440", "465607D4", "585502b2", "585502cc", "584108aa", "454108E4", "81312314", "58550222",
                                                        "58550189", "5855019b", "58410975", "5855029b", "585504b4", "58410841", "58410A3B", "5451083B", "54510852", "4D5309ED",
                                                        "4D530980", "97287552", "585501d6", "58550427", "585501c8", "585502b7", "585504a1", "5855017e", "5855037a", "584108db",
                                                        "5855012e", "58550219", "58550206", "58550180", "584109DB", "5855032b", "414B07D1", "58550166", "55530808", "58550378",
                                                        "5855012c", "585503fe", "585504b9", "585501a7", "58550431", "58550108", "465307da", "45410889", "4e4d07d1", "4E4D87E6",
                                                        "4E4D081C", "585504c7", "585508eb", "58550156", "58410886", "4B4E0819", "5855029d", "5855013a", "5855042a", "585503ca",
                                                        "5841083C", "58410A96", "58550373", "484507D2", "4E4D07D8", "58410A06", "494607D6", "584108f0", "58550297", "58550328",
                                                        "4B5907E0", "413307D3", "585707F0", "58550162", "585503dc", "4d530805", "4D5388DE", "585501b5", "5855036e", "45410916",
                                                        "5855010c", "58410AA3", "58410964", "58410A22", "4D4B07D2", "585508ee", "58410846", "5A440004", "58550319", "5855048e" };

            List<NameValue> parameters = new List<NameValue>()
            {
                new NameValue("Locale", ""),
                new NameValue("LegalLocale", ""),
                new NameValue("Store", "1"),
                new NameValue("PageSize", "100"),
                new NameValue("PageNum", "1"),
                new NameValue("DetailView", "3"), // 5
                new NameValue("OfferFilterLevel", "1"),
                new NameValue("MediaIds", ""),
                new NameValue("UserTypes", "2"),
                new NameValue("MediaTypes", "1"), //Xbox360
                new NameValue("MediaTypes", "21"),
                new NameValue("MediaTypes", "23"), //XBLA
                new NameValue("MediaTypes", "37"), //Community
                new NameValue("MediaTypes", "46")
            };

            List<string> locales = new List<string>() { "en-US", "ja-JP" };

            Parallel.ForEach<string>(titleIds, titleId =>

            //foreach (string titleId in titleIds)
            {
                foreach (string locale in locales)
                {
                    string url = "http://catalog.xboxlive.com/Catalog/Catalog.asmx/Query?methodName=FindGames";

                    List<NameValue> parameterOverrides = new List<NameValue>()
                    {
                        new NameValue("Locale", locale),
                        new NameValue("LegalLocale", locale),
                        new NameValue("MediaIds", "66acd000-77fe-1000-9115-d802" + titleId)
                    };

                    parameters.ForEach(p =>
                    {
                        NameValue param = parameterOverrides.Find(po => String.Compare(po.Name, p.Name, true) == 0);
                        param = param == null ? p : param;
                        url = String.Format("{0}&Names={1}&Values={2}", url, param.Name, param.Value);
                    });



                    //download
                    XmlDocument marketPlaceXML = new XmlDocument();

                    try
                    {

                        marketPlaceXML.Load(url);

                        XmlElement root = marketPlaceXML.DocumentElement;
                        XmlNamespaceManager xmlnsm = new XmlNamespaceManager(marketPlaceXML.NameTable);
                        xmlnsm.AddNamespace("default", "http://www.w3.org/2005/Atom");
                        xmlnsm.AddNamespace("live", "http://www.live.com/marketplace");

                        if (root.SelectSingleNode("live:totalItems/text()", xmlnsm).Value != "0")
                        {
                            string boxUrl = root.SelectSingleNode("default:entry/live:images/live:image[live:imageMediaType=14 and live:relationshipType=33 and live:size=23]/live:fileUrl/text()", xmlnsm).Value;
                            Debug.WriteLine(titleId + "\t" + locale + "\t" + boxUrl);

                            Image tmpImage = null;
                            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(boxUrl);
                            request.AllowWriteStreamBuffering = true;
                            request.Timeout = 4000;

                            WebResponse response = request.GetResponse();
                            Stream stream = response.GetResponseStream();
                            tmpImage = Image.FromStream(stream);

                            response.Close();
                            stream.Close();

                            tmpImage.Save("t:\\test\\" + titleId + "_" + locale + ".jpg");
                        }
                        else
                        {
                            Debug.WriteLine(titleId + "\t" + locale + "\tNOT FOUND (0 results)");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(titleId + "\t" + locale + "\t" + ex.Message);
                        Debug.WriteLine(titleId + "\t" + locale + "\t" + marketPlaceXML.InnerXml);
                    }
                }

            });
        }

        [TestMethod]
        public void IntegrationTest()
        {
            Result r = new Result();

            string path = "T:\\Media\\Software\\360";

            List<GameAsset> gameAssets = DirectoryScanner.GetGameAssets(path, out r);

            Parallel.ForEach<GameAsset>(gameAssets, ga => r.Append(ga.Process(path)));
        }

    }
}
