import { ApplicableTaxFlights } from '../enums/applicable-tax-flights';
import { FlightTypeClassifications } from '../enums/flight-type-classifications';
import { FeeCalculationTypes } from '../enums/fee-calculation-types';

export namespace EnumOptions {

  export class EnumOption {
    public text: string;
    public value: any;
  }

  export const applicableTaxFlightOptions: Array<EnumOption> = [
    { text: "Never", value: ApplicableTaxFlights.Never },
    { text: "Domestic Only", value: ApplicableTaxFlights.DomesticOnly },
    { text: "International Only", value: ApplicableTaxFlights.InternationalOnly },
    { text: "All Flights", value: ApplicableTaxFlights.All }
  ];

  export const strictApplicableTaxFlightOptions: Array<EnumOption> = [
    { text: "Domestic", value: ApplicableTaxFlights.DomesticOnly },
    { text: "International", value: ApplicableTaxFlights.InternationalOnly },
  ];

  export const flightTypeClassificationOptions: Array<EnumOption> = [    
    { text: "Private", value: FlightTypeClassifications.Private },
    { text: "Commercial", value: FlightTypeClassifications.Commercial },
    { text: "All Types", value: FlightTypeClassifications.All }
  ];

  export const strictFlightTypeClassificationOptions: Array<EnumOption> = [
    { text: "Private", value: FlightTypeClassifications.Private },
    { text: "Commercial", value: FlightTypeClassifications.Commercial }
  ]

  export const feeCalculationTypeOptions: Array<EnumOption> = [
    { text: "Flat Per Gallon", value: FeeCalculationTypes.FlatPerGallon },
    { text: "Percentage of Base", value: FlightTypeClassifications.Private },
    { text: "Percentage of All-In", value: FlightTypeClassifications.Commercial }
  ];
}
