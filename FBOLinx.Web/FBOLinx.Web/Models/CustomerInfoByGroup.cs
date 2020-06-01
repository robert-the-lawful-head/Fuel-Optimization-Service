using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    public partial class CustomerInfoByGroup
    {
        public enum CertificateTypes : short
        {
            [Description("Not set")]
            NotSet = 0,
            [Description("Part 91")]
            Part91 = 91,
            [Description("Part 121")]
            Part121 = 121,
            [Description("Part 135")]
            Part135 = 135
        }

        //public enum CustomerCompanyTypes
        //{
        //    [Description("Flight Department")]
        //    FlightDepartment = 0,
        //    [Description("Contract Fuel Vendor")]
        //    ContractFuelVendor = 1,
        //    [Description("Base")]
        //    Base = 2,
        //    [Description("CAA")]
        //    CAA = 3,
        //    [Description("Fue")]
        //}

        [Column("GroupID")]
        public int GroupId { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [StringLength(255)]
        public string Company { get; set; }
        [StringLength(255)]
        public string Username { get; set; }
        [StringLength(255)]
        public string Password { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Joined { get; set; }
        public bool? Active { get; set; }
        public bool? Distribute { get; set; }
        public bool? Network { get; set; }
        [StringLength(100)]
        public string MainPhone { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(10)]
        public string State { get; set; }
        [StringLength(11)]
        public string ZipCode { get; set; }
        [StringLength(255)]
        public string Country { get; set; }
        [StringLength(255)]
        public string Website { get; set; }
        public bool? ShowJetA { get; set; }
        [Column("Show100LL")]
        public bool? Show100Ll { get; set; }
        public bool? Suspended { get; set; }
        public int? DefaultTemplate { get; set; }
        public Customers.CustomerSources? CustomerType { get; set; }
        public bool? EmailSubscription { get; set; }
        [Column("SFID")]
        public string Sfid { get; set; }
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public CertificateTypes? CertificateType { get; set; }
        public int? CustomerCompanyType { get; set; }
        public bool? PricingTemplateRemoved { get; set; }

        [InverseProperty("CustomerInfoByGroup")]
        [ForeignKey("CustomerId")]
        public Customers Customer { get; set; }
    }
}
