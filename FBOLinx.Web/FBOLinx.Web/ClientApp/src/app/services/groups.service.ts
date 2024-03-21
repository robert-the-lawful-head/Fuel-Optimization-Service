import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class GroupsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/groups';
    }

    isGroupFboPartOfNetwork(groupId: number, fboName: string, icao: string) {
        return this.http.get<boolean>(`${this.accessPointUrl}/group/${groupId}/fbo-name/${fboName}/icao/${icao}/is-part-of-network`, {
            headers: this.headers,
        });
    }

    getAllGroups() {
        return this.http.get(this.accessPointUrl, {
            headers: this.headers,
        });
    }

    get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    add(payload) {
        return this.http.post(this.accessPointUrl, payload, {
            headers: this.headers,
        });
    }

    remove(payload) {
        return this.http.delete(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }

    groupsAndFbos() {
        return this.http.get(this.accessPointUrl + '/group-fbo', {
            headers: this.headers,
        });
    }

    mergeGroups(payload) {
        return this.http.post(this.accessPointUrl + '/merge-groups', payload, {
            headers: this.headers,
        });
    }

    getLogo(id: number) {
        return this.http.get(this.accessPointUrl + `/group/${id}/logo`, {
            headers: this.headers,
        });
    }

    uploadLogo(payload) {
        return this.http.post(this.accessPointUrl + '/upload-logo', payload, {
            headers: this.headers,
        });
    }

    deleteLogo(id: number) {
        return this.http.delete(this.accessPointUrl + `/group/${id}/logo`, {
            headers: this.headers,
        });
    }
}
