using System;
using System.Collections.Generic;
using System.Linq;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.DTO
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
        public PriceBreakdownDisplayTypes? PriceBreakdownDisplayType { get; set; }
        
        public double? BasePrice
        {
            get
            {
                double result = GetPreMarginSubTotal();
                result = GetSubtotalWithMargin(result);
                return result;
            }
        }

        public double? AllInPrice
        {
            get
            {
                return GetAllInPrice();
            }
        }

        public string CertificateTypeDescription
        {
            get
            {
                return Core.Utilities.Enum.GetDescription(CertificateType ?? CertificateTypes.NotSet);
            }
        }

        public List<FboFeesAndTaxes> FeesAndTaxes { get; set; }

        #region Private Methods
        private double GetAllInPrice()
        {
            //Start by calculating the total before the margin including pre-margin taxes and fees
            double result = GetPreMarginSubTotal();

            //Next apply the margin to the pre-margin subtotal
            result = GetSubtotalWithMargin(result);

            //Finally add the post-margin fees and taxes
            result = GetPostMarginTotal(result);

            return result;
        }

        private double GetPreMarginSubTotal()
        {
            if (!MarginType.HasValue || MarginType == MarginTypes.RetailMinus || MarginType == MarginTypes.FlatFee)
                return (FboPrice.GetValueOrDefault());
            double result = FboPrice.GetValueOrDefault();
            double basePrice = FboPrice.GetValueOrDefault();
            if (FeesAndTaxes == null)
                return result;
            
            foreach (var feeAndTax in FeesAndTaxes.Where(x => x.WhenToApply == FBOLinx.Core.Enums.FeeCalculationApplyingTypes.PreMargin && !x.IsOmitted).OrderBy(x => x.CalculationType == FBOLinx.Core.Enums.FeeCalculationTypes.Percentage ? 1 : 2).ThenBy(x => x.CalculationType == FBOLinx.Core.Enums.FeeCalculationTypes.FlatPerGallon ? 1 : 2))
            {
                result += feeAndTax.GetCalculatedValue(basePrice, result);
            }

            return result;
        }

        private double GetSubtotalWithMargin(double preMarginSubTotal)
        {
            double result = 0;
            double itp = 0;
            if (!MarginType.HasValue)
                result = 0;

            else if (MarginType.Value == MarginTypes.CostPlus)
            {
                if (DiscountType.GetValueOrDefault() == DiscountTypes.Percentage)
                {
                    itp = FboPrice.GetValueOrDefault() * (Math.Abs(CustomerMarginAmount.GetValueOrDefault()) / 100);
                    result = (preMarginSubTotal + itp);
                }
                else
                {
                    result = (preMarginSubTotal + Math.Abs(CustomerMarginAmount.GetValueOrDefault()));
                }
            }
            else if (MarginType.Value == MarginTypes.RetailMinus)
            {
                if (DiscountType.GetValueOrDefault() == DiscountTypes.Percentage)
                {

                    itp = FboPrice.GetValueOrDefault() * (Math.Abs(CustomerMarginAmount.GetValueOrDefault()) / 100);
                    result = (preMarginSubTotal - itp);
                }
                else
                {
                    result = (preMarginSubTotal - Math.Abs(CustomerMarginAmount.GetValueOrDefault()));
                }
            }
            return result;
        }

        private double GetPostMarginTotal(double subTotalWithMargin)
        {
            double result = subTotalWithMargin;

            if (FeesAndTaxes == null)
                return result;

            foreach (var feeAndTax in FeesAndTaxes.Where(x => x.WhenToApply == FBOLinx.Core.Enums.FeeCalculationApplyingTypes.PostMargin && !x.IsOmitted).OrderBy(x => x.CalculationType == FBOLinx.Core.Enums.FeeCalculationTypes.Percentage ? 1 : 2).ThenBy(x => x.CalculationType == FBOLinx.Core.Enums.FeeCalculationTypes.FlatPerGallon ? 1 : 2))
            {
                result += feeAndTax.GetCalculatedValue(subTotalWithMargin, result);
            }

            return result;
        }
        #endregion
    }
}
