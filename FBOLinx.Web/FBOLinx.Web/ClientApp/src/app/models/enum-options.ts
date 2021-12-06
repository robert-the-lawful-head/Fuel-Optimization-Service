/* eslint-disable @typescript-eslint/no-namespace */

import { ApplicableTaxFlights } from '../enums/applicable-tax-flights';
import { FeeCalculationApplyingTypes } from '../enums/fee-calculation-applying-types';
import { FeeCalculationTypes } from '../enums/fee-calculation-types';
import { FlightTypeClassifications } from '../enums/flight-type-classifications';
import { StringFilterConditions } from '../enums/string-filter-conditions';

export namespace EnumOptions {
    export class EnumOption {
        public text: string;
        public value: any;
    }

    export const applicableTaxFlightOptions: Array<EnumOption> = [
        { text: 'Never', value: ApplicableTaxFlights.Never },
        { text: 'Domestic Only', value: ApplicableTaxFlights.DomesticOnly },
        {
            text: 'International Only',
            value: ApplicableTaxFlights.InternationalOnly,
        },
        { text: 'All Flights', value: ApplicableTaxFlights.All },
    ];

    export const strictApplicableTaxFlightOptions: Array<EnumOption> = [
        { text: 'Domestic', value: ApplicableTaxFlights.DomesticOnly },
        {
            text: 'International',
            value: ApplicableTaxFlights.InternationalOnly,
        },
    ];

    export const flightTypeClassificationOptions: Array<EnumOption> = [
        { text: 'Private', value: FlightTypeClassifications.Private },
        { text: 'Commercial', value: FlightTypeClassifications.Commercial },
        { text: 'All Types', value: FlightTypeClassifications.All },
    ];

    export const strictFlightTypeClassificationOptions: Array<EnumOption> = [
        { text: 'Private', value: FlightTypeClassifications.Private },
        { text: 'Commercial', value: FlightTypeClassifications.Commercial },
    ];

    export const feeCalculationTypeOptions: Array<EnumOption> = [
        { text: 'Flat Per Gallon', value: FeeCalculationTypes.FlatPerGallon },
        { text: 'Percentage', value: FeeCalculationTypes.Percentage },
    ];

    export const feeCalculationApplyingTypeOptions: Array<EnumOption> = [
        {
            text: 'Above the Line',
            value: FeeCalculationApplyingTypes.PreMargin,
        },
        {
            text: 'Below the Line',
            value: FeeCalculationApplyingTypes.PostMargin,
        },
    ];

    export const stringFilterConditionOptions: Array<EnumOption> = [
        {
            text: 'Contains',
            value: StringFilterConditions.Contains,
        },
        {
            text: 'Does Not Contain',
            value: StringFilterConditions.DoesNotContain,
        }
    ];
}
