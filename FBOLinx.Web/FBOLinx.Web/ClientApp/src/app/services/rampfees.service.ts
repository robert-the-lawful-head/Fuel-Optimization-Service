import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class RampfeesService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/rampfees';
    }

    public getForFbo(payload) {
        return this.http.get(this.accessPointUrl + '/fbo/' + payload.oid, {
            headers: this.headers,
        });
  }

    public getForAircraft(fboId, tailNumber) {
      return this.http.get(this.accessPointUrl + '/fbo/' + fboId + '/tail/' + tailNumber, {
        headers: this.headers
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

    public importrampfees(payload) {
        return this.http.post(
            this.accessPointUrl + '/importrampfees',
            payload,
            {
                headers: this.headers,
            }
        );
    }
}
