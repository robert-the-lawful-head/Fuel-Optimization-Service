export enum FeeAndTaxBreakdownDisplayModes {
    PriceTaxBreakdown = 0,
    CustomerOmitting = 1,
    PricingPanel = 2,
}

export enum OmmitedFor {
    Customer = 'C',
    PricingTemplate = 'P',
}

export enum PriceBreakdownDisplayTypes {
    SingleColumnAllFlights = 0,
    TwoColumnsDomesticInternationalOnly = 1,
    TwoColumnsApplicableFlightTypesOnly = 2,
    FourColumnsAllRules = 3,
}