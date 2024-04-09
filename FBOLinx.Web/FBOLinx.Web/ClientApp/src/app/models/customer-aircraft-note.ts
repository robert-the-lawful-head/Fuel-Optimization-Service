export type CustomerAircraftNote = {
    oid: number;
    customerAircraftId: number;
    fboId?: number;
    notes: string;
    lastUpdatedUtc?: Date;
    lastUpdatedByUserId?: number;
}
