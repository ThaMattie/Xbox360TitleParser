using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.Text;

namespace Xbox360TitleParser.Common
{
    public enum Locale { en_US, en_GB };

    public class TitleIdValidator
    {
        public static bool IsValid(string titleId)
        {
            Regex titleIdRegex = new Regex(@"^[A-Fa-f0-9]{8}$");
            return titleIdRegex.IsMatch(titleId);
        }
    }

    public class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public NameValue(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    public class Result
    {
        public enum ResultType { Error, Warning, Info };
        
        public class ResultEntry
        {
            public ResultType Type { get; set; }
            public string Entry { get; set; }

            public ResultEntry(ResultType type, string entry)
	        {
                this.Type = type;
                this.Entry = entry;
            }
            
            public override string ToString()
            {
                return String.Format("[{0}]\t{1}", this.Type, this.Entry);
            }
        }

        public List<ResultEntry> Entries { get; private set; }

        public Result()
        {
            this.Entries = new List<ResultEntry>();
        }

        public bool Ok { get { return !this.ContainsErrors; } private set { } }

        private bool contains(ResultType type) { return (this.Entries.FindAll(e => e.Type == type).Count > 0); }

        public bool ContainsInfo { get { return (this.contains(ResultType.Info)); } private set { } }
        public bool ContainsWarnings { get { return (this.contains(ResultType.Warning)); } private set { } }
        public bool ContainsErrors { get { return (this.contains(ResultType.Error)); } private set { } }

        public void AddInfo(string info) { this.Entries.Add(new ResultEntry(ResultType.Info, info)); }
        public void AddWarning(string warning) { this.Entries.Add(new ResultEntry(ResultType.Warning, warning)); }
        public void AddError(string error) { this.Entries.Add(new ResultEntry(ResultType.Error, error)); }

        public void Append(Result result)
        {
            this.Entries.AddRange(result.Entries);
        }

        public override string ToString()
        {
            return ToString(false, false, false);
        }

        public string ToString(bool error)
        {
            return ToString(error, false, false);
        }

        public string ToString(bool error, bool warning)
        {
            return ToString(error, warning, false);
        }

        public string ToString(bool error, bool warning, bool info)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Result: {0}", this.Ok ? "OK" : "NOT OK"));

            this.Entries.FindAll(e => (e.Type == ResultType.Error && error) || (e.Type == ResultType.Warning && warning) || (e.Type == ResultType.Info && info)).ForEach(e => sb.AppendLine(e.ToString()));

            return sb.ToString();
        }
    }
}