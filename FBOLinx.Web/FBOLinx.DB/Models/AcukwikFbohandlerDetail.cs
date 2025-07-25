﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    [Table("AcukwikFBOHandlerDetail")]
    public partial class AcukwikFbohandlerDetail
    {
        [Column("Airport_ID")]
        public int? AirportId { get; set; }
        [Key]
        [Column("Handler_ID")]
        public int HandlerId { get; set; }
        [StringLength(255)]
        public string HandlerLongName { get; set; }
        [StringLength(255)]
        public string HandlerType { get; set; }
        [StringLength(255)]
        public string HandlerTelephone { get; set; }
        [StringLength(255)]
        public string HandlerFax { get; set; }
        [StringLength(255)]
        public string HandlerTollFree { get; set; }
        public double? HandlerFreq { get; set; }
        [StringLength(255)]
        public string HandlerFuelBrand { get; set; }
        [StringLength(255)]
        public string HandlerFuelBrand2 { get; set; }
        [StringLength(255)]
        public string HandlerFuelSupply { get; set; }
        [StringLength(255)]
        public string HandlerLocationOnField { get; set; }
        [StringLength(255)]
        public string MultiService { get; set; }
        [StringLength(255)]
        public string Avcard { get; set; }
        [Column("Acukwik_ID")]
        public double? AcukwikId { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        public string HandlerEmail { get; set; }

        #region Relationships
        [ForeignKey("AirportId")]
        [InverseProperty("AcukwikFbohandlerDetailCollection")]
        public AcukwikAirport AcukwikAirport { get; set; }
        #endregion
    }
}