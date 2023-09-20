import { DocumentAcceptanceFlag, DocumentTypeEnum } from "../enums/documents..enum";
import { PolicyAndAgreementGroupExemptions } from "./PolicyAndAgreementGroupExemptions";
import { UserAcceptedPolicyAndAgreements } from "./UserAcceptedPolicyAndAgreements";

export interface PolicyAndAgreementDocuments {
    oid: number;
    documentType: DocumentTypeEnum;
    documentVersion: string;
    document: string;
    acceptanceFlag: DocumentAcceptanceFlag;
    isEnabled: boolean;
    policyAndAgreementGroupExemptions: PolicyAndAgreementGroupExemptions[];
    userAcceptedPolicyAndAgreements: UserAcceptedPolicyAndAgreements[];
}
