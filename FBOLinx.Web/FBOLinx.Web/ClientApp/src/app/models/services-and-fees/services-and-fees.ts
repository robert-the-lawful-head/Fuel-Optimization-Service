export interface FbosServicesAndFeesResponse {
    serviceType: ServiceTypeResponse;
    servicesAndFees: ServicesAndFeesResponse[];
}

export interface ServiceTypeResponse extends FboCustomServiceType{
    isCustom: boolean;
    createdByUser: string;
}

export interface FboCustomServiceType {
    oid: number;
    name: string;
    createdByUserId: number;
    createdDate: Date;
}

export interface ServicesAndFeesResponse extends ServicesAndFees{
    isCustom: boolean;
    createdByUser: string;
}

export interface ServicesAndFees {
    oid: number;
    handlerId: number | null;
    serviceOfferedId: number | null;
    service: string;
    serviceTypeId: number | null;
    isActive: boolean;
    createdByUserId: number;
    createdDate: Date;
}
