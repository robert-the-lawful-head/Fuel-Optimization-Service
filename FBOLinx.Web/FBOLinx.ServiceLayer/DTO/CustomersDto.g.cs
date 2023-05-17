using System;
using System.Collections.Generic;
using FBOLinx.Core.Enums;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CustomersDto
    {
        public bool? Action { get; set; }
        public double? Margin { get; set; }
        public string Company { get; set; }
        public string Pilot { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? Joined { get; set; }
        public string Notes { get; set; }
        public bool? Active { get; set; }
        public string CheckFuelPrice { get; set; }
        public string Lastlogin { get; set; }
        public bool? Distribute { get; set; }
        public int? GroupId { get; set; }
        public bool? Network { get; set; }
        public int? FuelerlinxId { get; set; }
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
        public CertificateTypes? CertificateType { get; set; }
        public CompaniesByGroupDto CompanyByGroup { get; set; }
        public List<CustomerInfoByGroupDto> CustomerInfoByGroup { get; set; }
        public CustomCustomerTypesDto CustomCustomerType { get; set; }
        public ICollection<FuelReqDto> FuelReqs { get; set; }
        public ICollection<CustomerContactsDto> CustomerContacts { get; set; }
        public ICollection<CompanyPricingLogDto> CompanyPricingLogs { get; set; }
        public ICollection<CustomerAircraftsDto> CustomerAircrafts { get; set; }
        public int Oid { get; set; }
    }
}