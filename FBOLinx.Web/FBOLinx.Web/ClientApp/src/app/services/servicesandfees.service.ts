import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface ServicesAndFees {
    oid: number;
    handlerId: number | null;
    serviceOfferedId: number | null;
    service: string;
    serviceType: string;
}

@Injectable()
export class ServicesAndFeesService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/ServicesAndFees';
    }
    public getFboServicesAndFees(fboId : number) : Observable<ServicesAndFees[]> {
        return this.http.get<ServicesAndFees[]>(this.accessPointUrl + '/fbo/' +fboId, {
            headers: this.headers,
        });
    }

    public add(fboId: number , payload: ServicesAndFees) : Observable<ServicesAndFees> {
        return this.http.post<ServicesAndFees>(this.accessPointUrl + '/fbo/' + fboId, payload, {
            headers: this.headers,
        });
    }

    public remove(servicesAndFeesId: number) : Observable<void>{
        return this.http.delete<void>(this.accessPointUrl + '/' + servicesAndFeesId, {
            headers: this.headers,
        });
    }

    public update(fboId: number, payload:ServicesAndFees) : Observable<void> {
        return this.http.put<void>(this.accessPointUrl + '/fbo/' + fboId, payload, {
            headers: this.headers,
        });
    }
}
