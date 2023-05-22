import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import * as moment from 'moment';


@Injectable()
export class ServiceOrderService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/serviceorder';
    }

    //Call the GetServiceOrdersForFbo method in the ServiceOrderController
    public getServiceOrdersForFbo(fboId: number) {
        return this.http.get(this.accessPointUrl + '/list/fbo/' + fboId);
    }

    //Call the GetServiceOrder method in the ServiceOrderController
    public getServiceOrderById(serviceOrderId: number) {
        return this.http.get(this.accessPointUrl + '/id/' + serviceOrderId);
    }

    //Call the PostServiceOrder method in the ServiceOrderController
    public createServiceOrder(serviceOrder: any) {
        const body = JSON.stringify(serviceOrder);
        return this.http.post(this.accessPointUrl, body, { headers: this.headers });
    }

    //Call the PutServiceOrder method in the ServiceOrderController
    public updateServiceOrder(serviceOrder: any) {
        const body = JSON.stringify(serviceOrder);
        return this.http.put(this.accessPointUrl, body, { headers: this.headers });
    }

    //Call the DeleteServiceOrder method in the ServiceOrderController
    public deleteServiceOrder(serviceOrderId: number) {
        return this.http.delete(this.accessPointUrl + '/' + serviceOrderId);
    }

    //Call the GetServiceOrderItems method in the ServiceOrderController
    public getServiceOrderItems(serviceOrderId: number) {
        return this.http.get(this.accessPointUrl + '/items/' + serviceOrderId);
    }

    //Call the GetServiceOrderItem method in the ServiceOrderController
    public getServiceOrderItem(serviceOrderItemId: number) {
        return this.http.get(this.accessPointUrl + '/item/' + serviceOrderItemId);
    }

    //Call the PostServiceOrderItem method in the ServiceOrderController
    public createServiceOrderItem(serviceOrderItem: any) {
        const body = JSON.stringify(serviceOrderItem);
        return this.http.post(this.accessPointUrl + '/item', body, { headers: this.headers });
    }

    //Call the PutServiceOrderItem method in the ServiceOrderController
    public updateServiceOrderItem(serviceOrderItem: any) {
        const body = JSON.stringify(serviceOrderItem);
        return this.http.put(this.accessPointUrl + '/item', body, { headers: this.headers });
    }

//Call the DeleteServiceOrderItem method in the ServiceOrderController
    public deleteServiceOrderItem(serviceOrderItemId: number) {
        return this.http.delete(this.accessPointUrl + '/item/' + serviceOrderItemId);
    }        
        
}
