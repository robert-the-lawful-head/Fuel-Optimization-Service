import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class AircraftpricesService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/aircraftprices';
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public getAircraftPricesForCustomerAircraft(customerAircraftId, fboId) {
        return this.http.get(
            this.accessPointUrl +
                '/customeraircraft/' +
                customerAircraftId +
                '/fbo/' +
                fboId,
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

    public removeMultiple(payload) {
        return this.http.post(
            this.accessPointUrl + '/delete-multiple',
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }
}
