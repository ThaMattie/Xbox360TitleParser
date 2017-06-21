using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Xbox360TitleParser.Common
{
    [DebuggerDisplay("GameAsset [{TitleId}] {Title}")]
    public class GameAsset
    {
        private static string baseMarketPlaceURL = "http://catalog.xboxlive.com/Catalog/Catalog.asmx/Query?methodName=FindGames";

        private static List<NameValue> parameters = new List<NameValue>()
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


        private DesktopIni desktopIni = null;

        public List<string> ContentTypes { get; private set; }
        public string TitleId { get; private set; }
        public string Title { get; private set; }
        public string ImageURL { get; private set; }
        public Image TitleImage { get; private set; }
        public int MediaType { get; private set; }

        public GameAsset(string titleId) : this(titleId, new List<string>()) { }

        public GameAsset(string titleId, List<string> contentTypes)
        {
            if (String.IsNullOrEmpty(titleId)) throw new ArgumentException("Argument cannot be Null or Empty", "titleId");
            if (contentTypes == null) throw new ArgumentNullException("Argument cannot be Null", "contentTypes");

            if (!TitleIdValidator.IsValid(titleId)) throw new ArgumentException("Argument is invalid format", "titleId");

            this.TitleId = titleId;
            this.ContentTypes = contentTypes;
        }

        public GameAsset(string titleId, int mediaType, string title, string imageURL)
            : this(titleId)
        {
            if (String.IsNullOrEmpty(title)) throw new ArgumentException("Argument cannot be Null or Empty", "title");
            if (String.IsNullOrEmpty(imageURL)) throw new ArgumentException("Argument cannot be Null or Empty", "imageURL");

            this.Title = title;
            this.ImageURL = imageURL;
            this.MediaType = mediaType;

            this.TitleImage = null;
        }

        public Result DownloadImage()
        {
            Result r = new Result();

            Image tmpImage = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.ImageURL);
                request.AllowWriteStreamBuffering = true;
                request.Timeout = 4000;

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                tmpImage = Image.FromStream(stream);

                response.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                // Error
                r.AddError("Exception caught in process: " + ex.Message);
                return r;
            }

            this.TitleImage = tmpImage;

            return r;
        }

        private Result saveTitleImage(string dir)
        {
            return saveTitleImage(dir, false);
        }

        private Result saveTitleImage(string dir, bool overwrite)
        {
            if (String.IsNullOrEmpty(dir)) throw new ArgumentException("Argument cannot be Null or Empty", "dir");
            if (!Directory.Exists(dir)) throw new ArgumentException("Directory does not exist", "dir");

            Result r = new Result();

            if (this.TitleImage == null)
            {
                r.AddError("TitleImage is null");
                return r;
            }

            string path = Path.Combine(dir, "title.bmp");
            r.AddInfo("TitleImage path set to: " + path);

            if (File.Exists(path) && !overwrite)
            {
                r.AddError("File already exists");
                return r;
            }

            try
            {
                new Bitmap(this.TitleImage).Save(path, ImageFormat.Bmp);
                if (File.Exists(path)) File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
            }
            catch (Exception ex)
            {
                r.AddError("Exception saving TitleImage: " + ex.Message);
                return r;
            }

            return r;
        }

        public Result SaveGameAssets(string dir)
        {
            return SaveGameAssets(dir, false);
        }

        public Result SaveGameAssets(string dir, bool createSubDir)
        {
            return SaveGameAssets(dir, createSubDir, false);
        }

        public Result SaveGameAssets(string dir, bool createSubDir, bool overwrite)
        {
            if (String.IsNullOrEmpty(dir)) throw new ArgumentException("Argument cannot be Null or Empty", "dir");
            if (!Directory.Exists(dir)) throw new ArgumentException("Directory does not exist", "dir");

            Result r = new Result();

            if (createSubDir) dir = Path.Combine(dir, this.TitleId);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            r.Append(saveTitleImage(dir, true));

            if (!r.Ok) return r;

            string desktopIniFile = Path.Combine(dir, "desktop.ini");

            if (File.Exists(desktopIniFile) && !overwrite)
            {
                r.AddError(String.Format("File '{0}' already exists and Overwrite == false", desktopIniFile));
                return r;
            }

            string infoTip = ""; //this.TitleId;
            if (this.ContentTypes.Count > 0)
            {
                //infoTip += " {";
                infoTip += "{";
                for (int i = 0; i < this.ContentTypes.Count; i++)
                {
                    infoTip += (i > 0 ? " + " : "") + this.ContentTypes[i];
                }
                infoTip += "}";
            }

            DesktopIni desktopIni = new DesktopIni(this.Title, "title.bmp", infoTip);

            try
            {
                if (File.Exists(desktopIniFile)) File.SetAttributes(desktopIniFile, File.GetAttributes(desktopIniFile) & ~FileAttributes.System & ~FileAttributes.Hidden);
                File.WriteAllText(desktopIniFile, desktopIni.ToString(), Encoding.Unicode);
                File.SetAttributes(desktopIniFile, File.GetAttributes(desktopIniFile) | FileAttributes.System | FileAttributes.Hidden);
                File.SetAttributes(dir, File.GetAttributes(dir) | FileAttributes.System);
                DirectoryInfo di = new DirectoryInfo(dir);
                di.Attributes = di.Attributes | FileAttributes.System;
            }
            catch (Exception ex)
            {
                r.AddError("Exception saving game asset files - " + ex.Message);
            }
            return r;
        }


        private string getMarketPlaceURL(string titleId)
        {
            return this.getMarketPlaceURL(titleId, Locale.en_US);
        }

        private string getMarketPlaceURL(string titleId, Locale locale)
        {
            if (String.IsNullOrEmpty(titleId)) throw new ArgumentException("Argument cannot be Null or Empty", "titleId");
            if (!TitleIdValidator.IsValid(titleId)) throw new ArgumentException("Argument is invalid format", "titleId");

            List<NameValue> parameterOverrides = new List<NameValue>()
            {
                new NameValue("Locale", locale.ToString().Replace("_", "-")),
                new NameValue("LegalLocale", locale.ToString().Replace("_", "-")),
                new NameValue("MediaIds", "66acd000-77fe-1000-9115-d802" + titleId)
            };

            string url = baseMarketPlaceURL;
            parameters.ForEach(p =>
            {
                NameValue param = parameterOverrides.Find(po => String.Compare(po.Name, p.Name, true) == 0);
                param = param == null ? p : param;
                url = String.Format("{0}&Names={1}&Values={2}", url, param.Name, param.Value);
            });

            return url;
        }

        private XmlDocument getMarketPlaceXML(string marketPlaceURL)
        {
            if (String.IsNullOrEmpty(marketPlaceURL)) throw new ArgumentException("Argument cannot be Null or Empty", "marketPlaceURL");

            XmlDocument marketPlaceXML = new XmlDocument();
            marketPlaceXML.Load(marketPlaceURL);

            return marketPlaceXML;
        }


        public Result Process(string path)
        {
            Result r = new Result();

            XmlDocument marketPlaceXML = this.getMarketPlaceXML(this.getMarketPlaceURL(this.TitleId));

            XmlElement root = marketPlaceXML.DocumentElement;
            XmlNamespaceManager xmlnsm = new XmlNamespaceManager(marketPlaceXML.NameTable);
            xmlnsm.AddNamespace("default", "http://www.w3.org/2005/Atom");
            xmlnsm.AddNamespace("live", "http://www.live.com/marketplace");

            if (root.SelectSingleNode("live:totalItems/text()", xmlnsm).Value == "0")
            {
                r.AddWarning(String.Format("No assets found for TitleId '{0}'", this.TitleId));
                return r;
            }

            this.Title = root.SelectSingleNode("default:entry/default:title/text()", xmlnsm).Value;
            r.AddInfo(String.Format("title: {0}", this.Title));

            this.ImageURL = root.SelectSingleNode("default:entry/live:images/live:image[live:imageMediaType=14 and (live:relationshipType=23 or live:relationshipType=15)]/live:fileUrl/text()", xmlnsm).Value;
            r.AddInfo(String.Format("icon: {0}", this.ImageURL));

            //string mediaType = root.SelectSingleNode("default:entry/live:media/live:mediaType/text()", xmlnsm).Value;
            //r.AddInfo(String.Format("mediaType: {0}", mediaType));

            //string reducedTitle = root.SelectSingleNode("default:entry/live:media/live:reducedTitle/text()", xmlnsm).Value;
            //r.AddInfo(String.Format("reducedTitle: {0}", reducedTitle));

            //string category = root.SelectSingleNode("default:entry/live:categories/live:category[last()]/live:categoryId/text()", xmlnsm).Value;
            //category += " / " + root.SelectSingleNode("default:entry/live:categories/live:category[last()]/live:system/text()", xmlnsm).Value;
            //category += " / " + root.SelectSingleNode("default:entry/live:categories/live:category[last()]/live:name/text()", xmlnsm).Value;
            //r.AddInfo(String.Format("category: {0}", category));
            
            r.Append(this.DownloadImage());
            r.Append(this.SaveGameAssets(path, true, true));
            
            return r;
        }
    }
}
