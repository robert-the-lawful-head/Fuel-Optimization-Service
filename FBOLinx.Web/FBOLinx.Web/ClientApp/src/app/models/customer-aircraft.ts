import { AircraftSizes } from '../enums/aircraft-sizes';
import { CustomerAircraftNote } from './customer-aircraft-note';

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
    notes?: CustomerAircraftNote[];
};
