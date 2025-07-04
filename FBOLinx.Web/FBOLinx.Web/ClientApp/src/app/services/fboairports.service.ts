import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class FboairportsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/fboairports';
    }

    public getForFbo(payload) {
        return this.http.get(this.accessPointUrl + '/fbo/' + payload.oid, {
            headers: this.headers,
        });
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

    public getLocalDateTime(fboId) {
        return this.http.get(this.accessPointUrl + '/local-datetime-now/fbo/' + fboId, {
            headers: this.headers,
        });
    }

    public getLocalTimeZone(fboid) {
        return this.http.get(this.accessPointUrl + '/local-timezone/fbo/' + fboid, {
            headers: this.headers,
            responseType: 'text',
        });
    }
}
