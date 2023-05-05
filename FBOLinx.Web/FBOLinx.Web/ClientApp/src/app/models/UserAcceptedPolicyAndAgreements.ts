import { PolicyAndAgreementDocuments } from "./policyAndAgreementDocuments";
import { User } from "./user";

export interface UserAcceptedPolicyAndAgreements {
    oid: number;
    userId: number;
    documentId: number;
    acceptedDateTime: string;
    documents: PolicyAndAgreementDocuments[];
    user: User;
}
