using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360TitleParser.Common
{
    public class DesktopIni
    {
        private Dictionary<string, string> desktopIniLines = new Dictionary<string, string>()
        {
            { "[.ShellClassInfo]", null },
            { "ConfirmFileOp", "0" },
            { "NoSharing", "0" },
            { "IconFile", "" },
            { "IconIndex", "0" },
            { "LocalizedResourceName", "" },
            { "InfoTip", "" }
        };

        public DesktopIni() { }

        public DesktopIni(string name)
        {
            this.Name = name;
        }

        public DesktopIni(string name, string infoTip) : this(name)
        {
            this.InfoTip = infoTip;
        }

        public DesktopIni(string name, string iconFile, string infoTip) : this(name, infoTip)
        {
            this.IconFile = iconFile;
        }

        public bool ConfirmFileOperation
        {
            get { return desktopIniLines["ConfirmFileOp"] == "1"; }
            set { desktopIniLines["ConfirmFileOp"] = value ? "1" : "0"; }
        }

        public bool NoNetworkSharing
        {
            get { return desktopIniLines["NoSharing"] == "1"; }
            set { desktopIniLines["NoSharing"] = value ? "1" : "0"; }
        }

        public string Name
        {
            get { return desktopIniLines["LocalizedResourceName"]; }
            set { desktopIniLines["LocalizedResourceName"] = value; }
        }

        public string IconFile
        {
            get { return desktopIniLines["IconFile"]; }
            set { desktopIniLines["IconFile"] = value; }
        }

        public string InfoTip
        {
            get { return desktopIniLines["InfoTip"]; }
            set { desktopIniLines["InfoTip"] = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> line in desktopIniLines)
            {
                if (line.Value != null)
                {
                    sb.Append(line.Key).Append("=").AppendLine(line.Value);
                }
                else
                {
                    sb.AppendLine(line.Key);
                }
            }
            return sb.ToString();
        }

    }
}
