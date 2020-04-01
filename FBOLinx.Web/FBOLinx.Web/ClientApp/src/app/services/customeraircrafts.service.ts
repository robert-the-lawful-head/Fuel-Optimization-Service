import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable()
export class CustomeraircraftsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
        this.headers = new HttpHeaders({
            "Content-Type": "application/json; charset=utf-8",
        });
        this.accessPointUrl = baseUrl + "api/customeraircrafts";
    }

    public getCustomerAircraftsByGroupAndCustomerId(
        groupId,
        fboId,
        customerId
    ) {
        return this.http.get(
            this.accessPointUrl +
                "/group/" +
                groupId +
                "/fbo/" +
                fboId +
                "/customer/" +
                customerId,
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerAircraftsByGroup(groupId) {
        return this.http.get(this.accessPointUrl + "/group/" + groupId, {
            headers: this.headers,
        });
    }

    public getCustomerAircraftsCountByGroupId(groupId) {
        return this.http.get(
            this.accessPointUrl + "/group/" + groupId + "/count",
            {
                headers: this.headers,
            }
        );
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + "/" + payload.oid, {
            headers: this.headers,
        });
    }

    public add(payload) {
        return this.http.post(this.accessPointUrl, payload, {
            headers: this.headers,
        });
    }

    public import(payload) {
        return this.http.post(this.accessPointUrl + "/import", payload, {
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

    public updateTemplate(fboid, payload) {
        return this.http.put(this.accessPointUrl + "/fbo/" + fboid, payload, {
            headers: this.headers,
        });
    }
}
