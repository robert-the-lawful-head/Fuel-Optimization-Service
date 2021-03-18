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
};
