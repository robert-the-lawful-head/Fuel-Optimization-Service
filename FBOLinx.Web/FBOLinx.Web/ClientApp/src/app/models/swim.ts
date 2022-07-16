export interface Swim  {
    id:                 number;
    tailNumber:         string;
    flightDepartment:   string;
    make:               string;
    model:              string;
    fuelCapacityGal:    number;
    origin:             string;
    city:               string;
    departureICAO:      string;
    departureCity:      string;
    arrivalICAO:        string;
    arrivalCity:        string;
    atdLocal:           Date;
    atdZulu:            Date;
    etaLocal:           Date;
    etaZulu:            Date;
    ete:                Time;
    actualSpeed:        number;
    altitude:           number;
    latitude:           number;
    longitude:          number;
    isAircraftOnGround: boolean;
    itpMarginTemplate:  string;
};

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