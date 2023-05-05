import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { DocumentsToAcceptDto } from '../models/DocumentsToAcceptDto';
import { GroupPolicyAndAgreementDocuments } from '../models/GroupPolicyAndAgreementDocuments';
import { UserAcceptedPolicyAndAgreements } from '../models/UserAcceptedPolicyAndAgreements';

@Injectable()
export class DocumentService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/documents';
    }
    acceptDocument(userId: number, documentId: number) {
        return this.http.post<UserAcceptedPolicyAndAgreements[]>(this.accessPointUrl + '/' + documentId + '/user/' + userId + '/accept-policies-and-agreements', {}, {
            headers: this.headers,
        });
    }
    getDocumentsToAccept(userId: number) {
        return this.http.get<DocumentsToAcceptDto>(this.accessPointUrl + '/user/' + userId + '/documents-to-accept', {
            headers: this.headers,
        });
    }
    getRecentsAvailableDocuments(groupId: number) {
        return this.http.get(this.accessPointUrl+`/group/${groupId}`, {
            headers: this.headers,
        });
    }
    updateExemptedDocuments(groupId:number, payload: GroupPolicyAndAgreementDocuments[]) {
        return this.http.post(this.accessPointUrl+`/group/${groupId}/toogle-document-exemption`,payload, {
            headers: this.headers,
        });
    }
}
