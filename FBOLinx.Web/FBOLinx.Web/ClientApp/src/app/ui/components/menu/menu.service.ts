import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError as observableThrowError } from 'rxjs';
import { IMenuItem } from './menu-item';
import { AccountType } from 'src/app/enums/user-role';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/layouts/shared-service';

@Injectable({
    providedIn: 'root',
})
export class MenuService {
    constructor(
        private http: HttpClient,
        private router: Router,
        private sharedService: SharedService
        ) {}

    public freemiumActiveMenuItems = ["Dashboard","About FBOLinx","Orders","Service Orders","FBO Services & Fees"];

    public getData() {
        const URL = '../../../../assets/data/main-menu.json';
        return this.http.get(URL);
    }

    public handleError(error: any) {
        return observableThrowError(error.error || 'Server Error');
    }

    public setDisabledMenuItems(menuItems: IMenuItem[]): void {
        if(this.router.url == "/default-layout/groups") return;
        if(this.sharedService.currentUser.accountType !=  AccountType.Freemium) return;
        menuItems.forEach(element => {
            if(this.freemiumActiveMenuItems.includes(element.title))
                element.disabled = false;
            else
                element.disabled = true;
        });
    }
}
