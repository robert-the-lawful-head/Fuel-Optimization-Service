import { SwimType } from "../enums/flight-watch.enum";

export interface SwimFilter {
    filterText?: string;
    filteredTypes?: string[];
    dataType?: SwimType | null;
    isCommercialAircraftVisible?: boolean;
};
