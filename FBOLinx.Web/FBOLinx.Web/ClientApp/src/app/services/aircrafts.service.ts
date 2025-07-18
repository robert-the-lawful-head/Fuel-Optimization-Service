import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { AircraftSize, AircraftType } from '../models';

@Injectable()
export class AircraftsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/aircrafts';
    }

    public getAll() {
        return this.http.get<AircraftType[]>(this.accessPointUrl, {
            headers: this.headers,
        });
    }

    public getAircraftSizes() {
        return this.http.get<AircraftSize[]>(this.accessPointUrl + '/sizes', {
            headers: this.headers,
        });
    }

    public getCustomersByTail(groupId, tailNumber) {
        return this.http.get(
            this.accessPointUrl +
                '/customers-by-tail/group/' +
                groupId +
                '/tail/' +
                tailNumber,
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
