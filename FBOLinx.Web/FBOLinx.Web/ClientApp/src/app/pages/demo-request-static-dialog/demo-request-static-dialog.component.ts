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

    public freemiumDisableMenuItemsRoutes = [
        "/default-layout/dashboard-fbo-updated",
        "/default-layout/dashboard-csr",
        "/default-layout/flight-watch",
        "/default-layout/pricing-templates",
        "/default-layout/email-templates",
        "/default-layout/customers",
        "/default-layout/analytics"
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

        if(this.sharedService.currentUser.accountType == AccountType.Premium)
            return false;
        if(this.freemiumDisableMenuItemsRoutes.includes(url))
            return true;

        return false;
    }
}
