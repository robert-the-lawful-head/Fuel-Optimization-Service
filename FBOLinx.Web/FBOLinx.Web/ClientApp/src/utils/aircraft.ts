export const COMMERCIAL_AIRCRAFT_FLIGHT_NUMBER = [
    'AAL',
    'AAY',
    'ACA',
    'AIJ',
    'AMF',
    'AMX',
    'ANA',
    'ASA',
    'ASH',
    'ATN',
    'AWI',
    'BAW',
    'DAL',
    'DLH',
    'EDV',
    'ENY',
    'FDX',
    'FFT',
    'GEC',
    'HAL',
    'JBU',
    'JIA',
    'JSX',
    'KLM',
    'NKS',
    'PDT',
    'POE',
    'QTR',
    'QXE',
    'ROU',
    'RPA',
    'SCX',
    'SKW',
    'SWA',
    'SWG',
    'TSC',
    'UAE',
    'UAL',
    'UCA',
    'UPS',
    'VIV',
    'VOI',
    'WJA'
];

export const isCommercialAircraft = (flightNumber: string) : boolean =>
    COMMERCIAL_AIRCRAFT_FLIGHT_NUMBER.some((startNum) =>
        flightNumber?.toUpperCase().startsWith(startNum)
    );

export const isCommercialAircraftInFlightNumbers = (flightNumbers: string[]): boolean => {
    if (!flightNumbers)
        return false;
    if (flightNumbers.length == 0)
        return false;
    var matches = COMMERCIAL_AIRCRAFT_FLIGHT_NUMBER.filter((startNum) => (flightNumbers.some(f => f.toUpperCase().startsWith(startNum))));
    return (matches && matches.length > 0);
}
     
