using System.Collections.Generic;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;

namespace FBOLinx.Web.ViewModels
{
    public class CustomersGridViewModel
    {
        public int CustomerId { get; set; }
        public int? CustomerInfoByGroupId { get; set; }
        public int? CompanyByGroupId { get; set; }
        public string Company { get; set; }
        public int? PricingTemplateId { get; set; }
        public int? DefaultCustomerType { get; set; }
        public bool? Suspended { get; set; }
        public int? FuelerLinxId { get; set; }
        public bool? Network { get; set; } = false;
        public int? GroupId { get; set; }
        public MarginTypes? MarginType { get; set; }
        public double? FboPrice { get; set; }
        public double? CustomerMarginAmount { get; set; }
        public bool NeedsAttention { get; set; }
        public string NeedsAttentionReason { get; set; }
        public string PricingTemplateName { get; set; }
        public CertificateTypes? CertificateType { get; set; }
        public double? MinGallons { get; set; }
        public double? MaxGallons { get; set; }
        public bool? IsFuelerLinxCustomer { get; set; }
        public int? CustomerCompanyType { get; set; }
        public string CustomerCompanyTypeName { get; set; }
        public bool HasBeenViewed { get; set; }
        public bool SelectAll { get; set; }
        public string TailNumbers { get; set; }

        public double? AllInPrice { get; set; }
        public bool IsPricingExpired { get; set; }
        public bool? ContactExists { get; set; }
        public int? FleetSize { get; set; }
        public bool? PricingTemplateExists { get; set; }
        public bool? PricingTemplateRemoved { get; set; }

        public bool? Active { get; set; }
        public int AircraftsVisits { get; set; }
        public string CertificateTypeDescription
        {
            get
            {
                return Core.Utilities.Enum.GetDescription(CertificateType ?? CertificateTypes.NotSet);
            }
        }

        public List<string> FuelVendors { get; set; }
        public ICollection<CustomerTag> Tags { get; set; }
        public string PricingFormula { get; set; }
        public List<CustomerGridContactsViewModel> Contacts { get; set; }
    }
    public class CustomerGridContactsViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
