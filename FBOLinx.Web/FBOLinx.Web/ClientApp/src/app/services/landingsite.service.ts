import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class LandingsiteService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/landingsite';
    }

    public postContactUsMessage(payload) {
        return this.http.post(this.accessPointUrl + '/ContactUs', payload, {
            headers: this.headers,
        });
    }
}
