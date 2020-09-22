import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class FbopricesService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/fboprices';
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public getFbopricesByFboIdCurrent(fboId) {
        return this.http.get(
            this.accessPointUrl + '/fbo/' + fboId + '/current',
            {
                headers: this.headers,
            }
        );
    }

    public checkFboExpiredPricing(fboId) {
        return this.http.get(
            this.accessPointUrl + '/fbo/' + fboId + '/ispricingexpired',
            {
                headers: this.headers,
            }
        );
    }

    public checkFboExpiredPricingGroup(groupid) {
        return this.http.get(
            this.accessPointUrl + '/group/' + groupid + '/ispricingexpiredgroupadmin',
            {
                headers: this.headers,
            }
        );
    }

    public getFbopricesByFboIdStaged(fboId) {
        return this.http.get(
            this.accessPointUrl + '/fbo/' + fboId + '/staged',
            {
                headers: this.headers,
            }
        );
    }

    public getFbopricesByFboIdAndProductCurrent(fboId, product) {
        return this.http.get(
            this.accessPointUrl +
                '/fbo/' +
                fboId +
                '/product/' +
                encodeURIComponent(product) +
                '/current',
            {
                headers: this.headers,
            }
        );
    }

    public checkifExistFrboPrice(fboId, payload) {
        return this.http.post(
            this.accessPointUrl + '/fbo/' + fboId + '/check/',
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public suspendAllPricing(fboId) {
        return this.http.post(
            this.accessPointUrl + '/fbo/' + fboId + '/suspendpricing',
            {
                headers: this.headers,
            }
        );
    }

    public suspendJetPricing(fboId) {
        return this.http.post(
            this.accessPointUrl + '/fbo/' + fboId + '/suspendpricing/jet',
            {
                headers: this.headers,
            }
        );
    }

    public suspendRetailPricing(fboId) {
        return this.http.post(
            this.accessPointUrl + '/fbo/' + fboId + '/suspendpricing/retail',
            {
                headers: this.headers,
            }
        );
    }

    public getPricesByMonthForFbo(fboId, payload) {
        return this.http.post(
            this.accessPointUrl + '/analysis/prices-by-month/fbo/' + fboId,
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public getFuelPricesForCompany(payload) {
        return this.http.post(
            this.accessPointUrl + '/volume-discounts-for-customer/', payload,
            {
                headers: this.headers,
            }
        );
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
}
