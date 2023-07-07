export type CustomerInfoByGroupNote = {
    oid: number;
    customerInfoByGroupId: number;
    fboId?: number;
    notes: string;
    lastUpdatedUtc?: Date;
    lastUpdatedByUserId?: number;
}
