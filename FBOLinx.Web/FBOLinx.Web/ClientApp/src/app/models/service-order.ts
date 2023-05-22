//Create a ServiceOrder type that reflects the ServiceOrderDto

import { ServiceOrderItem } from "./service-order-item";

export type ServiceOrder = {
    oid: number;
    fboId: number;
    groupId: number;
    customerId: number;
    serviceDateTimeUtc: Date;
    tailNumber: string;
    associatedFuelOrderId: number;
    numberOfCompletedItems: number;

    serviceOrderItems: ServiceOrderItem[]
}
