import { PolicyAndAgreementDocuments } from "./policyAndAgreementDocuments";

export interface DocumentsToAcceptDto {
    userId: number;
    hasPendingDocumentsToAccept: boolean;
    documentToAccept: PolicyAndAgreementDocuments | null;
}
