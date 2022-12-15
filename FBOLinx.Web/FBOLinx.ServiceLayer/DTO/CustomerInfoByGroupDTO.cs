using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using FBOLinx.Core.Enums;

namespace FBOLinx.ServiceLayer.DTO
{
    public class CustomerInfoByGroupDTO : BaseEntityModelDTO<DB.Models.CustomerInfoByGroup>, IEntityModelDTO<DB.Models.CustomerInfoByGroup, int>
    {
        public int Oid { get; set; }
        public int GroupId { get; set; }
        public int CustomerId { get; set; }
        [StringLength(255)]
        public string Company { get; set; }
        [StringLength(255)]
        public string Username { get; set; }
        [StringLength(255)]
        public string Password { get; set; }
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
        public bool? Show100Ll { get; set; }
        public bool? Suspended { get; set; }
        public int? DefaultTemplate { get; set; }
        public int? CustomerType { get; set; }
        public bool? EmailSubscription { get; set; }
        public string Sfid { get; set; }
        public CertificateTypes? CertificateType { get; set; }
        public int? CustomerCompanyType { get; set; }
        public bool? PricingTemplateRemoved { get; set; }
        public bool? MergeRejected { get; set; }

        public CustomerDTO Customer { get; set; }
    }
}
