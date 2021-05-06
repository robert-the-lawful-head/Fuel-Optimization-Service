using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FBOLinx.Core.Utilities.Data.Cleaners
{
    public class StringCleaner
    {
        public static Regex InvalidCharactersRegex = new Regex(@"[^\w\.@-\\%*/\]() ]");

        public static string CleanInvalidCharacters(string input)
        {
            return InvalidCharactersRegex.Replace(input, "");
        }

        public static string CleanInvalidFilePathCharacters(string filePath)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            invalid += "#";

            foreach (char c in invalid)
            {
                filePath = filePath.Replace(c.ToString(), "");
            }
            return filePath;
        }

        public static string SeparateWordsByUpperCase(string stringToSeparate)
        {
            return String.Join(" ", System.Text.RegularExpressions.Regex.Split(stringToSeparate.Replace(" ", ""), @"(?<!^)(?=[A-Z])"));
        }

        public static string StripHTMLFromString(string html)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            string noHTML = Regex.Replace(html, @"<[^>]+>|&nbsp;", "").Trim();
            return Regex.Replace(noHTML, @"\s{2,}", " ");
        }

        //public static string CleanHTML(string html, string[] safeTags, string startIndexHTML = "", string endIndexHTML = "")
        //{
        //    if (string.IsNullOrEmpty(html))
        //        return "";

        //    int startIndex = string.IsNullOrEmpty(startIndexHTML) ? 0 : html.IndexOf(startIndexHTML) + startIndexHTML.Length;
        //    int endIndex = string.IsNullOrEmpty(endIndexHTML) ? html.Length - 1 : html.IndexOf(endIndexHTML);
        //    NSoup.Safety.Whitelist whiteList = new NSoup.Safety.Whitelist();
        //    whiteList.AddTags(safeTags);
        //    if (startIndex > 0 && endIndex > 0)
        //    {
        //        html = html.Substring(startIndex, endIndex - startIndex);
        //        html = html.Replace("\n", "").Replace("&nbsp;", "");
        //        html = NSoupClient.Clean(html, whiteList);
        //    }

        //    return html;
        //}
    }
}
