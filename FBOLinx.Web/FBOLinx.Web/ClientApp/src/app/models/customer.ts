export type CustomersListType = {
    customerInfoByGroupID: number;
    companyId: number;
    company: string;
};

export type Customer = {
    oid: number;
    company: string;
    certificateType: string;
    mainPhone: string;
    address: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
    website: string;
};
