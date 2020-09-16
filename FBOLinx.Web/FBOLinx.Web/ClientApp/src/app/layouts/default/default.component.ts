import { Component, OnInit, Input } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import * as moment from "moment";
import { Store } from "@ngrx/store";

import { State } from "../../store/reducers";
import { getBreadcrumbs } from "../../store/selectors";

// Services
import { FbopricesService } from "../../services/fboprices.service";
import { PricingtemplatesService } from "../../services/pricingtemplates.service";
import { SharedService } from "../shared-service";

// Components
import { PricingExpiredNotificationComponent } from "../../shared/components/pricing-expired-notification/pricing-expired-notification.component";
import { fboChangedEvent, locationChangedEvent, fboPricesUpdatedEvent } from "../../models/sharedEvents";
import { Observable } from "rxjs";
import { NavigationStart, Router } from "@angular/router";
import { delay } from "rxjs/operators";

@Component({
    moduleId: module.id,
    selector: "default-layout",
    templateUrl: "default.component.html",
    styleUrls: ["../layouts.scss"],
    providers: [SharedService],
})
export class DefaultLayoutComponent implements OnInit {
    @Input() openedSidebar: boolean;

    $breadcrumb: Observable<any[]>;
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
        private store: Store<State>,
        private router: Router,
        private sharedService: SharedService,
        private fboPricesService: FbopricesService,
        private pricingTemplatesService: PricingtemplatesService,
        private expiredPricingDialog: MatDialog
    ) {
        this.openedSidebar = false;
        this.boxed = false;
        this.compress = false;
        this.menuStyle = "style-3";

        if (this.sharedService.currentUser.fboId) {
            this.pricingTemplatesService
                .getByFbo(this.sharedService.currentUser.fboId)
                .subscribe((data: any) => (this.pricingTemplatesData = data));
            sharedService.titleChanged$.subscribe((title) => {
                this.pageTitle = title;
            });
        }
    }

    ngOnInit() {
        this.loadFboPrices();
        this.checkCurrentPrices();
        this.layoutClasses = this.getClasses();

        this.sharedService.changeEmitted$.subscribe((message) => {
            if ((message === fboChangedEvent || message === locationChangedEvent) && this.sharedService.currentUser.fboId) {
                this.pricingTemplatesService
                    .getByFbo(this.sharedService.currentUser.fboId)
                    .subscribe((data: any) => (this.pricingTemplatesData = data));
            }
        });
        this.sharedService.valueChanged$.subscribe((value: any) => {
            if (value.message === fboPricesUpdatedEvent) {
                this.cost = value.JetACost;
                this.retail = value.JetARetail;
            }
        });

        this.$breadcrumb = this.store.select(getBreadcrumbs).pipe(delay(0));

        this.checkIfPricesPanelVisible(window.location.pathname);
        this.router.events.subscribe(event => {
            if (event instanceof NavigationStart) {
                this.checkIfPricesPanelVisible(event.url);
            }
        });
    }

    checkIfPricesPanelVisible (url: string) {
        const blacklist = [
            "/default-layout/groups",
            "/default-layout/fbos",
        ];
        if (blacklist.findIndex(v => v.startsWith(url)) >= 0) {
            this.hidePricesPanel = true;
        } else {
            this.hidePricesPanel = false;
        }
    }

    getClasses() {
        const menu: string = this.menuStyle;

        return {
            ["menu-" + menu]: menu,
            boxed: this.boxed,
            "compress-vertical-navbar": this.compress,
            "open-sidebar": this.openedSidebar,
            rtl: false,
        };
    }

    sidebarState() {
        this.openedSidebar = !this.openedSidebar;
    }

    public checkCurrentPrices() {
        const remindMeLaterFlag = localStorage.getItem(
            "pricingExpiredNotification"
        );
        const noThanksFlag = sessionStorage.getItem(
            "pricingExpiredNotification"
        );
        if (noThanksFlag) {
            return;
        }

        if (remindMeLaterFlag && (moment(moment().format("L")) !== moment(remindMeLaterFlag))) {
            return;
        }

        if (this.sharedService.currentUser.role === 1 || this.sharedService.currentUser.role === 4) {
            this.fboPricesService
                .checkFboExpiredPricing(this.sharedService.currentUser.fboId)
                .subscribe((data: any) => {
                    if (!data) {
                        const dialogRef = this.expiredPricingDialog.open(
                            PricingExpiredNotificationComponent,
                            {
                                data: {},
                                autoFocus: false,
                            }
                        );
                        dialogRef.afterClosed().subscribe();
                    }
                });
        }


    }

    private loadFboPrices() {
        this.fboPricesService
            .getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                for (const price of data) {
                    if (price.product === "JetA Cost") {
                        this.cost = price.price;
                    }
                    if (price.product === "JetA Retail") {
                        this.retail = price.price;
                    }
                }
            });
    }
}
