import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class FbofeeandtaxomitsbypricingtemplateService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl =
            baseUrl + 'api/FboFeeAndTaxOmitsByPricingTemplate';
    }

    public getById(id) {
        return this.http.get(this.accessPointUrl + '/' + id, {
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

    public update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }
}
