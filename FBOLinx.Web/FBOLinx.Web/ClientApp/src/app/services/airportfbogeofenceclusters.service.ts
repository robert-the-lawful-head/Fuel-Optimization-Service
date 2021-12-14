import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class AirportFboGeofenceClustersService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/airportfbogeofenceclusters';
    }

    get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    getAllClusters() {
        return this.http.get(this.accessPointUrl + '/', {
            headers: this.headers,
        });
    }

    add(payload) {
        return this.http.post(this.accessPointUrl, payload, {
            headers: this.headers,
        });
    }

    remove(payload) {
        return this.http.delete(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }

    getAirportsWithAntennaData() {
        return this.http.get(this.accessPointUrl + '/airports/list',
            {
                headers: this.headers
            });
    }

    getClustersByAcukwikAirportId(acukwikAirportId) {
        return this.http.get(this.accessPointUrl + '/clusters-by-acukwik-airport-id/' + acukwikAirportId,
            {
                headers: this.headers
            });
    }

    getClustersByIcao(icao) {
        return this.http.get(this.accessPointUrl + '/clusters-by-icao/' + icao,
            {
                headers: this.headers
            });
    }
}
