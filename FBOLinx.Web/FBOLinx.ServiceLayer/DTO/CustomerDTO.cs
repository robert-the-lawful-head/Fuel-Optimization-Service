using System;
using System.Collections.Generic;
using FBOLinx.DB.Models;
using System.ComponentModel.DataAnnotations;
using FBOLinx.Core.Enums;

namespace FBOLinx.ServiceLayer.DTO
{
    public class CustomerDTO : BaseEntityModelDTO<DB.Models.Customers>, IEntityModelDTO<DB.Models.Customers, int>
    {
        public int Oid { get; set; }
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
        public DateTime? Joined { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
        public bool? Active { get; set; }
        [StringLength(255)]
        public string CheckFuelPrice { get; set; }
        [StringLength(255)]
        public string Lastlogin { get; set; }
        public bool? Distribute { get; set; }
        public int? GroupId { get; set; }
        public bool? Network { get; set; }
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
        public bool? Show100Ll { get; set; }
        public bool? Suspended { get; set; }
        public CertificateTypes? CertificateType { get; set; }

        public virtual ICollection<CustomerInfoByGroup> CustomerInfoByGroup { get; set; }


    }
}
