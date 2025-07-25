import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { CertificateType } from '../models';
import { CustomerInfoByGroupNote } from '../models/customer-info-by-group-note';

@Injectable()
export class CustomerinfobygroupService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/customerinfobygroup';
    }

    public getCustomerInfoByGroupListByGroupId(groupId) {
        return this.http.get(this.accessPointUrl + '/group/' + groupId + '/list', {
            headers: this.headers,
        });
    }

    public getCustomersViewModelByGroup(groupId) {
        return this.http.get(this.accessPointUrl + '/group/' + groupId + '/viewmodel', {
            headers: this.headers,
        });
    }

    public getCustomerInfoByGroupAndCustomerId(groupId, customerId) {
        return this.http.get(this.accessPointUrl + '/group/' +
            groupId +
            '/customer/' +
            customerId,
            {
                headers: this.headers,
            }
        );
    }

    public getByGroupAndFbo(groupId, fboId) {
        return this.http.get(
            this.accessPointUrl + '/group/' + groupId + '/fbo/' + fboId,
            {
                headers: this.headers,
            }
        );
    }

    public getCustomersWithContactsByGroup(groupId) {
        return this.http.get(
            `${this.accessPointUrl}/group/${groupId}/customers-with-contacts`,
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerLogger(customerId, fboId: number)
    {
        return this.http.get(`${this.accessPointUrl}/CustomerLogger/customer/${customerId}/fbo/${fboId}`, {
            headers: this.headers,
        });
    }

    public getNeedsAttentionByGroupAndFbo(groupId: number, fboId: number) {
        return this.http.get(
            `${this.accessPointUrl}/group/${groupId}/fbo/${fboId}/needs-attention`,
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerCountByGroupAndFBO(groupId, fboId) {
        return this.http.get(
            this.accessPointUrl +
                '/group/' +
                groupId +
                '/fbo/' +
                fboId +
                '/count',
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerNamesByGroupAndFBO(groupId, fboId) {
        return this.http.get(
            this.accessPointUrl +
                '/customers/group/' +
                groupId +
                '/fbo/' +
                fboId,
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
                '/group/' +
                groupId +
                '/fbo/' +
                fboId +
                '/pricingtemplate/' +
                pricingTemplateId,
            {
                headers: this.headers,
            }
        );
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public getCertificateTypes() {
        return this.http.get<CertificateType[]>(
            this.accessPointUrl + '/CertificateTypes',
            {
                headers: this.headers,
            }
        );
    }

    public getCustomerSources() {
        return this.http.get(this.accessPointUrl + '/CustomerSources', {
            headers: this.headers,
        });
    }

    public getCustomersWithoutMargins(groupId, fboId, count) {
        if (!count) {
            count = 0;
        }
        return this.http.get(
            this.accessPointUrl +
                '/group/' +
                groupId +
                '/fbo/' +
                fboId +
                '/nomargin/' +
                count.toString(),
            {
                headers: this.headers,
            }
        );
    }

    public add(payload , userId) {
        return this.http.post(this.accessPointUrl+'/'+userId, payload, {
            headers: this.headers,
        });
    }

    public remove(payload) {
        return this.http.delete(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public update(payload , userId) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid+'/'+userId, payload, {
            headers: this.headers,
        });
    }

    public matchcustomerinfo(customerId: number, groupId: number) {
        return this.http.get(
            `${this.accessPointUrl}/matchcustomerinfo/customerId/${customerId}/groupId/${groupId}`,
            {
                headers: this.headers,
            }
        );
    }

    getCustomerByGroupLogger(customerId) {
        return this.http.post(
            `${this.accessPointUrl}/GetCustomerLogger/${customerId}`,
            {
                headers: this.headers,
            }
        );
    }

    getCustomerByGroupLoggerData(oid , logType) {
        return this.http.post(
            `${this.accessPointUrl}/GetCustomerLoggerDetails/id/${oid}/logType/${logType}`,
            {
                headers: this.headers,
            }
        );
    }
    public rejectmerge(customerId: number, groupId: number) {
        return this.http.post(
            `${this.accessPointUrl}/rejectmergeforcustomer/customerId/${customerId}/groupId/${groupId}`,
            {
                headers: this.headers,
            }
        );
    }

    public acceptmerge(
        customerId: number,
        flcustomerId: number,
        groupId: number
    ) {
        return this.http.post(
            `${this.accessPointUrl}/mergecustomers/customerId/${customerId}/flcustomerid/${flcustomerId}/groupId/${groupId}`,
            {
                headers: this.headers,
            }
        );
    }

    getGroupAnalytics(groupId: number, customerId: number) {
        return this.http.post(
            `${this.accessPointUrl}/group-analytics/group/${groupId}`,
            customerId,
            {
                headers: this.headers,
            }
        );
    }

    getGroupAnalyticsAndEmail(groupId: number, customerId: number) {
        return this.http.post(
            `${this.accessPointUrl}/group-analytics/email/group/${groupId}`,
            customerId,
            {
                headers: this.headers,
            }
        );
    }

    getCustomersListByGroupAndFbo(groupId: number, fboId: number) {
        return this.http.get(
            `${this.accessPointUrl}/group/${groupId}/fbo/${fboId}/list`,
            {
                headers: this.headers,
            }
        );
    }

    getFuelVendors() {
        return this.http.get(`${this.accessPointUrl}/fuelvendors`, {
            headers: this.headers,
        });
    }

    getCustomerInfoByGroupNote(customerInfoByGroupId: number) {
        return this.http.get(`${this.accessPointUrl}/notes/` + customerInfoByGroupId, {
            headers: this.headers,
        });
    }

    getCustomerInfoByGroupNoteByCustomerIdGroupId(customerId: number, groupId: number) {
        return this.http.get(`${this.accessPointUrl}/notes/customerid/${customerId}/groupid/${groupId}`, {
            headers: this.headers,
        });
    }

    addCustomerInfoByGroupNote(payload: CustomerInfoByGroupNote) {
        return this.http.post(`${this.accessPointUrl}/notes`, payload, {
            headers: this.headers,
        });
    }

    updateCustomerInfoByGroupNote(payload: CustomerInfoByGroupNote) {

        return this.http.put(`${this.accessPointUrl}/notes/` + payload.oid, payload, {
            headers: this.headers,
        });
    }

    deleteCustomerInfoByGroupNoteById(id: number) {
        return this.http.delete(`${this.accessPointUrl}/notes/` + id, {
            headers: this.headers,
        });
    }
}
