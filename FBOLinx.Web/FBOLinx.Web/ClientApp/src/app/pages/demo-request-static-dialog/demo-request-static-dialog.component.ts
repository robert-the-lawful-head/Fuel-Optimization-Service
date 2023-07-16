import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { SharedService } from 'src/app/layouts/shared-service';
import { fboChangedEvent } from 'src/app/constants/sharedEvents';
import { urls } from 'src/app/constants/externalUrlsConstants';
import { AccountType } from 'src/app/enums/user-role';
import { Subscription } from 'rxjs';

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

    public freemiumEnaledMenuItemsUrls = [
        "/default-layout/about-fbolinx",
        "/default-layout/fuelreqs",
        "/default-layout/service-orders",
        "/default-layout/rampfees",
        "/default-layout/groups",
        "/default-layout/fbo-geofencing",
        "/default-layout/antenna-status"];


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
            if (message === fboChangedEvent) {
                this.isStaticModalVisible = this.getIsStaticModalVisible(this.router.url);
            }
        });
    }

    openRequestDemo() {
        window.open(urls.demoRequestUrl, '_blank').focus();
    }
    getIsStaticModalVisible(url: string): boolean {
        if(this.sharedService.currentUser.accountType == AccountType.Premium)
            return false;
        if(this.freemiumEnaledMenuItemsUrls.includes(url))
            return false;

        return true;
    }
}
