export interface AcukwikAirport {
    oid: number;
    icao: string;
    iata: string;
    faa: string;
    fullAirportName: string;
    airportCity: string;
    stateSubdivision: string;
    country: string;
    airportType: string;
    distanceFromCity: string;
    latitude: string;
    longitude: string;
    elevation: number | null;
    variation: string;
    intlTimeZone: number | null;
    daylightSavingsYn: string;
    fuelType: string;
    airportOfEntry: string;
    customs: string;
    handlingMandatory: string;
    slotsRequired: string;
    open24Hours: string;
    controlTowerHours: string;
    approachList: string;
    primaryRunwayId: string;
    runwayLength: number | null;
    runwayWidth: number | null;
    lighting: string;
    airportNameShort: string;
    distanceToSelectedAirport: number;
    latitudeInDegrees: number;
    longitudeInDegrees: number;
    acukwikFbohandlerDetailCollection: AcukwikFbohandlerDetail[];
}

export interface AcukwikFbohandlerDetail {
    airportId: number | null;
    handlerId: number;
    handlerLongName: string;
    handlerType: string;
    handlerTelephone: string;
    handlerFax: string;
    handlerTollFree: string;
    handlerFreq: number | null;
    handlerFuelBrand: string;
    handlerFuelBrand2: string;
    handlerFuelSupply: string;
    handlerLocationOnField: string;
    multiService: string;
    avcard: string;
    acukwikId: number | null;
    acukwikAirport: AcukwikAirport;
}
