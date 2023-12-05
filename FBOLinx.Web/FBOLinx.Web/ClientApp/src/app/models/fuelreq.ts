//Create a FuelReq type that reflects the FuelReqDto

import { Customer } from "./customer"
import { CustomerAircraft } from "./customer-aircraft"
import { ServiceOrderItem } from "./service-order-item"

export type FuelReq = {
    oid: number;
    customerId: number;
    fboId: number;
    customerInfoByGroupId: number;
    arrivalDateTimeLocal?: Date;
    departureDateTimeLocal?: Date;
    eta?: Date;
    etd?: Date;
    dateCreated?: Date;
    icao: string;
    customerAircraftId: number;
    timeStandard: string;
    cancelled?: boolean;
    quotedVolume?: number;
    quotedPpg?: number;
    notes: string;
    actualVolume?: number;
    actualPpg?: number;
    source: string;
    sourceId?: number;
    dispatchNotes: string;
    archived?: boolean;
    email: string;
    phoneNumber: string;
    fuelOn: string;
    customerName: string;
    customerNotes: string;
    paymentMethod: string;
    timeZone: string;
    isConfirmed?: boolean;
    tailNumber: string;
    fboName: string;
    pricingTemplateName: string;

    serviceOrderItems: ServiceOrderItem[]
    customer: Customer;
    customerAircraft: CustomerAircraft;
    //fbo: 
    //FuelReqPricingTemplate
}
