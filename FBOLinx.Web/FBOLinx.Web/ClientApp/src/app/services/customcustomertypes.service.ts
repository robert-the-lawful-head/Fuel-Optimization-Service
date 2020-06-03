import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable()
export class CustomcustomertypesService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
        this.headers = new HttpHeaders({
            "Content-Type": "application/json; charset=utf-8",
        });
        this.accessPointUrl = baseUrl + "api/customcustomertypes";
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + "/" + payload.oid, {
            headers: this.headers,
        });
    }

    public getForFboAndCustomer(fboId, customerId) {
        return this.http.get(
            this.accessPointUrl + "/fbo/" + fboId + "/customer/" + customerId,
            {
                headers: this.headers,
            }
        );
    }

    public add(payload) {
        return this.http.post(this.accessPointUrl, payload, {
            headers: this.headers,
        });
    }

    public remove(payload) {
        return this.http.delete(this.accessPointUrl + "/" + payload.oid, {
            headers: this.headers,
        });
    }

    public update(payload) {
        return this.http.put(this.accessPointUrl + "/" + payload.oid, payload, {
            headers: this.headers,
        });
    }

    public updateForFboAndCustomer(payload) {
        return this.http.put(this.accessPointUrl + "/update", payload, {
            headers: this.headers,
        });
    }

    public updateCollection(payload) {
        return this.http.post(this.accessPointUrl + "/collection", payload, {
            headers: this.headers,
        });
    }

    public updateDefaultTemplate(payload) {
        return this.http.post(
            this.accessPointUrl + "/updatedefaulttemplate",
            payload,
            {
                headers: this.headers,
            }
        );
    }
}
