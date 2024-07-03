export type JetNet = {
    responseid: string;
    responsestatus: string;
    aircraftresult: AircraftResult;
};

export type AircraftResult = {
    aircraftid: number;
    modelid: number
    airframetype: string;
    maketype: string;
    make: string;
    model: string;
    icaotype: string;
    serialnbr: string;
    regnbr: string;
    yearmfr: string;
    yeardlv: string;
    weightclass: string;
    categorysize: string;
    baseicao: string;
    baseairport: string;
    ownership: string;
    usage: string;
    maintained: string;
    companies: Company[];
    companyrelationships: CompanyRelationship[];
};

export type Company = {
    company: string;
    companyDetailOpenState: boolean;
    companyrelationships: CompanyRelationship[];
}

export type CompanyRelationship = {
    companyid: number;
    parentcompanyid: number;
    companyrelation: string;
    companyname: string;
    companyisoperator: string;
    companyagencytype: string;
    companybusinesstype: string;
    companyaddress1: string;
    companyaddress2: string;
    companycity: string;
    companystate: string;
    companystateabbr: string;
    companypostcode: string;
    companycountry: string;
    companyemail: string;
    companyofficephone: string;
    contactid: number;
    contactsirname: string;
    contactfirstname: string;
    contactmiddleinitial: string;
    contactlastname: string;
    contactsuffix: string;
    contacttitle: string;
    contactemail: string;
    contactbestphone: string;
    contactofficephone: string;
    contactmobilephone: string;
    add: boolean;
};
