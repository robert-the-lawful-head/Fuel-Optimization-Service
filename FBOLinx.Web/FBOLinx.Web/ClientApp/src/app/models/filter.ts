import { SwimType } from "../enums/flight-watch.enum";

export interface SwimFilter {
    filterText: string;
    dataType: SwimType | null;
};
