import { DocumentAcceptanceFlag } from "../enums/documents..enum";

export interface GroupPolicyAndAgreementDocuments {
    oid: number;
    documentType: string;
    documentVersion: string;
    document: string;
    acceptanceFlag: DocumentAcceptanceFlag;
    isExempted: boolean;
}
