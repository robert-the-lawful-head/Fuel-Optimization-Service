import {CertificateTypes } from '../enums/certificate-types';

export type CustomerInfoByGroup = {
    oid: number;
    groupId: number;
    customerId: number;
    company: string;
    userName: string;
    password: string;
    joined?: Date;
    active?: boolean;
    distribute?: boolean;
    network?: boolean;
    mainPhone: string;
    address: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
    website: string;
    showJetA?: boolean;
    show100Ll?: boolean;
    suspended?: boolean;
    defaultTemplate?: number;
    customerType?: number;
    emailSubscription?: boolean;
    sfid: string;
    certificateType?: CertificateTypes;
    customerCompanyType?: number;
    pricingTemplateRemoved?: boolean;
    mergeRejected?: boolean;
}
