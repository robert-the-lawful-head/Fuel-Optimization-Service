import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponseWraper } from '../models/apiResponseWraper';
import { Swim } from '../models/swim';

@Injectable()
export class SwimService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/SWIM';
    }

    public getArrivals(icao: string): Observable<ApiResponseWraper<Swim[]>> {
        return this.http.get<ApiResponseWraper<Swim[]>>(this.accessPointUrl + '/arrivals/'+icao, {
            headers: this.headers,
        });
    }
    public getDepartures(icao: string): Observable<ApiResponseWraper<Swim[]>> {
        return this.http.get<ApiResponseWraper<Swim[]>>(this.accessPointUrl + '/departures/'+icao, {
            headers: this.headers,
        });
    }
}
