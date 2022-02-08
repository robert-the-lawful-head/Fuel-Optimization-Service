import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class DateTimeService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/datetime';
    }

    public getNexTuesdayDate(date) {
        return this.http.get(this.accessPointUrl + '/next-tuesday/' + date, {
            headers: this.headers,
            responseType: 'text',
        });
    }
}
