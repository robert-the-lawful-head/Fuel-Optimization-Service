import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { PricingTemplate } from '../models';

@Injectable()
export class PricingtemplatesService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/pricingtemplates';
    }

    public getByFbo(fboId, groupId?) {
        return this.http.get<PricingTemplate[]>(
            this.accessPointUrl + '/group/' + groupId + '/fbo/' + fboId,
            { headers: this.headers }
        );
    }

    public getWithEmailContentByFbo(fboId, groupId?) {
        return this.http.get<PricingTemplate[]>(
            this.accessPointUrl +
                '/with-email-content/group/' +
                groupId +
                '/fbo/' +
                fboId,
            { headers: this.headers }
        );
    }

    public getByFboDefaultTemplate(groupId, fboId) {
        return this.http.get(
            this.accessPointUrl +
                '/fbodefaultpricingtemplate/group/' +
                groupId +
                '/fbo/' +
                fboId,
            { headers: this.headers }
        );
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public add(payload) {
        return this.http.post(this.accessPointUrl, payload, {
            headers: this.headers,
        });
    }

    public remove(payload) {
        return this.http.delete(
            this.accessPointUrl + '/' + payload.oid + '/fbo/' + payload.fboId,
            {
                headers: this.headers,
            }
        );
    }

    public update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }

    public getcostpluspricingtemplates(payload) {
        return this.http.get(
            this.accessPointUrl + '/getcostpluspricingtemplate/' + payload,
            {
                headers: this.headers,
            }
        );
    }

    public copy(payload) {
        return this.http.post(
            this.accessPointUrl + '/copypricingtemplate/',
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public checkdefaultpricingtemplates(payload) {
        return this.http.get(
            this.accessPointUrl + '/checkdefaulttemplate/' + payload,
            {
                headers: this.headers,
            }
        );
    }

    public getFileAttachmentName(pricingTemplateId) {
        return this.http.get(
            this.accessPointUrl + '/fileattachmentname/' +
            pricingTemplateId,
            {
                headers: this.headers,
                responseType: 'text',
            }
        );
    }

    public uploadFileAttachment(payload) {
        return this.http.post(
            this.accessPointUrl + '/uploadfileattachment/',
            payload,
            {
                headers: this.headers,
                responseType: 'text',
            }
        );
    }

    public downloadFileAttachment(pricingTemplateId) {
        return this.http.get(
            this.accessPointUrl + '/fileattachment/' + 
            pricingTemplateId,
            {
                headers: this.headers,
                responseType: 'text',
            }
        );
    }

    public deleteFileAttachment(pricingTemplateId) {
        return this.http.delete(
            this.accessPointUrl + '/fileattachment/' + pricingTemplateId,
            {
                headers: this.headers,
            }
        );
    }
}
