export enum FlightWatchStatus {
    Landing = 0,
    Takeoff = 1,
    Parking = 2,
}

export type FlightWatchHistorical = {
    companyId: number;
    company: string;
    dateTime: Date;
    tailNumber: string;
    flightNumber: string;
    hexCode: string;
    aircraftType: string;
    aircraftTypeCode: string;
    status: number;
    pastVisits: number;
    originated: string;
    isFuelerlinx: boolean;
};

export type AirportWatchHistoricalDataRequest = {
    startDateTime?: Date;
    endDateTime?: Date;
};
