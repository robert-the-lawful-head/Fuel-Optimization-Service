using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.Dto.Responses
{
    public class PricingTemplateGrid
    {
        public int Oid { get; set; }
        public string Name { get; set; }
        public int Fboid { get; set; }
        public int? CustomerId { get; set; }
        public bool? Default { get; set; }
        public string Notes { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public short? Type { get; set; }
        public MarginTypes? MarginType { get; set; }

        public string MarginTypeDescription
        {
            get { return FBOLinx.Core.Utilities.Enum.GetDescription(MarginType ?? MarginTypes.CostPlus); }
        }
        public double Margin { get; set; }
        public double? IntoPlanePrice { get; set; }
        public bool IsInvalid { get; set; }
        public bool IsPricingExpired { get; set; }
        public double? YourMargin { get; set; }
        public int? CustomersAssigned { get; set; }
        public int? EmailContentId { get; set; }
        public EmailContent EmailContent { get; set; }
        public List<string> CustomerEmails { get; set; }
        public DiscountTypes? DiscountType { get; set; }
        public double InitialAmount { get; set; }
        public string PricingFormula { get; set; }
    }
}
