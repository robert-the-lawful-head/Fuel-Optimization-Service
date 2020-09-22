import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class UserService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/users';
    }

    public getCurrentUser() {
        return this.http.get(this.accessPointUrl + '/current', {
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

    public getForGroupId(groupId) {
        return this.http.get(this.accessPointUrl + '/group/' + groupId, {
            headers: this.headers,
        });
    }

    public getForFboId(fboId) {
        return this.http.get(this.accessPointUrl + '/fbo/' + fboId, {
            headers: this.headers,
        });
    }

    public getRoles() {
        return this.http.get(this.accessPointUrl + '/roles', {
            headers: this.headers,
        });
    }

    public resetPassword(payload) {
        return this.http.post(this.accessPointUrl + '/resetpassword', payload, {
            headers: this.headers,
        });
    }

    public updatePassword(payload) {
        return this.http.post(this.accessPointUrl + '/newpassword', payload, {
            headers: this.headers,
        });
    }

    public checkemailexists(payload) {
        return this.http.get(
            this.accessPointUrl + '/checkemailexists/' + payload,
            {
                headers: this.headers,
            }
        );
    }
}
