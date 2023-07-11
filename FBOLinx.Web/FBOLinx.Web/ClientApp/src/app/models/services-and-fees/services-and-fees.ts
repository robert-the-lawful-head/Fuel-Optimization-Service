export interface FbosServicesAndFeesResponse {
    serviceType: ServiceTypeResponse;
    servicesAndFees: ServicesAndFeesResponse[];
}

export interface ServiceTypeResponse extends FboCustomServiceType{
    isCustom: boolean;
}

export interface FboCustomServiceType {
    oid: number;
    name: string;
}

export interface ServicesAndFeesResponse extends ServicesAndFees{
    isCustom: boolean;
}

export interface ServicesAndFees {
    oid: number;
    handlerId: number | null;
    serviceOfferedId: number | null;
    service: string;
    serviceTypeId: number | null;
    isActive: boolean;
}
