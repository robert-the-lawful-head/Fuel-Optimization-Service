import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AcukwikAirport } from '../models/AcukwikAirport';

@Injectable()
export class AcukwikairportsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/AcukwikAirports';
    }

    public getAllAirports(): Observable<AcukwikAirport[]> {
        return this.http.get<AcukwikAirport[]>(this.accessPointUrl, { headers: this.headers });
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

    public search(value) {
        return this.http.get(this.accessPointUrl + '/search/' + value, {
            headers: this.headers,
        });
    }

    public getAcukwikFboHandlerDetailByIcao(icao) {
        return this.http.get(
            this.accessPointUrl + '/' + icao + '/fbo-handler-detail',
            { headers: this.headers }
        );
    }
    public getAcukwikAirportByICAO(icao:string): Observable<AcukwikAirport> {
        return this.http.get<AcukwikAirport>(
            this.accessPointUrl + '/byicao/' + icao ,
            { headers: this.headers }
        );
    }
    public getNearByAcukwikAirportsByICAO(icao:string,miles:number): Observable<AcukwikAirport[]> {
        return this.http.get<AcukwikAirport[]>(
            this.accessPointUrl + '/' + icao + '/nearby-airports/' + miles + '/nautical-miles',
            { headers: this.headers }
        );
    }
}
