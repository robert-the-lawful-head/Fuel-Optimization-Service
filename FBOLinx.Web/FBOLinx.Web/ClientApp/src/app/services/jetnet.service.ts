import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';


@Injectable()
export class JetNetService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/jetnet';
    }

    //Call the GetJetNetInformation method in the JetNetController
    public getJetNetInformationByTailNumber(tailNumber: string) {
            return this.http.get(this.accessPointUrl + '/' + tailNumber);
    }
}
