using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.Web.Models.Requests
{
    public class FboLogoRequest
    {
        [Required]
        public int FboId { get; set; }
        public string FileData { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
    }
}
