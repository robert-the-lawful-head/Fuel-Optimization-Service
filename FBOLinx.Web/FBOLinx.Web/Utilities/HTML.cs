using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CoreHtmlToImage;


namespace FBOLinx.Web.Utilities
{
    public class HTML
    {
        public static byte[] CreateImageFromHTML(string html)
        {
            var htmlToImageConv = new HtmlConverter();
            return htmlToImageConv.FromHtmlString(html, 1024, ImageFormat.Jpg);
        }
    }
}
