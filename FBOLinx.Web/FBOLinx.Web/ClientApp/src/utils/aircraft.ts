export const COMMERCIAL_AIRCRAFT_FLIGHT_NUMBER = [
    'ASA',
    'UPS',
    'SKW',
    'FDX',
    'UAL',
    'AAL',
    'DAL',
    'SWA',
    'GTI',
];

export const isCommercialAircraft = (flightNumber: string) : boolean =>
    COMMERCIAL_AIRCRAFT_FLIGHT_NUMBER.find((startNum) =>
        flightNumber.startsWith(startNum)
    ).length > 0;
