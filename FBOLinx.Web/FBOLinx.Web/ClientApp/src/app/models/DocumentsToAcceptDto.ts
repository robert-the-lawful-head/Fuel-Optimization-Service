import { PolicyAndAgreementDocuments } from "./PolicyAndAgreementDocuments";

export interface DocumentsToAcceptDto {
    userId: number;
    hasPendingDocumentsToAccept: boolean;
    documentToAccept: PolicyAndAgreementDocuments | null;
}
