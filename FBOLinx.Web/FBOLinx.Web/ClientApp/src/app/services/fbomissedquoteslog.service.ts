import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class FbomissedquoteslogService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/fbomissedquoteslog';
    }

    public getRecentMissedQuotes(fboId) {
        return this.http.get(this.accessPointUrl + '/recent-missed-quotes/fbo/' + fboId, {
            headers: this.headers,
        });
    }

    public getRecentMissedOrders(fboId) {
        return this.http.get(this.accessPointUrl + '/recent-missed-orders/fbo/' + fboId, {
            headers: this.headers,
        });
    }
}
