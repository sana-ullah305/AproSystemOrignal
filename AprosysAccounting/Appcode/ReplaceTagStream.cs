using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AprosysAccounting.Appcode
{
    public class ReplaceTagsStream : MemoryStream
    {
        private readonly Stream _response;
        private StringBuilder _appendStr;

        public ReplaceTagsStream(Stream response)
        {
            _response = response;
            _appendStr = new StringBuilder();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _appendStr.Append(Encoding.UTF8.GetString(buffer, offset, count));
        }

        public override void Close()
        {
            var html = "";
            html = ReplaceTags(_appendStr.ToString());
            byte[] buffer = Encoding.UTF8.GetBytes(html);
            _response.Write(buffer, 0, buffer.Length);
        }

        private string ReplaceTags(string html)
        {
            var scriptRegex = "<script.+?src=[\"'](.+?)[\"'].*?>";
            var styleRegex = "<link.+?href=[\"'](.+?)[\"'].*?>";
            StringBuilder strBuilder = new StringBuilder(html);
            StringBuilder lastWrite = new StringBuilder();
            foreach (Match m in Regex.Matches(html, scriptRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline))
            {
                try
                {
                    lastWrite.Append(new System.IO.FileInfo(System.Web.HttpContext.Current.Server.MapPath(m.Groups[1].Value)).LastWriteTime.ToString("yyyyMMddhhmmss"));
                    strBuilder.Replace(m.Groups[1].Value, m.Groups[1].Value + "?v=" + lastWrite.ToString());
                }
                catch (Exception)
                {
                    continue;
                }
                finally
                {
                    lastWrite.Clear();
                }
            }

            foreach (Match m in Regex.Matches(html, styleRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline))
            {
                try
                {
                    if (!m.Groups[1].Value.Contains(".css")) { continue; }
                    lastWrite.Append(new System.IO.FileInfo(System.Web.HttpContext.Current.Server.MapPath(m.Groups[1].Value)).LastWriteTime.ToString("yyyyMMddhhmmss"));
                    strBuilder.Replace(m.Groups[1].Value, m.Groups[1].Value + "?v=" + lastWrite.ToString());
                }
                catch (Exception)
                {
                    continue;
                }
                finally
                {
                    lastWrite.Clear();
                }
            }

            return strBuilder.ToString();
        }

    }
}