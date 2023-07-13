import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FbosServicesAndFeesResponse, ServicesAndFees, ServicesAndFeesResponse } from '../models/services-and-fees/services-and-fees';

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
    public getFboServicesAndFees(fboId : number) : Observable<FbosServicesAndFeesResponse[]> {
        return this.http.get<FbosServicesAndFeesResponse[]>(this.accessPointUrl + '/fbo/' +fboId, {
            headers: this.headers,
        });
    }

    public add(fboId: number , payload: ServicesAndFees) : Observable<ServicesAndFeesResponse> {
        return this.http.post<ServicesAndFeesResponse>(this.accessPointUrl + '/fbo/' + fboId, payload, {
            headers: this.headers,
        });
    }

    public remove(servicesAndFeesId: number) : Observable<void>{
        return this.http.delete<void>(this.accessPointUrl + '/' + servicesAndFeesId, {
            headers: this.headers,
        });
    }

    public update(fboId: number, payload:ServicesAndFees) : Observable<ServicesAndFeesResponse> {
        return this.http.put<ServicesAndFeesResponse>(this.accessPointUrl + '/fbo/' + fboId, payload, {
            headers: this.headers,
        });
    }
}
