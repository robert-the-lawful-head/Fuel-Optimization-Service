using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Models;

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
        public PricingTemplate.MarginTypes? MarginType { get; set; }
        public double? FboPrice { get; set; }
        public double? CustomerMarginAmount { get; set; }
        public bool NeedsAttention { get; set; }
        public string NeedsAttentionReason { get; set; }
        public string PricingTemplateName { get; set; }
        public Models.CustomerInfoByGroup.CertificateTypes? CertificateType { get; set; }
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

        public string CertificateTypeDescription
        {
            get
            {
                return Utilities.Enum.GetDescription(CertificateType ?? CustomerInfoByGroup.CertificateTypes.NotSet);
            }
        }
    }
}
