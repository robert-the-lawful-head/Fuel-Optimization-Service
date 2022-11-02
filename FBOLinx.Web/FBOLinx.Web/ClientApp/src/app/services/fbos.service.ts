import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class FbosService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/fbos';
    }

    public getForGroup(groupId) {
        return this.http.get(this.accessPointUrl + '/group/' + groupId, {
            headers: this.headers,
        });
    }

    public getAllFbos() {
        return this.http.get(this.accessPointUrl, { headers: this.headers });
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public getByFboId(fboid) {
        return this.http.get(this.accessPointUrl + '/' + fboid, {
            headers: this.headers,
        });
    }

    public add(payload) {
        return this.http.post(this.accessPointUrl, payload, {
            headers: this.headers,
        });
    }

    public addSingleFbo(payload) {
        return this.http.post(this.accessPointUrl + '/single', payload, {
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

    public manageFbo(id: number) {
        return this.http.post(
            this.accessPointUrl + '/manage/' + id,
            {},
            {
                headers: this.headers,
            }
        );
    }

    public getLocation(id: number) {
        return this.http.get(this.accessPointUrl + '/' + id + '/location', {
            headers: this.headers,
        });
    }

    public getByAcukwikHandlerId(handlerId: number) {
        return this.http.get(
            this.accessPointUrl + '/by-akukwik-handlerId/' + handlerId,
            {
                headers: this.headers,
            }
        );
    }

    public updateLastLogin(fboId: number) {
        return this.http.post(this.accessPointUrl + '/updatelastlogin/' + fboId, {
            headers: this.headers,
        });
    }

    public uploadLogo(payload) {
        return this.http.post(this.accessPointUrl + '/uploadfbologo', payload, {
            headers: this.headers,
        });
    }

    public deleteLogo(id: number) {
        return this.http.delete(this.accessPointUrl + '/fbologo/' + id, {
            headers: this.headers,
        });
    }

    public getLogo(fboid: number) {
        return this.http.get(this.accessPointUrl + '/getfbologo/' + fboid, {
            headers: this.headers,
        });
    }
}
