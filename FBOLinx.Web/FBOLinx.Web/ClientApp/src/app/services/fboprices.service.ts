import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

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
            this.accessPointUrl +
                '/group/' +
                groupid +
                '/ispricingexpiredgroupadmin',
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

    public getFbopricesByFboIdAllStaged(fboId) {
        return this.http.get(
            this.accessPointUrl + '/fbo/' + fboId + '/all-staged',
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

    public checkifExistFboPrice(fboId, payload) {
        return this.http.post(
            this.accessPointUrl + '/fbo/' + fboId + '/check/',
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public checkifExistStagedFboPrice(fboId, payload) {
        return this.http.post(
            this.accessPointUrl + '/fbo/' + fboId + '/checkstaged/',
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public suspendPricing(oid) {
        return this.http.post(this.accessPointUrl + '/suspendpricing/' + oid, {
            headers: this.headers,
        });
    }

    public suspendPricingGenerator(payload) {
        return this.http.post(this.accessPointUrl + '/suspend-pricing-generator/', 
            payload,
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
            this.accessPointUrl + '/price-lookup-for-customer/',
            payload,
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

    public removePricing(fboid, product) {
        return this.http.delete(this.accessPointUrl + '/delete-price-by-product/fbo/' + fboid + "/product/" + product, {
            headers: this.headers,
        });
    }

    public update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }

    public updatePricingGenerator(payload) {
        return this.http.post(this.accessPointUrl + '/update-price-generator/', payload, {
            headers: this.headers,
        });
    }

    public handlePriceChangeCleanUp(fboId) {
        return this.http.post(this.accessPointUrl + '/handle-price-change-cleanup/' + fboId,
            {
                headers: this.headers
            });
    }
}
