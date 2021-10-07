using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.Web.Models.Requests
{
    public class FboLogoResponse
    {
        public int Oid { get; set; }
        public int FboId { get; set; }
        public byte[] FileData { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
    }
}
