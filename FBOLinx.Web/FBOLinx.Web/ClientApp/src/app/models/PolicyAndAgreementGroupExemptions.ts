import { PolicyAndAgreementDocuments } from "./policyAndAgreementDocuments";

export interface PolicyAndAgreementGroupExemptions {
    oid: number;
    documentId: number;
    dateTimeExempted: string;
    groupId: number;
    policyAndAgreementDocuments: PolicyAndAgreementDocuments;
}
