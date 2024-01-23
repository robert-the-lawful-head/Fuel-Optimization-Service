//Create a ServiceOrderItem type that reflects the ServiceOrderItemDto
export type ServiceOrderItem = {
    oid: number;
    serviceOrderId: number;
    serviceName: string;
    quantity: number;
    isCompleted: boolean;
    completionDateTimeUtc?: Date;
    completedByUserId?: number;
    completedByName?: string;
    isEditMode: boolean;
    isAddMode: boolean;
    isAdding: boolean;
    addedByUserId?: number;
    addedByName?: string;
    serviceNote?: string;
}
