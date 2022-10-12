import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ApiResponseWraper } from '../models/apiResponseWraper';
import { FlightWatchModelResponse } from '../models/flight-watch';

@Injectable()
export class FlightWatchService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/flightWatch';
    }

    public getAirportLiveData(fboId: number, icao: string) {
        return this.http.get<ApiResponseWraper<FlightWatchModelResponse[]>>(
            `${this.accessPointUrl}/list/fbo/${fboId}/airport/${icao}`,
            { headers: this.headers }
        );
    }
}
