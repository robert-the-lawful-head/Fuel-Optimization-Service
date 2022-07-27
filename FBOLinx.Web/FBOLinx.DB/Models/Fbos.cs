using FBOLinx.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    [Table("FBOs")]
    public partial class Fbos
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [StringLength(255)]
        public string Fbo { get; set; }
        [Column("GroupID")]
        public int? GroupId { get; set; }
        [StringLength(50)]
        public string Username { get; set; }
        [StringLength(50)]
        public string Password { get; set; }
        public double? PostedRetail { get; set; }
        public double? TotalCost { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateActivated { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(10)]
        public string State { get; set; }
        [StringLength(15)]
        public string ZipCode { get; set; }
        [StringLength(255)]
        public string Country { get; set; }
        [Column("TotalCost100LL")]
        public double? TotalCost100Ll { get; set; }
        [Column("PostedRetail100LL")]
        public double? PostedRetail100Ll { get; set; }
        public bool? Suspended { get; set; }
        public bool? GroupMargin { get; set; }
        public bool? ApplyGroupMargin { get; set; }
        public short? GroupMarginSetting { get; set; }
        public bool? GroupMarginFuture { get; set; }
        public short? GroupMarginTemplate { get; set; }
        public double? GroupMarginMargin { get; set; }
        public MarginTypes? GroupMarginType { get; set; }
        //public short? GroupMarginType { get; set; }
        [Column("GroupMargin100LLMargin")]
        public double? GroupMargin100Llmargin { get; set; }
        [Column("GroupMargin100LLType")]
        public short? GroupMargin100Lltype { get; set; }
        public string FuelDeskEmail { get; set; }
        public string Website { get; set; }
        [StringLength(50)]
        public string MainPhone { get; set; }
        [StringLength(50)]
        public string Extension { get; set; }
        public short? DefaultMarginTypeJetA { get; set; }
        [Column("DefaultMarginType100LL")]
        public MarginTypes? DefaultMarginType100Ll { get; set; }
        //public short? DefaultMarginType100Ll { get; set; }
        public bool? SalesTax { get; set; }
        public bool? ApplySalesTax { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        public bool? PriceUpdateReminderPrompt { get; set; }
        public bool? PriceUpdateNeverPrompt { get; set; }
        [StringLength(50)]
        public string Currency { get; set; }
        [Required]
        public bool? InitialSetupPhase { get; set; }
        public bool DisableCost { get; set; }
        [Column("AcukwikFBOHandlerID")]
        public int? AcukwikFBOHandlerId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string SenderAddress
        {
            get
            {
                if (_SenderAddress == null || _SenderAddress == "") return "DoNotReply";
                else return _SenderAddress;
            }
            set { _SenderAddress = value; }
        }
        private string _SenderAddress;
        public string ReplyTo {
            get
            {
                if (_ReplyTo == null || _ReplyTo == "") return FuelDeskEmail;
                else return _ReplyTo;
            }
            set { _ReplyTo = value; }
        }
        private string _ReplyTo;

        public AccountTypes AccountType { get; set; } = AccountTypes.RevFbo;

        public string AntennaName { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("Fbos")]
        public Group Group { get; set; }
        
        [InverseProperty("Fbo")]
        public Fboairports FboAirport { get; set; }

        [InverseProperty("Fbo")]
        public ICollection<FuelReq> FuelReqs { get; set; }

        [InverseProperty("Fbo")]
        public ICollection<PricingTemplate> PricingTemplates { get; set; }

        [InverseProperty("Fbo")]
        public Models.Fbopreferences Preferences { get; set; }

        [InverseProperty("Fbo")]
        public ICollection<Fbocontacts> Contacts { get; set; }

        [InverseProperty("Fbo")]
        public ICollection<User> Users { get; set; }

        [InverseProperty("Fbo")]
        public ICollection<Fboprices> Fboprices { get; set; }
    }
}
