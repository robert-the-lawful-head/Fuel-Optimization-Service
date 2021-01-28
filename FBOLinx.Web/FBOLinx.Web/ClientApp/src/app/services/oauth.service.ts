import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({providedIn: 'root'})
export class OAuthService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/oauth';
    }

    public login(username: string, password: string, partnerId: string) {
        return this.http.post(this.accessPointUrl + '/login',
            {
                username,
                password,
                partnerId,
            },
            {headers: this.headers}
        );
    }
}
