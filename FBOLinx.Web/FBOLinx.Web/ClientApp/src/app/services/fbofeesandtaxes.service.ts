import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { FeesAndTaxes } from '../models';

@Injectable()
export class FbofeesandtaxesService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/fboFeesAndTaxes';
    }

    public getById(id) {
        return this.http.get(this.accessPointUrl + '/' + id, {
            headers: this.headers,
        });
    }

    public getByFbo(fboId) {
        return this.http.get(this.accessPointUrl + '/fbo/' + fboId, {
            headers: this.headers,
        });
    }

    public getByFboAndCustomer(fboId, customerId) {
        return this.http.get(
            this.accessPointUrl + '/fbo/' + fboId + '/customer/' + customerId,
            {
                headers: this.headers,
            }
        );
    }

    public getByFboAndPricingTemplate(fboId, pricingTemplateId) {
        return this.http.get<FeesAndTaxes[]>(
            this.accessPointUrl +
                '/fbo/' +
                fboId +
                '/pricing-template/' +
                pricingTemplateId,
            {
                headers: this.headers,
            }
        );
    }

    public getByFboAndAircraft(fboId, aircraftId) {
        return this.http.get(
            this.accessPointUrl + '/fbo/' + fboId + '/aircraft/' + aircraftId,
            {
                headers: this.headers,
            }
        );
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
