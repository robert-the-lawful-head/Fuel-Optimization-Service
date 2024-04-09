import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class CustomermarginsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/customermargins';
    }

    public getCustomerMarginsByPricingTemplateId(pricingTemplateId) {
        return this.http.get(
            this.accessPointUrl + '/pricingtemplate/' + pricingTemplateId,
            { headers: this.headers }
        );
    }

    public getAllFbos() {
        return this.http.get(this.accessPointUrl, { headers: this.headers });
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
        return this.http.delete(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public bulkRemove(payload) {
        return this.http.post(this.accessPointUrl + '/bulkremove', payload, {
            headers: this.headers,
        });
    }

    public update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }


    public updatecustomermargin(payload) {
        return this.http.post(
            this.accessPointUrl + '/updatecustomermargin',
            payload,
            { headers: this.headers }
        );
    }

    public updatemultiplecustomermargin(payload) {
        return this.http.post(
            this.accessPointUrl + '/updatemultiplecustomermargin',
            payload,
            {
                headers: this.headers,
            }
        );
    }
}
