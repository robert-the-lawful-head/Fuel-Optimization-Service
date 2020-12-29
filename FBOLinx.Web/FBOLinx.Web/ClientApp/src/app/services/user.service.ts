import { Inject, Injectable } from '@angular/core';
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

    getCurrentUser() {
        return this.http.get(this.accessPointUrl + '/current', {
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

    getForGroupId(groupId) {
        return this.http.get(this.accessPointUrl + '/group/' + groupId, {
            headers: this.headers,
        });
    }

    getForFboId(fboId) {
        return this.http.get(this.accessPointUrl + '/fbo/' + fboId, {
            headers: this.headers,
        });
    }

    getRoles() {
        return this.http.get(this.accessPointUrl + '/roles', {
            headers: this.headers,
        });
    }

    requestResetPassword(payload) {
        return this.http.post(this.accessPointUrl + '/request-reset-password', payload, {
            headers: this.headers,
        });
    }

    resetPassword(payload) {
        return this.http.post(this.accessPointUrl + '/reset-password', payload, {
            headers: this.headers,
        });
    }

    validateResetPasswordToken(token: string) {
        return this.http.post(
            this.accessPointUrl + '/validate-reset-password-token', {
                token: decodeURIComponent(token)
            }, {
                headers: this.headers
            }
        );
    }

    updatePassword(payload) {
        return this.http.post(this.accessPointUrl + '/newpassword', payload, {
            headers: this.headers,
        });
    }

    checkemailexists(payload) {
        return this.http.get(
            this.accessPointUrl + '/checkemailexists/' + payload, {
                headers: this.headers,
            }
        );
    }
}
