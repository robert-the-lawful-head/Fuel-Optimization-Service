//Create a ServiceOrderItem type that reflects the ServiceOrderItemDto
export type ServiceOrderItem = {
    oid: number;
    serviceOrderId: number;
    serviceName: string;
    quantity: number;
    isCompleted: boolean;
    completionDateTimeUtc: Date;
}
