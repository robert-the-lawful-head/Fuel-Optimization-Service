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
}
export enum FlightLegStatusEnum {
    EnRoute,
    Landing,
    Taxiing,
    Arrived,
    Departing
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
