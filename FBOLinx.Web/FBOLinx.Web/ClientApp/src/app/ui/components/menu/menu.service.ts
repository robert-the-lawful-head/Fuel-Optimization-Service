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
    public setMenuProps(menuItems: IMenuItem[]): void {
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
            
            element.class = this.getLiClass(element);

        });
    }
    private enableMenuItems(menuItems: IMenuItem[]): void {
        menuItems.forEach(element => {
                element.disabled = false;
                element.class = this.getLiClass(element);
        });
    }
    private getLiClass(item: any) {
        let role = this.sharedService.currentUser.role;
        if (this.sharedService.currentUser.impersonatedRole) {
            role = this.sharedService.currentUser.impersonatedRole;
        }

        const hidden = item.roles && item.roles.indexOf(role) === -1;
        const isDisabled = (this.sharedService.currentUser.accountType == AccountType.Premium || !this.sharedService.currentUser?.accountType) ? false : item.disabled;
        return {
            active: item.active || false,  // Active class will be set in the template based on rla.isActive
            disabled: isDisabled,
            'has-sub': item.sub,
            hidden,
            'menu-item-group': item.groupTitle,
          };
    
    }
}
