import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { LogColorCode, LogLevel } from '../models/logs/appLogs.service';

@Injectable()
export class AppService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/app';
    }

    public getVersion() {
        return this.http.get(this.accessPointUrl + '/version');
    }
    public logData(title: string, data: string, colorCode: LogColorCode = LogColorCode.Red, logLevel: LogLevel = LogLevel.Error) {
        var body = { Title: title, Data: data, logColorCode: colorCode, logLevel: logLevel };
        return this.http.post(this.accessPointUrl + '/log-data', body);
    }
}
