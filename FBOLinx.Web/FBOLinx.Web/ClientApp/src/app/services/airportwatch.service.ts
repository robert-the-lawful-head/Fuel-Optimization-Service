import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FlightWatch } from '../models/flight-watch';

@Injectable()
export class AirportWatchService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/airportwatch';
    }

    public getAll(fboId: number) {
        return this.http.get<FlightWatch[]>(this.accessPointUrl + '/list/fbo/' + fboId, { headers: this.headers });
    }
}
