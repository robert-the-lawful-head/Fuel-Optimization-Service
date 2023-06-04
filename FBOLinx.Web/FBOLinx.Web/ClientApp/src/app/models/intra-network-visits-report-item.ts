import { IntraNetworkVisitsReportByAirportItem } from './intra-network-visits-report-by-airport-item';

export type IntraNetworkVisitsReportItem = {
    groupName: string;
    tailNumber: string;
    company: string;
    faaRegisteredOwner: string;
    aircraftType: string;
    aircraftTypeCode: string;
    customerInfoByGroupId: number;
    visitsByAirport: IntraNetworkVisitsReportByAirportItem[];
}
