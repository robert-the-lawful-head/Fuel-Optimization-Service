import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class DistributionService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/distribution';
    }

    public getDistributionLogForFbo(fboId, resultCount) {
        return this.http.get(
            this.accessPointUrl + '/fbo/' + fboId + '/log/' + resultCount,
            { headers: this.headers }
        );
    }

    public getDistributionValidityForFbo(fboId) {
        return this.http.get(
            this.accessPointUrl + '/fbo/' + fboId + '/validity',
            { headers: this.headers }
        );
    }

    public distributePricing(payload) {
        return this.http.post(
            this.accessPointUrl + '/distributepricing',
            payload,
            { headers: this.headers }
        );
    }

    public previewDistribution(payload) {
        return this.http.post(
            this.accessPointUrl + '/previewdistribution',
            payload,
            { headers: this.headers }
        );
    }
}
