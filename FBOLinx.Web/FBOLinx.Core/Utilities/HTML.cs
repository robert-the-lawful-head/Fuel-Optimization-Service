using CoreHtmlToImage;

namespace FBOLinx.Core.Utilities
{
    public class HTML
    {
        public static byte[] CreateImageFromHTML(string html)
        {
            var htmlToImageConv = new HtmlConverter();
            return htmlToImageConv.FromHtmlString(html, 512, ImageFormat.Jpg);
        }
    }
}
