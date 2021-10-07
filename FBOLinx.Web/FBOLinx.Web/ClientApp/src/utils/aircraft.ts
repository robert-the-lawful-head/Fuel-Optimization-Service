export const COMMERCIAL_AIRCRAFT_TYPE_CODES = ['A3', 'A5'];
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

export const isCommercialAircraft = (type: string, flightNumber: string) =>
    COMMERCIAL_AIRCRAFT_TYPE_CODES.includes(type) ||
    COMMERCIAL_AIRCRAFT_FLIGHT_NUMBER.find((startNum) =>
        flightNumber.startsWith(startNum)
    );
