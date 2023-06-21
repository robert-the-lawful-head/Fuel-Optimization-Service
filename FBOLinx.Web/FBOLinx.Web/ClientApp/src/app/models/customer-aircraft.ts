import {AircraftSizes } from '../enums/aircraft-sizes';

export type CustomerAircraft = {
    oid: number;
    groupId: number;
    customerId: number;
    aircraftId: number;
    tailNumber: string;
    size?: AircraftSizes;
    basedPagLocation?: string;
    networkCode?: string;
    addedFrom?: number;
};
