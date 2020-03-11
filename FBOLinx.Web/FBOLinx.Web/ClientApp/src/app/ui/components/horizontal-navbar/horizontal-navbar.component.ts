import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material';

import * as _ from 'lodash';

//Services
import { UserService } from '../../../services/user.service';
import { AuthenticationService } from '../../../services/authentication.service';
import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { FboairportsService } from '../../../services/fboairports.service';
import { AcukwikairportsService } from '../../../services/acukwikairports.service';
import { FbosService } from '../../../services/fbos.service';

//Components
import { AccountProfileComponent } from '../../../shared/components/account-profile/account-profile.component';

@Component({
    moduleId: module.id,
    selector: 'horizontal-navbar',
    templateUrl: 'horizontal-navbar.component.html',
    styleUrls: ['horizontal-navbar.component.scss'],
    host: {
        '[class.app-navbar]': 'true',
        '[class.show-overlay]': 'showOverlay'
    }
})
export class HorizontalNavbarComponent implements OnInit {
    @Input()
    title: string;
    @Input()
    openedSidebar: boolean;
    @Output()
    sidebarState = new EventEmitter();
    showOverlay: boolean;
    isOpened: boolean;
    isLocationsLoaded: boolean;
    public userFullName: string;
    public customersWithoutMargins: any[];
    public accountProfileMenu: any = {isOpened: false};
    public needsAttentionMenu: any = { isOpened: false };
    public currrentJetACostPricing: any;
    public currrentJetARetailPricing: any;
    public hasLoadedJetACost: boolean = false;
    public hasLoadedJetARetail: boolean = false;
    public viewAllNotifications: boolean = false;
    public locations: any[];
    public fboAirport: any;
    public fbo: any;
    private currentUser: any;

    constructor(private authenticationService: AuthenticationService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService,
        private router: Router,
        public accountProfileDialog: MatDialog,
        private userService: UserService,
        private fboPricesService: FbopricesService,
        private fboAirportsService: FboairportsService,
        private acukwikAirportsService: AcukwikairportsService,
        private fbosService: FbosService
    ) {
        this.openedSidebar = false;
        this.showOverlay = false;
        this.isOpened = false;
        this.currentUser = this.sharedService.currentUser;
        if (!this.currentUser)
            return;
        this.userFullName = this.currentUser.firstName + ' ' + this.currentUser.lastName;
        if (this.userFullName.length < 2)
            this.userFullName = this.currentUser.username;
    }

    ngOnInit() {
        this.loadCustomersWithoutMargins(5);
        this.loadCurrentPrices();
        this.loadLocations();
        this.loadFboInfo();
    }

    toggle(event) {
        if (this.isOpened)
            this.close(event);
        else
            this.open(event);
    }

    open(event) {
        let clickedComponent = event.target.closest('.nav-item');
        let items = clickedComponent.parentElement.children;

        event.preventDefault();

        for (let i = 0; i < items.length; i++) {
            items[i].classList.remove('opened');
        }
        clickedComponent.classList.add('opened');

        this.isOpened = true;
    }

    openUpdate() {
        this.loadCustomersWithoutMargins(5);
        this.needsAttentionMenu.isOpened = !this.needsAttentionMenu.isOpened;
        
    }

    close(event) {
        event.preventDefault();
        this.accountProfileMenu.isOpened = false;
        this.needsAttentionMenu.isOpened = false;
        this.isOpened = false;
    }

    openSidebar() {
        this.openedSidebar = !this.openedSidebar;
        this.sidebarState.emit();
    }

    public logout() {
        this.authenticationService.logout();
        this.router.navigate(['/landing-site-layout']);
    }

    public accountProfileClicked(event) {
        this.userService.getCurrentUser().subscribe((data: any) => {
            const dialogRef = this.accountProfileDialog.open(AccountProfileComponent,
                {
                    height: '550px',
                    width: '650px',
                    data: data
                });
            dialogRef.afterClosed().subscribe(result => {
                if (!result)
                    return;
                console.log('Dialog data: ', result);
                this.userService.update(result).subscribe((data: any) => {
                    if (result.newPassword && result.newPassword != '') {
                        this.userService.updatePassword({ user: data, newPassword: result.newPassword }).subscribe(
                            (newPass:
                                any) => {
                                result.password = newPass;
                            });
                    }
                });
            });
        });
        this.close(event);
    }

    public stopManagingClicked(event) {
        this.sharedService.currentUser.impersonatedRole = null;
        this.sharedService.currentUser.fboId = 0;
        this.close(event);
        this.router.navigate(['/default-layout/fbos/']);
    }

    public updatePricingClicked(event) {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate(['/default-layout/fbo-prices']);
        this.close(event);
    }

    public addMarginForCustomerClicked(customer, event) {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate(['/default-layout/customers/' + customer.oid]);
        this.close(event);
    }

    public viewAllNotificationsClicked() {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate(['/default-layout/customers']);
        this.close(event);
    }

    public showLessClicked() {
        this.viewAllNotifications = false;
    }

    public loadCustomersWithoutMargins(count) {
        if (!count)
            count = 0;
        this.customerInfoByGroupService
            .getCustomersWithoutMargins(this.currentUser.groupId, this.currentUser.fboId, count)
            .subscribe((data: any) => {
                this.customersWithoutMargins = data;
            });
    }

    private loadCurrentPrices() {
        this.fboPricesService.getFbopricesByFboIdAndProductCurrent(this.currentUser.fboId, 'JetA Cost').subscribe((data: any) => {
            this.currrentJetACostPricing = data;
            this.hasLoadedJetACost = true;
        });

        this.fboPricesService.getFbopricesByFboIdAndProductCurrent(this.currentUser.fboId, 'JetA Retail').subscribe((data: any) => {
            this.currrentJetARetailPricing = data;
            this.hasLoadedJetARetail = true;
        });
    }

    private loadLocations() {
        this.acukwikAirportsService.getAllAirports().subscribe((data: any) => {
            this.isLocationsLoaded = true;
            if (data && data.length) {
                this.locations = _.cloneDeep(data);
            }
        }, (error: any) => {
            this.isLocationsLoaded = true;
            console.log(error);
        });
    }

    private loadFboInfo() {
        this.fboAirportsService.getForFbo({ oid: this.currentUser.fboId }).subscribe((data: any) => {
            this.fboAirport = _.assign({}, data);
        }, (error: any) => {
            console.log(error);
        });
        this.fbosService.get({ oid: this.currentUser.fboId }).subscribe((data: any) => {
            this.fbo = _.assign({}, data);
        }, (error: any) => {
            console.log(error);
        });
    }

    private changeLocation(location: any) {
        this.fboAirport.iata = location.iata;
        this.fboAirport.icao = location.icao;
        this.fboAirportsService.update(this.fboAirport).subscribe();
        this.accountProfileMenu.isOpened = false;
        this.needsAttentionMenu.isOpened = false;
        this.isOpened = false;
    }

    private toggleProfileMenu() {
        if (!this.isLocationsLoaded) return;
        this.accountProfileMenu.isOpened = !this.accountProfileMenu.isOpened;
    }
}
