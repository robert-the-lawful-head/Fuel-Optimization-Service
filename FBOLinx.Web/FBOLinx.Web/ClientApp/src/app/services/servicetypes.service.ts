import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ServiceTypeResponse } from '../models/services-and-fees/services-and-fees';

@Injectable()
export class ServiceTypeService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/FboCustomServiceTypes';
    }
    public getServiceTypes(fboId : number) : Observable<ServiceTypeResponse[]> {
        return this.http.get<ServiceTypeResponse[]>(this.accessPointUrl + '/fbo/' +fboId, {
            headers: this.headers,
        });
    }

    public add(fboId: number , payload: ServiceTypeResponse) : Observable<ServiceTypeResponse> {
        return this.http.post<ServiceTypeResponse>(this.accessPointUrl + '/fbo/' + fboId, payload, {
            headers: this.headers,
        });
    }

    public remove(serviceTypeId: number) : Observable<void>{
        return this.http.delete<void>(this.accessPointUrl + '/' + serviceTypeId, {
            headers: this.headers,
        });
    }

    public update(serviceTypeId: number, payload:ServiceTypeResponse) : Observable<ServiceTypeResponse> {
        return this.http.put<ServiceTypeResponse>(this.accessPointUrl + '/' + serviceTypeId, payload, {
            headers: this.headers,
        });
    }
}
