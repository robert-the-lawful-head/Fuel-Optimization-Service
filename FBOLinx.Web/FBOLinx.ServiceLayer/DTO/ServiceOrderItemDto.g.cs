﻿using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO
{
    public class ServiceOrderItemDto
    {
        public long Oid { get; set; }
        public int ServiceOrderId { get; set; }
        [Required]
        [StringLength(255)]
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime? CompletionDateTimeUtc { get; set; }
        public int? CompletedByUserId { get; set; }
        [StringLength(255)]
        public string CompletedByName { get; set; }
        public int? AddedByUserId { get; set; }
        public string? AddedByName { get; set; }
        public string? ServiceNote { get; set; }
    }
}
