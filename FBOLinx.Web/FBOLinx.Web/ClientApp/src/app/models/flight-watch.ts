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
    fuelOrder?: boolean;
    isFuelerLinxCustomer: boolean;
    tailNumber: string;
    company : string;
    aircraftMakeModel : string;
    lastQuote : string;
    currentPricing : string;
};

export type Aircraftwatch = {
    customerInfoBygGroupId: number;
    tailNumber: string;
    atcFlightNumber: string;
    aircraftTypeCode: string;
    isAircraftOnGround: boolean;
    company : string;
    aircraftMakeModel : string;
    lastQuote : string;
    currentPricing : string;
};
