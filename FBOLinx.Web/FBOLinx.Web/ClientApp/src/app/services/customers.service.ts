import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject,Injectable } from '@angular/core';

import { Customer } from '../models/customer';

@Injectable()
export class CustomersService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/customers';
    }

    public getByGroupAndFbo(groupId, fboId) {
        return this.http.get(
            this.accessPointUrl + '/group/' + groupId + '/fbo/' + fboId,
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

    public add(payload: Customer) {
        return this.http.post<Customer>(this.accessPointUrl, payload, {
            headers: this.headers,
        });
    }

    public remove(payload) {
        return this.http.delete(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }

    public importcustomers(payload) {
        return this.http.post(
            this.accessPointUrl + '/importcustomers',
            payload,
            {
                headers: this.headers,
            }
        );
    }
}
