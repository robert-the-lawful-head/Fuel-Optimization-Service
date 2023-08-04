import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { SharedService } from 'src/app/layouts/shared-service';
import { urls } from 'src/app/constants/externalUrlsConstants';
import { AccountType } from 'src/app/enums/user-role';
import { Subscription } from 'rxjs';
import { accountTypeChangedEvent, fboChangedEvent } from 'src/app/constants/sharedEvents';

@Component({
  selector: 'app-demo-request-static-dialog',
  templateUrl: './demo-request-static-dialog.component.html',
  styleUrls: ['./demo-request-static-dialog.component.scss']
})
export class DemoRequestStaticDialogComponent implements OnInit {
    public isStaticModalVisible: boolean;
    private routerSubscription: Subscription;

    public freemiumEnaledMenuItemsTitles = [
        "About FBOLinx",
        "Orders",
        "Service Orders",
        "FBO Services & Fees",
        "Groups",
        "FBO Geofencing",
        "Antenna Status"
    ];

    public freemiumEnabledMenuItemsUrls = [
        "/default-layout/about-fbolinx",
        "/default-layout/fuelreqs",
        "/default-layout/service-orders",
        "/default-layout/rampfees",
        "/default-layout/groups",
        "/default-layout/fbo-geofencing",
        "/default-layout/antenna-status",
        "/default-layout/services-and-fees",
        "/default-layout/fbos"
    ];


    constructor(
        private router: Router,
        private sharedService: SharedService) {
            this.routerSubscription = this.router.events.subscribe((event) => {
                if (event instanceof NavigationEnd) {
                    this.isStaticModalVisible = this.getIsStaticModalVisible(event.url);
                }
            });
        }

    ngOnInit() {
        this.isStaticModalVisible = this.getIsStaticModalVisible(this.router.url);
        this.sharedService.changeEmitted$.subscribe((message) => {
            if (message === fboChangedEvent || message === accountTypeChangedEvent) {
                this.isStaticModalVisible = this.getIsStaticModalVisible(this.router.url);
            }
        });
    }
    ngOnDestroy() {
        this.routerSubscription.unsubscribe();
    }

    openRequestDemo() {
        window.open(urls.demoRequestUrl, '_blank').focus();
    }
    getIsStaticModalVisible(url: string): boolean {
        url = (url.split('/').length > 3) ? (url.split('/').splice(0, 3)).join('/') : url;
        console.log("🚀 ~ file: demo-request-static-dialog.component.ts:69 ~ DemoRequestStaticDialogComponent ~ getIsStaticModalVisible ~ url:", url)
        console.log("🚀 ~ file: demo-request-static-dialog.component.ts:71 ~ DemoRequestStaticDialogComponent ~ getIsStaticModalVisible ~ this.sharedService.currentUser.accountType:", this.sharedService.currentUser.accountType)

        if(this.sharedService.currentUser.accountType == AccountType.Premium)
            return false;
        if(this.freemiumEnabledMenuItemsUrls.includes(url))
            return false;

        return true;
    }
}
