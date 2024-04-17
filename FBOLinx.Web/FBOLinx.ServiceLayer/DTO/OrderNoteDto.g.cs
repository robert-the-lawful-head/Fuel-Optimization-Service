using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Service.Mapping.Dto;
using Mapster;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.Core.Enums;

namespace FBOLinx.ServiceLayer.DTO
{
    public class OrderNoteDto
    {
        public int Oid { get; set; }
        public int AssociatedFuelOrderId { get; set; }
        public int AssociatedServiceOrderId { get; set; }
        public int AssociatedFuelerLinxTransactionId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateAdded { get; set; }
        public string? Note { get; set; }
        [Column("AddedByUserID")]
        public int? AddedByUserId { get; set; }
        [StringLength(255)]
        public string? AddedByName { get; set; }
        public string? TimeZone { get; set; }
    }
}