export enum FlightWatchStatus {
    Landing = 0,
    Takeoff = 1,
    Parking = 2,
}

export type FlightWatchHistorical = {
    company: string;
    dateTime: Date;
    tailNumber: string;
    flightNumber: string;
    hexCode: string;
    aircraftType: string;
    status: number;
    pastVisits: number;
    originated: string;
    isFuelerlinx: boolean;
};
