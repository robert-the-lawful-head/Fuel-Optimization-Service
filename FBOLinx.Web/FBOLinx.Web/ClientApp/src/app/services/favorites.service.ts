import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FavoritesService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/favorites';
    }
    public saveCompanyFavorite(fboId: number, customerInfoByGroupId: number) {
        return this.http.post(`${this.accessPointUrl}/fbo/${fboId}/group/${customerInfoByGroupId}`,{}, {
            headers: this.headers,
        });
    }
    public saveAircraftFavorite(fboId: number, aircraftId: number) {
        console.log(`${this.accessPointUrl}/fbo/${fboId}/aircraft/${aircraftId}`);
        return this.http.post(`${this.accessPointUrl}/fbo/${fboId}/aircraft/${aircraftId}`,{}, {
            headers: this.headers,
        });
    }
    public deleteCompanyFavorite(oid: number) {
        return this.http.delete(`${this.accessPointUrl}/company/${oid}`, {
            headers: this.headers,
        });
    }
    public deleteAircraftFavorite(oid: number) {
        return this.http.delete(`${this.accessPointUrl}/aircraft/${oid}`, {
            headers: this.headers,
        });
    }
}
