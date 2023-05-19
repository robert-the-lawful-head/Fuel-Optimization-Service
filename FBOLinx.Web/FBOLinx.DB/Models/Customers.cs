using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FBOLinx.Core.Enums;

namespace FBOLinx.DB.Models
{
    public class Customers : FBOLinxBaseEntityModel<int>
    {        
        public bool? Action { get; set; }
        public double? Margin { get; set; }
        [StringLength(255)]
        public string Company { get; set; }
        [StringLength(255)]
        public string Pilot { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Username { get; set; }
        [StringLength(255)]
        public string Password { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Joined { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
        public bool? Active { get; set; }
        [StringLength(255)]
        public string CheckFuelPrice { get; set; }
        [StringLength(255)]
        public string Lastlogin { get; set; }
        public bool? Distribute { get; set; }
        [Column("GroupID")]
        public int? GroupId { get; set; }
        public bool? Network { get; set; }
        [Column("FuelerlinxID")]
        public int? FuelerlinxId { get; set; }
        [StringLength(50)]
        public string MainPhone { get; set; }
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
        [StringLength(255)]
        public string Website { get; set; }
        public bool? ShowJetA { get; set; }
        [Column("Show100LL")]
        public bool? Show100Ll { get; set; }
        public bool? Suspended { get; set; }
        public CertificateTypes? CertificateType { get; set; }

        [InverseProperty("Customer")]
        public virtual CompaniesByGroup CompanyByGroup { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<CustomerInfoByGroup> CustomerInfoByGroup { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<CustomCustomerTypes> CustomCustomerType { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<FuelReq> FuelReqs { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<CustomerContacts> CustomerContacts { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<CompanyPricingLog> CompanyPricingLogs { get; set; }
    }
}
