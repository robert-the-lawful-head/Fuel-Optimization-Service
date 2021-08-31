import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class TagsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/customertags';
    }

    public getTags(payload) {
        return this.http.get(this.accessPointUrl, {
            headers: this.headers,
            params: { GroupId: payload.groupId, CustomerId: payload.customerId, IsFuelerLinx: payload.isFuelerLinx }
        });
    }

    public getGroupTags(payload) {
        return this.http.get(this.accessPointUrl + "/groups/" + payload, {
            headers: this.headers
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
}
