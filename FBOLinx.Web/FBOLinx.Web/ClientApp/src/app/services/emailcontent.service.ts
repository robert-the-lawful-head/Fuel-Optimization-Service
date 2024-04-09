import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class EmailcontentService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/EmailContents';
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public getForFbo(fboId) {
        return this.http.get(this.accessPointUrl + '/fbo/' + fboId, {
            headers: this.headers,
        });
    }

    public getForGroup(groupId: number) {
        return this.http.get(this.accessPointUrl + '/group/' + groupId, {
            headers: this.headers,
        });
    }

    public add(payload) {
        return this.http.post(this.accessPointUrl, payload, {
            headers: this.headers,
        });
    }

    public remove(payload) {
        return this.http.delete(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }

    public getFileAttachmentName(emailContentId) {
        return this.http.get(
            this.accessPointUrl + '/fileattachmentname/' +
            emailContentId,
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

    public downloadFileAttachment(emailContentId) {
        return this.http.get(
            this.accessPointUrl + '/fileattachment/' +
            emailContentId,
            {
                headers: this.headers,
                responseType: 'text',
            }
        );
    }

    public deleteFileAttachment(emailContentId) {
        return this.http.delete(
            this.accessPointUrl + '/fileattachment/' + emailContentId,
            {
                headers: this.headers,
            }
        );
    }
}
