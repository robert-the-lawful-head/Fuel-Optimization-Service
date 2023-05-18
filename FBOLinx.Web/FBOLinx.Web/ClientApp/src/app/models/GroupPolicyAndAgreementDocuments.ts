import { DocumentAcceptanceFlag, DocumentTypeEnum } from "../enums/documents..enum";

export interface GroupPolicyAndAgreementDocuments {
    oid: number;
    documentType: DocumentTypeEnum;
    documentVersion: string;
    document: string;
    acceptanceFlag: DocumentAcceptanceFlag;
    isExempted: boolean;
}
