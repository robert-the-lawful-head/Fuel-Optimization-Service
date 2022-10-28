import { FlightLegStatus } from "../enums/flight-watch.enum";

export interface Swim {
    tailNumber: string;
    flightDepartment: string;
    make: string;
    model: string;
    fAAMake: string;
    fAAModel: string;
    fuelCapacityGal: number | any;
    origin: string;
    city: string;
    departureICAO: string;
    departureCity: string;
    arrivalICAO: string;
    arrivalCity: string;
    atdLocal: string;
    atdZulu: string;
    etaLocal: string;
    etaZulu: string;
    ete: string;
    actualSpeed: number | any;
    altitude: number | any;
    latitude: number | any;
    longitude: number | any;
    isAircraftOnGround: boolean | any;
    itpMarginTemplate: string;
    status: FlightLegStatus | any;
    phone: string;
    visitsToMyFBO: number;
    arrivals: number;
    departures: number;
    id: number | any;
    fuelerlinxID: number | any;
    vendor: string;
    transactionStatus: string;
    icaoAircraftCode: string;
    isInNetwork: boolean | any;
    isOutOfNetwork: boolean | any;
    isActiveFuelRelease: boolean | any;
    isFuelerLinxClient: boolean | any;
}
export interface Time {
    ticks: number;
    days: number;
    hours: number;
    milliseconds: number;
    minutes: number;
    seconds: number;
    totalDays: number;
    totalHours: number;
    totalMilliseconds: number;
    totalMinutes: number;
    totalSeconds: number;
}
export const swimTableColumns = {
    status: 'status',
    tailNumber: 'tailNumber',
    flightDepartment: 'flightDepartment',
    icaoAircraftCode: 'icaoAircraftCode',
    ete: 'ete',
    eta: 'eta',
    atd: 'atd',
    origin: 'origin',
    arrival: 'arrivalICAO',
    isAircraftOnGround: 'isAircraftOnGround',
    itpMarginTemplate: 'itpMarginTemplate',
    etaAtdZulu: 'etaAtdZulu',
    expandedDetail: 'expandedDetail',
};

export const swimTableColumnsDisplayText = {
    status: 'Status',
    tailNumber: 'Tail Number',
    flightDepartment: 'Flight Department',
    icaoAircraftCode: 'Aircraft Type',
    ete: 'ETE',
    eta: 'ETA',
    atd: 'ATD',
    origin: 'Origin',
    arrivalICAO: 'Destination',
    isAircraftOnGround: 'On Ground',
    itpMarginTemplate: 'ITP Margin Template',
    expandedDetail: 'expandedDetail',
};

export const tailNumberTextColor = {
    fuelerLinx: '#1D497F',
    activeFuelRelease: '#0DC520',
    outOfNetwork: '#C1C1C1',
    inNetwork: '#FF7F00',
};
export const stautsTextColor = {
    EnRoute: 'black',
    Landing: 'blue',
    TaxiingDestination: '#FDD953',
    Arrived: 'green',
    Departing: 'gray',
    TaxiingOrigin: '#FDD953',
    default: 'black'
}

export interface MapMarkers {
    flights : MapMarkerInfo,
    fbos: MapMarkerInfo,
    airports: MapMarkerInfo
}
export interface MapMarkerInfo {
    sourceId: string,
    layerId: string,
    data: any[] | null
}
