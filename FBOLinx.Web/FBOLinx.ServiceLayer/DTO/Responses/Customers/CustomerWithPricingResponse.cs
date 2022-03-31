using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.Responses.Customers
{
    public class CustomerWithPricingResponse
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
        public int FboId { get; set; }
        public MarginTypes? MarginType { get; set; }
        public DiscountTypes? DiscountType { get; set; }
        public double? FboPrice { get; set; }
        public double? CustomerMarginAmount { get; set; }
        public bool NeedsAttention { get; set; }
        public double? amount { get; set; }
        public string PricingTemplateName { get; set; }
        public CertificateTypes? CertificateType { get; set; }
        public double? MinGallons { get; set; }
        public double? MaxGallons { get; set; }
        public int? CustomerCompanyType { get; set; }
        public string CustomerCompanyTypeName { get; set; }
        public bool HasBeenViewed { get; set; }
        public bool IsPricingExpired { get; set; }
        public string TailNumbers { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Icao { get; set; }
        public string Iata { get; set; }
        public string Notes { get; set; }
        public string Fbo { get; set; }
        public string Group { get; set; }
        public string FuelDeskEmail { get; set; }
        public string CopyEmails { get; set; }
        public string Product { get; set; }
        //public PriceDistributionService.PriceBreakdownDisplayTypes? PriceBreakdownDisplayType { get; set; }

        //public double? BasePrice
        //{
        //    get
        //    {
        //        double result = GetPreMarginSubTotal();
        //        result = GetSubtotalWithMargin(result);
        //        return result;
        //    }
        //}

        public double? AllInPrice { get; set; }

        public string CertificateTypeDescription
        {
            get
            {
                return Core.Utilities.Enum.GetDescription(CertificateType ?? CertificateTypes.NotSet);
            }
        }

        public List<FboFeesAndTaxes> FeesAndTaxes { get; set; }
    }
}
