export interface Swim {
    tailNumber: string;
    flightDepartment: string;
    make: string;
    model: string;
    fuelCapacityGal: number | null;
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
    actualSpeed: number | null;
    altitude: number | null;
    latitude: number | null;
    longitude: number | null;
    isAircraftOnGround: boolean;
    itpMarginTemplate: string;
    status: FlightLegStatusEnum;
    phone: string;
    visitsToMyFBO: number;
    arrivals: number;
    departures: number;
    id: number | null;
    fuelerlinxID: number | null;
    vendor: string;
    transactionStatus: string;
    icaoAircraftCode: string;
    isInNetwork: boolean;
    isOutOfNetwork: boolean;
    isActiveFuelRelease: boolean;
    isFuelerLinxClient: boolean;
}
export enum FlightLegStatusEnum {
    EnRoute,
    Landing,
    TaxiingDestination,
    Arrived,
    Departing,
    TaxiingOrigin
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

export enum SwimType {
    Arrival ,
    Departure
  }

export const swimTableColumns = {
       status:
       'Status',
        tailNumber:
       'Tail Number',
        flightDepartment:
       'Flight Department',
        icaoAircraftCode:
       'Aircraft Type',
        ete:
       'ETE',
        etaAtd:
       'ETA/ATD',
        originDestination:
       'Origin/Destination',
        isAircraftOnGround:
       'On Ground',
        itpMarginTemplate:
       'ITP Margin Template',
       etaAtdZulu:
       'etaAtdZulu',
       expandedDetail: "expandedDetail"
    };

    export const tailNumberTextColor = {
        fuelerLinx:
        '#1D497F',
        activeFuelRelease:
        '#0DC520',
        outOfNetwork:
        '#FF7F00',
        inNetwork:
        '#C9CEEA'
     };
