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
    }
}
