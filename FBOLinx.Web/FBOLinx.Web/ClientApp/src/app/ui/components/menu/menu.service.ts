import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError as observableThrowError } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class MenuService {
    constructor(private http: HttpClient) {
    }

    public getData() {
        const URL = '../../../../assets/data/main-menu.json';
        return this.http.get(URL);
    }

    public handleError(error: any) {
        return observableThrowError(error.error || 'Server Error');
    }
}
