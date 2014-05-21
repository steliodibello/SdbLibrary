using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using HtmlAgilityPack;
using SdbBackbone.Utils.Xml;

namespace SdbBackbone.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string target, string value, StringComparison comparison)
        {
            return target.IndexOf(value, comparison) >= 0;
        }

        /// <summary>Cleans the html string and removes the body tag if present</summary>
        /// <param name="html">The html</param>
        /// <param name="errors">Retuns error when parsing the html. If html is valid, returns an empty IEnumerable</param>
        /// <returns>The cleaned html</returns>
        public static string CleanHtml(this string html, out IEnumerable<string> errors)
        {
            var doc = new HtmlDocument {OptionWriteEmptyNodes = true};
            doc.LoadHtml(html);
            errors = doc.ParseErrors != null && doc.ParseErrors.Any()
                         ? doc.ParseErrors.Select(e => e.ToString())
                         : Enumerable.Empty<string>();
            return doc.DocumentNode.SelectSingleNode("//body") != null
                       ? doc.DocumentNode.SelectSingleNode("//body").InnerHtml
                       : doc.DocumentNode.InnerHtml;
        }

        public static string FormatWith(this string str, params object[] args)
        {
            return String.Format(str, args);
        }

        public static bool IsEmpty(this string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        public static string RemoveHtmlTags(this string text)
        {
            return Regex.Replace(text ?? string.Empty, "</?[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     A case insenstive replace function.
        /// </summary>
        /// <param name="originalString">The string to examine.(HayStack)</param>
        /// <param name="oldValue">The value to replace.(Needle)</param>
        /// <param name="newValue">The new value to be inserted</param>
        /// <returns>A string</returns>
        public static string CaseInsenstiveReplace(string originalString, string oldValue, string newValue)
        {
            var regEx = new Regex(oldValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(originalString, newValue);
        }

        public static string StripForUrl(this string title)
        {
            string url = title;

            url.RemoveHtmlTags();
            url = HttpUtility.HtmlDecode(url);

            //get rid of URI escapes
            url = Regex.Replace(url, "%[a-fA-F0-9]{2}", "", RegexOptions.Compiled);

            //get rid of HTML escapes
            url = Regex.Replace(url, "&(\\#{1}\\d*|\\w*);", "", RegexOptions.Compiled);

            //go to town on special characters
            url = Regex.Replace(url, "[~`!@#$%^*()<>?,.\\|/:;'\"\\\\]", "", RegexOptions.Compiled);

            //get rid of all non-ascii characters
            url = Regex.Replace(url, "[^\\x20-\\x7E]", "", RegexOptions.Compiled);

            //All theese lines are covered by otehr lines in this function
            //strOutput = Regex.Replace(strOutput, "</?[^>]*>", "", RegexOptions.Compiled) ' Removes any HTML tags from the text
            //strOutput = Regex.Replace(strOutput, "[^\w \s\./-]", "", RegexOptions.Compiled)
            //strOutput = Regex.Replace(strOutput, "\s", "-", RegexOptions.Compiled)
            //strOutput = Regex.Replace(strOutput, "\.", "", RegexOptions.Compiled)
            //strOutput = Regex.Replace(strOutput, "/.", "", RegexOptions.Compiled)

            //replace ampersands with the word 'and', providing they're surrounded by white space
            url = Regex.Replace(url, "\\s&\\s", " and ", RegexOptions.Compiled);

            //replace all other ampersands with hyphens
            url = Regex.Replace(url, "&", "-", RegexOptions.Compiled);

            //get rid of groups of spaces (ie 2 or more in a row)
            url = Regex.Replace(url, "\\s{2,}", "", RegexOptions.Compiled);

            //replace spaces with dashes
            url = url.Trim();
            url = Regex.Replace(url, "\\s", "-", RegexOptions.Compiled);
            url = Regex.Replace(url, "-{2,}", "-", RegexOptions.Compiled);

            const int maxLength = 100;
            if (url.Length > maxLength)
            {
                if (url.IndexOf("-", StringComparison.Ordinal) > maxLength)
                {
                    url = url.Substring(0, maxLength);
                }
                else
                {
                    string strTemp = "";

                    foreach (string word in url.Split('-'))
                    {
                        if (strTemp.Length + word.Length > maxLength)
                        {
                            break;
                        }

                        if (strTemp.Length == 0)
                        {
                            strTemp += word;
                        }
                        else
                        {
                            strTemp += "-" + word;
                        }
                    }

                    url = strTemp;
                }
            }

            return url;
        }

        public static string EscapeSingleQuotes(this string value)
        {
            return value.Replace("'", "\\u0027");
        }

        public static string UnescapeSingleQuotes(this string value)
        {
            return value.Replace("\\u0027", "'");
        }

        public static string Decode(this string value)
        {
            return WebUtility.HtmlDecode(value);
        }

        public static Stream ToStream(this string @this)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@this);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Deserialize an XML string to an object of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>(this string value)
        {
            var serializer = new XmlSerializer(typeof (T));
            var reader = new NamespaceIgnorantXmlTextReader(new StringReader(value));  //XmlReader.Create(value.Trim().ToStream());
            return (T) serializer.Deserialize(reader);
        }

        public static string SerializeXML<T>(this string value, T obj)
        {
            var serializer = new XmlSerializer(typeof (T));
            var stream = new MemoryStream();
            using (var tw = new XmlTextWriter(stream, System.Text.Encoding.UTF8))
            {
                serializer.Serialize(stream, obj);
                return System.Text.Encoding.UTF8.GetString(stream.ToArray());   
            }
        }
         
    }
}