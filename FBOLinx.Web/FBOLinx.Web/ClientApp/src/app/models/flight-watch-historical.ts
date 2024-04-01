import { AirportWatchHistoricalParking } from "./airport-watch-historical-parking";

export enum FlightWatchStatus {
    Landing = 0,
    Takeoff = 1,
    Parking = 2,
}

export type FlightWatchHistorical = {
    airportWatchHistoricalDataId: number;
    customerInfoByGroupID: number;
    companyId: number;
    company: string;
    dateTime: Date;
    tailNumber: string;
    flightNumber: string;
    hexCode: string;
    aircraftType: string;
    aircraftTypeCode: string;
    status: number | string;
    pastVisits: number;
    originated: string;
    isFuelerlinx: boolean;
    visitsToMyFbo: number;
    percentOfVisits: number;
    isConfirmedVisit: boolean;
    airportWatchHistoricalParking: AirportWatchHistoricalParking;
    parkingAcukwikFBOHandlerId: number | null;
};

export type AirportWatchHistoricalDataRequest = {
    startDateTime?: Date;
    endDateTime?: Date;
};
