using System.Text.RegularExpressions;
using CoreHtmlToImage;

namespace FBOLinx.Core.Utilities
{
    public class HTML
    {
        public static byte[] CreateImageFromHTML(string html, int width)
        {
            var htmlToImageConv = new HtmlConverter();
            return htmlToImageConv.FromHtmlString(html, width, ImageFormat.Png);
        }

        public static string UseRegularExpression(string input)
        {
            var result = Regex.Replace(input, "<.*?>", string.Empty);
            result = Regex.Replace(result, @"\t|\n|\r", string.Empty);
            return result;
        }
    }
}
