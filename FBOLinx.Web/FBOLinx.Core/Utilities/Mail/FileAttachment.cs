using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.Core.Utilities.Mail
{
    public class FileAttachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FileData { get; set; }
    }
}
