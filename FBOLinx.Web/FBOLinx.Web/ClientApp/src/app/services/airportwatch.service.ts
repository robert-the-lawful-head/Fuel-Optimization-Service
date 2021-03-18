import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AirportWatchHistoricalDataRequest, FlightWatchHistorical } from '../models/flight-watch-historical';

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
        return this.http.get<any>(this.accessPointUrl + '/list/fbo/' + fboId, { headers: this.headers });
    }

    public getArrivalsDepartures(groupId: number, fboId: number) {
        return this.http.post<FlightWatchHistorical[]>(
            this.accessPointUrl + '/group/' + groupId + '/fbo/' + fboId + '/arrivals-depatures',
            {},
            { headers: this.headers },
        );
    }

    public getVisits(groupId: number, fboId: number, body: AirportWatchHistoricalDataRequest) {
        return this.http.post<FlightWatchHistorical[]>(
            this.accessPointUrl + '/group/' + groupId + '/fbo/' + fboId + '/visits',
            body,
            { headers: this.headers },
        );
    }
}
