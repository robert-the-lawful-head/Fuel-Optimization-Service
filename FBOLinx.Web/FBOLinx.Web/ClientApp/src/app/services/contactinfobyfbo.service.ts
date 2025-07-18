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
        this.accessPointUrl = baseUrl + 'api/contactinfobyfbo';
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
        if (payload.oid) {
            return this.http.put(
                this.accessPointUrl + '/contact/' + payload.oid,
                payload,
                {
                    headers: this.headers,
                }
            );
        }
    }

    public updateDistributionForAllCustomerContacts(customerId, fboId, isEnabled) {
        return this.http.post(
            this.accessPointUrl + '/update-distribution-all-customer-contacts/' + customerId + '/fbo/' + fboId + '/is-enabled/' + (isEnabled == true ? 1 : 0),
            {
                headers: this.headers,
            }
        );
    }
}

