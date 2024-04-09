export type OrderNote = {
    oid: number;
    associatedFuelOrderId?: number;
    associatedServiceOrderId?: number;
    associatedFuelerLinxTransactionId?: number;
    note?: string;
    dateAdded?: Date;
    addedByUserId?: number;
    addedByName?: string;
    timeZone?: string;
    isEdit?: boolean;
}
