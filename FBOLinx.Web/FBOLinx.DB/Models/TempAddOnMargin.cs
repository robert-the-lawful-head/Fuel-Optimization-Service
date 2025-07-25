﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class TempAddOnMargin
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("fboId")]
        public int FboId { get; set; }
        [Column("effectiveFrom")]
        public DateTime EffectiveFrom { get; set; }
        [Column("effectiveTo")]
        public DateTime EffectiveTo { get; set; }
        [Column("marginJet")]
        public double? MarginJet { get; set; }
        [Column("marginAvgas")]
        public double? MarginAvgas { get; set; }
    }
}
