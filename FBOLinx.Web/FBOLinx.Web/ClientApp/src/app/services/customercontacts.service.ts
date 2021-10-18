import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class CustomercontactsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/customercontacts';
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public add(payload , userId  , Role) {
        return this.http.post(this.accessPointUrl, payload, {
            headers: this.headers,
        });
    }

    public remove(payload , userId ,  customerId) {
        return this.http.delete(this.accessPointUrl + '/' + payload+'/'+userId+'/'+ customerId, {
            headers: this.headers,
        });
    }

    public update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }

    public getCustomerEmailsByGroupAndFBOAndPricing(
        groupId: number,
        fboId: number,
        pricingTemplateId: number
    ) {
        return this.http.get(
            this.accessPointUrl +
                '/group/' +
                groupId +
                '/fbo/' +
                fboId +
                '/pricingtemplate/' +
                pricingTemplateId,
            {
                headers: this.headers,
            }
        );
    }
}
