using System.Collections.Generic;
using FBOLinx.Core.Enums;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FboFeesAndTaxesDto
    {
        public int Oid { get; set; }
        public int Fboid { get; set; }
        public string Name { get; set; }
        public FeeCalculationTypes? CalculationType { get; set; }
        public double Value { get; set; }
        public FlightTypeClassifications? FlightTypeClassification { get; set; }
        public ApplicableTaxFlights? DepartureType { get; set; }
        public FeeCalculationApplyingTypes? WhenToApply { get; set; }
        public bool IsOmitted { get; set; }
        public string OmittedFor { get; set; }
        public List<FboFeeAndTaxOmitsByCustomerDto> OmitsByCustomer { get; set; }
        public List<FboFeeAndTaxOmitsByPricingTemplateDto> OmitsByPricingTemplate { get; set; }
    }
}