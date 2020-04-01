import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable()
export class CustomerinfobygroupService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
        this.headers = new HttpHeaders({
            "Content-Type": "application/json; charset=utf-8",
        });
        this.accessPointUrl = baseUrl + "api/customerinfobygroup";
    }

    public getByGroup(groupId) {
        return this.http.get(this.accessPointUrl + "/group/" + groupId, {
            headers: this.headers,
        });
    }

    public getByGroupAndFbo(groupId, fboId) {
        return this.http.get(
            this.accessPointUrl + "/group/" + groupId + "/fbo/" + fboId,
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerCountByGroupAndFBO(groupId, fboId) {
        return this.http.get(
            this.accessPointUrl +
                "/group/" +
                groupId +
                "/fbo/" +
                fboId +
                "/count",
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerNamesByGroupAndFBO(groupId, fboId) {
        return this.http.get(
            this.accessPointUrl + "/customers/group/" + groupId + "/fbo/" + fboId,
            {
                headers: this.headers,
            }
        );
    }

    public getCustomersByGroupAndFBOAndPricing(
        groupId,
        fboId,
        pricingTemplateId
    ) {
        return this.http.get(
            this.accessPointUrl +
                "/group/" +
                groupId +
                "/fbo/" +
                fboId +
                "/pricingtemplate/" +
                pricingTemplateId,
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

    public getCertificateTypes() {
        return this.http.get(this.accessPointUrl + "/CertificateTypes", {
            headers: this.headers,
        });
    }

    public getCustomerSources() {
        return this.http.get(this.accessPointUrl + "/CustomerSources", {
            headers: this.headers,
        });
    }

    public getCustomersWithoutMargins(groupId, fboId, count) {
        if (!count) {
            count = 0;
        }
        return this.http.get(
            this.accessPointUrl +
                "/group/" +
                groupId +
                "/fbo/" +
                fboId +
                "/nomargin/" +
                count.toString(),
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
}
