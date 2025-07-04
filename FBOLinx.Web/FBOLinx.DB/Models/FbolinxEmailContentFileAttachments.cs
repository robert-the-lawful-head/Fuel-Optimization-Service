﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FBOLinx.DB.Models
{
    public class FbolinxEmailContentFileAttachment
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public byte[] FileData { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public int EmailContentId { get; set; }
    }
}
