import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CustomerAircraftNote } from '../models/customer-aircraft-note';

@Injectable()
export class CustomeraircraftsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string , private route : ActivatedRoute) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/customeraircrafts';
    }

    public getCustomerAircraftsByGroupAndCustomerId(
        groupId,
        fboId,
        customerId
    ) {
        return this.http.get(
            this.accessPointUrl +
                '/group/' +
                groupId +
                '/fbo/' +
                fboId +
                '/customer/' +
                customerId,
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerAircraftsByGroup(groupId) {
        return this.http.get(this.accessPointUrl + '/group/' + groupId, {
            headers: this.headers,
        });
    }

    public getCustomerAircraftsByGroupAndFbo(groupId, fboId) {
        return this.http.get(
            this.accessPointUrl + '/group/' + groupId + '/fbo/' + fboId,
            {
                headers: this.headers,
            }
        );
    }

    public getAircraftsListByGroupAndFbo(groupId, fboId) {
        return this.http.get(
            this.accessPointUrl +
                '/group/' +
                groupId +
                '/fbo/' +
                fboId +
                '/list',
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerAircraftsCountByGroupId(groupId) {
        return this.http.get(
            this.accessPointUrl + '/group/' + groupId + '/count',
            {
                headers: this.headers,
            }
        );
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public add(payload , userId, fboId: number = 0, isFavorite: boolean = false ) {
        return this.http.post(`${this.accessPointUrl}/${userId}/?isFavorite=${isFavorite}&fboId=${fboId}`, payload, {
            headers: this.headers,
        });
    }

    public addMultipleWithTemplate(groupId, fboId, customerId, payload) {
        return this.http.post(
            `${this.accessPointUrl}/group/${groupId}/fbo/${fboId}/customer/${customerId}/multiple`,
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public import(payload) {
        return this.http.post(this.accessPointUrl+'/import', payload, {
            headers: this.headers,
        });
    }

    public remove(payload , userId) {
        return this.http.delete(this.accessPointUrl + '/' + payload.oid+'/'+userId, {
            headers: this.headers,
        });
    }

    public update(payload , userId) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid+'/'+userId, payload, {
            headers: this.headers,
        });
    }

    public updateTemplate(fboid, payload) {
        return this.http.put(this.accessPointUrl + '/fbo/' + fboid, payload, {
            headers: this.headers,
        });
    }

    public createAircraftWithCustomer(payload) {
        return this.http.post(
            this.accessPointUrl + '/create-with-customer',
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerAircraftNotes(customerAircraftId: number) {
        return this.http.get(
            this.accessPointUrl + '/notes/' + customerAircraftId,
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerAircraftNotesByTailNumberGroupIdCustomerId(tailNumber: string, groupId: number, customerId: number) {
        return this.http.get(
            this.accessPointUrl + '/notes/tailnumber/' + tailNumber + '/groupId/' + groupId + '/customerid/' + customerId,
            {
                headers: this.headers,
            }
        );
    }

    public addCustomerAircraftNotes(payload: CustomerAircraftNote) {
        return this.http.post(this.accessPointUrl + '/notes', payload, {
            headers: this.headers,
        });
    }

    public updateCustomerAircraftNotes(payload: CustomerAircraftNote) {
        return this.http.put(this.accessPointUrl + '/notes/' + payload.oid, payload, {
            headers: this.headers,
        });
    }

    public deleteCustomerAircraftNotes(id: number) {
        return this.http.delete(
            this.accessPointUrl + '/notes/' + id,
            {
                headers: this.headers,
            }
        );
    }
}
