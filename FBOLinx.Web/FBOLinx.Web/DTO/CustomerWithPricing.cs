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
        public int? DefaultCustomerType { get; set; }
        public bool? Suspended { get; set; }
        public int? FuelerLinxId { get; set; }
        public bool? Network { get; set; } = false;
        public int? GroupId { get; set; }
        public int FboId { get; set; }
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

        public double? BasePrice
        {
            get
            {
                double result = 0;
                if (!MarginType.HasValue)
                    result = 0;
                else if (MarginType.Value == PricingTemplate.MarginTypes.CostPlus)
                    result = (FboPrice.GetValueOrDefault() + Math.Abs(CustomerMarginAmount.GetValueOrDefault()));
                else if (MarginType.Value == PricingTemplate.MarginTypes.RetailMinus)
                    result = (FboPrice.GetValueOrDefault() - Math.Abs(CustomerMarginAmount.GetValueOrDefault()));
                else if (MarginType.Value == PricingTemplate.MarginTypes.FlatFee)
                    result = Math.Abs(CustomerMarginAmount.GetValueOrDefault());
                return result;
            }
        }

        public double? AllInPrice
        {
            get
            {
                double result = 0;
                if (!MarginType.HasValue)
                    result = 0;
                else if (MarginType.Value == PricingTemplate.MarginTypes.CostPlus)
                    result = (FboPrice.GetValueOrDefault() + Math.Abs(CustomerMarginAmount.GetValueOrDefault()));
                else if (MarginType.Value == PricingTemplate.MarginTypes.RetailMinus)
                    result = (FboPrice.GetValueOrDefault() - Math.Abs(CustomerMarginAmount.GetValueOrDefault()));
                else if (MarginType.Value == PricingTemplate.MarginTypes.FlatFee)
                    result = Math.Abs(CustomerMarginAmount.GetValueOrDefault());

                if (FeesAndTaxes == null)
                    return result;

                //Calculate the fee totals, adding in flat first, then percentage of base, then percentage of total after the others have been totalled
                double resultWithBaseFees = result;
                foreach(var feeAndTax in FeesAndTaxes.Where(x => x.CalculationType != Enums.FeeCalculationTypes.PercentageOfTotal).OrderBy(x => x.CalculationType == Enums.FeeCalculationTypes.PercentageOfBase ? 1 : 2).ThenBy(x => x.CalculationType == Enums.FeeCalculationTypes.FlatPerGallon ? 1 : 2))
                {
                    resultWithBaseFees += feeAndTax.GetCalculatedValue(result, resultWithBaseFees);
                }

                double resultWithAllFees = resultWithBaseFees;
                foreach(var feeAndTax in FeesAndTaxes.Where(x => x.CalculationType == Enums.FeeCalculationTypes.PercentageOfTotal))
                {
                    resultWithAllFees += feeAndTax.GetCalculatedValue(result, resultWithBaseFees);
                }

                return resultWithAllFees;
            }
        }

        public string CertificateTypeDescription
        {
            get
            {
                return Utilities.Enum.GetDescription(CertificateType ?? CustomerInfoByGroup.CertificateTypes.NotSet);
            }
        }

        public List<FboFeesAndTaxes> FeesAndTaxes { get; set; }
    }
}
