//Create a ServiceOrder type that reflects the ServiceOrderDto

import { CustomerAircraft } from "./customer-aircraft";
import { CustomerInfoByGroup } from "./customer-info-by-group";
import { ServiceOrderItem } from "./service-order-item";
import { ServiceOrderAppliedDateTypes } from "../enums/service-order-applied-date-types";

export type ServiceOrder = {
    oid: number;
    fboId: number;
    groupId: number;
    customerInfoByGroupId: number;
    serviceDateTimeUtc?: Date;
    serviceDateTimeLocal?: Date;
    arrivalDateTimeUtc?: Date;
    arrivalDateTimeLocal?: Date;
    departureDateTimeUtc?: Date;
    departureDateTimeLocal?: Date;
    customerAircraftId: number;
    associatedFuelOrderId?: number;
    FuelerLinxTransactionId?: number;
    serviceOn?: ServiceOrderAppliedDateTypes;
    isCompleted: boolean;
    numberOfCompletedItems: number;

    serviceOrderItems: ServiceOrderItem[]
    customerInfoByGroup: CustomerInfoByGroup;
    customerAircraft: CustomerAircraft;
        
}
