import { SwimType } from "./swim";

export interface SwimFilter {
    filterText: string;
    dataType: SwimType | null;
};