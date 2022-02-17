using System;
using FBOLinx.Core.Enums;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CustomerInfoByGroupDto
    {
        public int GroupId { get; set; }
        public int CustomerId { get; set; }
        public string Company { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? Joined { get; set; }
        public bool? Active { get; set; }
        public bool? Distribute { get; set; }
        public bool? Network { get; set; }
        public string MainPhone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
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
        public CustomersDto Customer { get; set; }
        public int Oid { get; set; }
    }
}