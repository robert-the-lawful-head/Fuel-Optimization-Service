using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Models;

namespace FBOLinx.Web.DTO
{
    public class CustomerWithPricing
    {
        public int CustomerId { get; set; }
        public int? CustomerInfoByGroupId { get; set; }
        public int? CompanyByGroupId { get; set; }
        public string Company { get; set; }
        public int? PricingTemplateId { get; set; }
        public Customers.CustomerSources? DefaultCustomerType { get; set; }
        public bool? Suspended { get; set; }
        public int? FuelerLinxId { get; set; }
        public bool? Network { get; set; } = false;
        public int? GroupId { get; set; }
        public PricingTemplate.MarginTypes? MarginType { get; set; }
        public double? FboPrice { get; set; }
        public double? CustomerMarginAmount { get; set; }
        public double? FboFeeAmount { get; set; }
        public bool NeedsAttention { get; set; }
        public string PricingTemplateName { get; set; }
        public Models.CustomerInfoByGroup.CertificateTypes? CertificateType { get; set; }
        public double? MinGallons { get; set; }
        public double? MaxGallons { get; set; }
        public int? CustomerCompanyType { get; set; }
        public string CustomerCompanyTypeName { get; set; }
        public bool HasBeenViewed { get; set; }
        public bool IsPricingExpired { get; set; }

        public double? AllInPrice
        {
            get
            {
                if (!MarginType.HasValue)
                    return 0;
                if (MarginType.Value == PricingTemplate.MarginTypes.CostPlus)
                    return (FboPrice.GetValueOrDefault() + Math.Abs(CustomerMarginAmount.GetValueOrDefault())) * (1 + (FboFeeAmount / 100.0));
                if (MarginType.Value == PricingTemplate.MarginTypes.RetailMinus)
                    return (FboPrice.GetValueOrDefault() - Math.Abs(CustomerMarginAmount.GetValueOrDefault()));
                if (MarginType.Value == PricingTemplate.MarginTypes.FlatFee)
                    return Math.Abs(CustomerMarginAmount.GetValueOrDefault());
                return 0;
            }
        }

        public string DefaultCustomerTypeDescription
        {
            get
            {
                return Utilities.Enum.GetDescription(DefaultCustomerType ?? Customers.CustomerSources.NotSpecified);
            }
        }

        public string CertificateTypeDescription
        {
            get
            {
                return Utilities.Enum.GetDescription(CertificateType ?? CustomerInfoByGroup.CertificateTypes.NotSet);
            }
        }
    }
}
