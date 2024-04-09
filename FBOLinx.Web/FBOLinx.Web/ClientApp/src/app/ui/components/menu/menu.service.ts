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

    public freemiumDisabledMenuItemsUrls = [
        "/default-layout/flight-watch",
        "/default-layout/pricing-templates",
        "/default-layout/email-templates",
        "/default-layout/customers",
        "/default-layout/analytics"
    ];

    public getData() {
        const URL = '../../../../assets/data/main-menu.json';
        return this.http.get(URL);
    }

    public handleError(error: any) {
        return observableThrowError(error.error || 'Server Error');
    }
    public setDisabledMenuItems(menuItems: IMenuItem[]): void {
        if(menuItems == null) return;
        if (this.sharedService.currentUser.accountType ==  AccountType.Premium){
            this.enableMenuItems(menuItems);
        }else{
            this.DisabledMenuItems(menuItems);
        }


    }
    private DisabledMenuItems(menuItems: IMenuItem[]): void {
        menuItems.forEach(element => {
            if(this.freemiumDisabledMenuItemsUrls.includes(element.routing))
                element.disabled = true;
            else
                element.disabled = false;
        });
    }
    private enableMenuItems(menuItems: IMenuItem[]): void {
        menuItems.forEach(element => {
                element.disabled = false;
        });
    }
}
