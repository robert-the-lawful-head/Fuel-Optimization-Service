import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError as observableThrowError } from 'rxjs';
import { IMenuItem } from './menu-item';
import { AccountType } from 'src/app/enums/user-role';
import { SharedService } from 'src/app/layouts/shared-service';

@Injectable({
    providedIn: 'root',
})
export class MenuService {
    constructor(
        private http: HttpClient,
        private sharedService: SharedService
        ) {}

    public freemiumEnaledMenuItemsTitles = [
        "Dashboard",
        "CSR Dashboard",
        "About FBOLinx",
        "Orders",
        "Service Orders",
        "FBO Services & Fees",
        "Groups",
        "FBO Geofencing",
        "Antenna Status"
    ];

    public getData() {
        const URL = '../../../../assets/data/main-menu.json';
        return this.http.get(URL);
    }

    public handleError(error: any) {
        return observableThrowError(error.error || 'Server Error');
    }
    public setDisabledMenuItems(menuItems: IMenuItem[]): void {
        if (this.sharedService.currentUser.accountType ==  AccountType.Premium) return;

        menuItems.forEach(element => {
            if(this.freemiumEnaledMenuItemsTitles.includes(element.title))
                element.disabled = false;
            else
                element.disabled = true;
        });
    }
}
