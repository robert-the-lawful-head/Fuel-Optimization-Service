//Create a ServiceOrder type that reflects the ServiceOrderDto

import { CustomerAircraft } from "./customer-aircraft";
import { CustomerInfoByGroup } from "./customer-info-by-group";
import { ServiceOrderItem } from "./service-order-item";

export type ServiceOrder = {
    oid: number;
    fboId: number;
    groupId: number;
    customerInfoByGroupId: number;
    serviceDateTimeUtc: Date;
    serviceDateTimeLocal: Date;
    customerAircraftId: number;
    associatedFuelOrderId: number;
    numberOfCompletedItems: number;

    serviceOrderItems: ServiceOrderItem[]
    customerInfoByGroup: CustomerInfoByGroup;
    customerAircraft: CustomerAircraft;
}
