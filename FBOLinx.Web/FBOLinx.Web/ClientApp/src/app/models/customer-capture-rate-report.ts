import { IntraNetworkVisitsReportByAirportItem } from './intra-network-visits-report-by-airport-item';

export interface CustomerCaptureRateReport {
    oid: number;
    customerId: number;
    company: string;
    totalOrders: number;
    airportOrders: number;
    PercentCustomerBusiness: number | null;
}
