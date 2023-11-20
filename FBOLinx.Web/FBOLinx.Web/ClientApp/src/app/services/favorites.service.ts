import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { FboFavoriteAircraft } from '../models/favorites/favoriteAircraft';
import { FboFavoriteCompany } from '../models/favorites/favoriteCompany';

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
        var payload : FboFavoriteCompany = {
            oid:  0,
            customerInfoByGroupId: customerInfoByGroupId,
            fboId: fboId
        };

        return this.http.post(`${this.accessPointUrl}/company`,payload, {
            headers: this.headers,
        });
    }
    public saveAircraftFavorite(fboId: number,customerAircraftId: number) {
        var payload : FboFavoriteAircraft = {
            oid: 0,
            fboId: fboId,
            customerAircraftsId: customerAircraftId
        };
        return this.http.post(`${this.accessPointUrl}/aircraft`,payload, {
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
