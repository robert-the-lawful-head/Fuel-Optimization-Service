export interface FbosGridViewModel {
    oid: number;
    fbo: string;
    active: boolean | null;
    icao: string;
    iata: string;
    groupId: number;
    needAttentionCustomers: number;
    pricingExpired: boolean;
    accountExpired: boolean;
    lastLogin: string | null;
    users: any[];
    quotes30Days: number;
    orders30Days: number;
    costPrice: number | null;
    retailPrice: number | null;
    acukwikFboHandlerId: number | null;
}
