import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NavigationStart, Router, RouterEvent } from '@angular/router';
import { Store } from '@ngrx/store';
import * as moment from 'moment';
import { filter } from 'rxjs/operators';

import {
    fboChangedEvent,
    fboPricesUpdatedEvent,
    locationChangedEvent,
} from '../../models/sharedEvents';
// Services
import { FbopricesService } from '../../services/fboprices.service';
import { PricingtemplatesService } from '../../services/pricingtemplates.service';
// Components
import { PricingExpiredNotificationComponent } from '../../shared/components/pricing-expired-notification/pricing-expired-notification.component';
import { customerGridClear } from '../../store/actions';
import { State } from '../../store/reducers';
import { SharedService } from '../shared-service';

@Component({
    providers: [SharedService],
    selector: 'default-layout',
    styleUrls: ['../layouts.scss'],
    templateUrl: 'default.component.html',
})
export class DefaultLayoutComponent implements OnInit {
    @Input() openedSidebar: boolean;

    pageTitle: any;
    boxed: boolean;
    compress: boolean;
    menuStyle: string;
    layoutClasses: any;
    pricingTemplatesData: any[];
    retail: number;
    cost: number;

    hidePricesPanel: boolean;

    constructor(
        private sharedService: SharedService,
        private fboPricesService: FbopricesService,
        private pricingTemplatesService: PricingtemplatesService,
        private expiredPricingDialog: MatDialog,
        private router: Router,
        private store: Store<State>
    ) {
        this.openedSidebar = false;
        this.boxed = false;
        this.compress = false;
        this.menuStyle = 'style-3';

        if (this.sharedService.currentUser.fboId) {
            this.pricingTemplatesService
                .getByFbo(
                    this.sharedService.currentUser.fboId,
                    this.sharedService.currentUser.groupId
                )
                .subscribe((data: any) => (this.pricingTemplatesData = data));
            sharedService.titleChanged$.subscribe((title) => {
                setTimeout(() => (this.pageTitle = title), 100);
            });
        }

        this.router.events
            .pipe(filter((event) => event instanceof NavigationStart))
            .subscribe((event: RouterEvent) => {
                if (!event.url.startsWith('/default-layout/customers')) {
                    this.store.dispatch(customerGridClear());
                }
            });
    }

    get isCsr() {
        return this.sharedService.currentUser.role === 5;
    }

    ngOnInit() {
        if (this.canUserSeePricing()) {
            this.loadFboPrices();
            this.checkCurrentPrices();
        }

        this.sharedService.changeEmitted$.subscribe((message) => {
            if (!this.canUserSeePricing()) {
                return;
            }
            if (
                (message === fboChangedEvent ||
                    message === locationChangedEvent) &&
                this.sharedService.currentUser.fboId
            ) {
                this.pricingTemplatesService
                    .getByFbo(
                        this.sharedService.currentUser.fboId,
                        this.sharedService.currentUser.groupId
                    )
                    .subscribe(
                        (data: any) => (this.pricingTemplatesData = data)
                    );
            }
        });
        this.sharedService.valueChanged$.subscribe((value: any) => {
            if (!this.canUserSeePricing()) {
                return;
            }
            if (value.message === fboPricesUpdatedEvent) {
                this.cost = value.JetACost;
                this.retail = value.JetARetail;
            }
        });

        this.layoutClasses = this.getClasses();
    }

    isPricePanelVisible() {
        const blacklist = [
            '/default-layout/groups',
            '/default-layout/fbos',
            '/default-layout/group-analytics',
        ];
        if (
            blacklist.findIndex((v) =>
                window.location.pathname.startsWith(v)
            ) >= 0
        ) {
            return false;
        }
        return !this.isCsr;
    }

    getClasses() {
        const menu: string = this.menuStyle;

        return {
            ['menu-' + menu]: menu,
            boxed: this.boxed,
            'compress-vertical-navbar': this.compress,
            'open-sidebar': this.openedSidebar,
            rtl: false,
        };
    }

    sidebarState() {
        this.openedSidebar = !this.openedSidebar;
    }

    checkCurrentPrices() {
        if (!this.sharedService.currentUser.fboId) {
            return;
        }
        const remindMeLaterFlag = localStorage.getItem(
            'pricingExpiredNotification'
        );
        const noThanksFlag = sessionStorage.getItem(
            'pricingExpiredNotification'
        );
        if (noThanksFlag) {
            return;
        }

        if (
            remindMeLaterFlag &&
            moment(new Date(moment().format('L'))) !==
                moment(new Date(remindMeLaterFlag))
        ) {
            return;
        }

        this.fboPricesService
            .checkFboExpiredPricing(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                if (!data) {
                    const dialogRef = this.expiredPricingDialog.open(
                        PricingExpiredNotificationComponent,
                        {
                            autoFocus: false,
                            data: {},
                        }
                    );
                    dialogRef.afterClosed().subscribe();
                }
            });
    }

    isSidebarInvisible() {
        return (
            this.sharedService.currentUser.role === 3 &&
            !this.sharedService.currentUser.impersonatedRole
        );
    }

    private loadFboPrices() {
        this.fboPricesService
            .getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                for (const price of data) {
                    if (price.product === 'JetA Cost') {
                        this.cost = price.price;
                    }
                    if (price.product === 'JetA Retail') {
                        this.retail = price.price;
                    }
                }
            });
    }

    private canUserSeePricing(): boolean {
        return (
            [1, 4].includes(this.sharedService.currentUser.role) ||
            [1, 4].includes(this.sharedService.currentUser.impersonatedRole)
        );
    }
}
