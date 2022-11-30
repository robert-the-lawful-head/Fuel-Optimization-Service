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
    status: FlightLegStatus;
    statusDisplayString: string;
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
    faaRegisteredOwner: string;
}
export const swimTableColumns = {
    status: 'status',
    tailNumber: 'tailNumber',
    flightDepartment: 'flightDepartment',
    icaoAircraftCode: 'icaoAircraftCode',
    ete: 'ete',
    eta: 'etaLocal',
    atd: 'atdLocal',
    originAirport: 'arrivalICAO',
    originCity: 'arrivalCity',
    makeModel: 'makeModel',
    destinationAirport: 'departureICAO',
    destinationCity: 'departureCity',
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
    etaLocal: 'ETA',
    atdLocal: 'ATD',
    departureICAO: 'Origin Airport',
    departureCity: 'Origin City',
    arrivalICAO: 'Destination Airport',
    arrivalCity: 'Destination City',
    isAircraftOnGround: 'On Ground',
    itpMarginTemplate: 'ITP Margin Template',
    makeModel: 'Make/Model',
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
    flightsReversed : MapMarkerInfo,
    fbos: MapMarkerInfo,
    airports: MapMarkerInfo
}
export interface MapMarkerInfo {
    sourceId: string,
    layerId: string,
    data: any[] | null
}
