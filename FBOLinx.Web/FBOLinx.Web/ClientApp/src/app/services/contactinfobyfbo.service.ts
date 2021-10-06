import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class ContactinfobyfboService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/contactinfobygroups';
    }

    public getCustomerContactInfoByFbo(fboId, contactId) {
        return this.http.get(
            this.accessPointUrl +
            '/contact/' +
            contactId +
            '/fbo/' +
            fboId,
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

    public remove(payload) {
        return this.http.delete(this.accessPointUrl + '/' + payload, {
            headers: this.headers,
        });
    }

    public update(payload) {
        if (payload.contactInfoByFboId) {
            //payload.Oid = payload.contactInfoByFboId;
            return this.http.put(
                this.accessPointUrl + '/' + payload.Oid,
                payload,
                {
                    headers: this.headers,
                }
            );
        }
    }
}

