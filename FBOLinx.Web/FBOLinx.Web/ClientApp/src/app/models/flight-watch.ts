export enum AircraftStatusType {
    Landing = 0,
    Takeoff = 1,
    Parking = 2,
};

export type FlightWatch = {
    oid: number;
    boxTransmissionDateTimeUtc: Date;
    aircraftHexCode: string;
    atcFlightNumber: string;
    altitudeInStandardPressure?: number;
    groundSpeedKts?: number;
    trackingDegree?: number;
    latitude: number;
    longitude: number;
    verticalSpeedKts?: number;
    transponderCode?: number;
    boxName: string;
    aircraftPositionDateTimeUtc: Date;
    aircraftTypeCode: string;
    gpsAltitude?: number;
    isAircraftOnGround: boolean;
    aircraftStatus: AircraftStatusType;
};
