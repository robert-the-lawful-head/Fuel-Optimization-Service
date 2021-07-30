import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject,Injectable } from '@angular/core';

@Injectable()
export class ContactinfobygroupsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/contactinfobygroups';
    }

    public getCustomerContactInfoByGroup(groupId, customerId) {
        return this.http.get(
            this.accessPointUrl +
                '/group/' +
                groupId +
                '/customer/' +
                customerId,
            {
                headers: this.headers,
            }
        );
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public add(payload) {
        return this.http.post(this.accessPointUrl, payload, {
            headers: this.headers,
        });
    }

    public addMultiple(groupId, customerId, payload) {
        return this.http.post(`${this.accessPointUrl}/group/${groupId}/customer/${customerId}/multiple`, payload, {
            headers: this.headers,
        });
    }

    public remove(payload) {
        return this.http.delete(this.accessPointUrl + '/' + payload, {
            headers: this.headers,
        });
    }

    public update(payload) {
        if (payload.oid) {
            return this.http.put(
                this.accessPointUrl + '/' + payload.oid,
                payload,
                {
                    headers: this.headers,
                }
            );
        } else if (payload.contactInfoByGroupId) {
            payload.Oid = payload.contactInfoByGroupId;
            return this.http.put(
                this.accessPointUrl + '/' + payload.Oid,
                payload,
                {
                    headers: this.headers,
                }
            );
        }
    }

    public import(payload) {
        return this.http.post(this.accessPointUrl + '/import', payload, {
            headers: this.headers,
        });
    }
}
