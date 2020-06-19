import { Component, OnInit, Input, AfterViewInit } from "@angular/core";
import { SharedService } from "../shared-service";
import { MatDialog } from "@angular/material/dialog";

// Services
import { FbopricesService } from "../../services/fboprices.service";
import { PricingtemplatesService } from "../../services/pricingtemplates.service";
// Components
import { PricingExpiredNotificationComponent } from "../../shared/components/pricing-expired-notification/pricing-expired-notification.component";
import * as moment from "moment";
import { fboChangedEvent } from "../../models/sharedEvents";

@Component({
    moduleId: module.id,
    selector: "default-layout",
    templateUrl: "default.component.html",
    styleUrls: ["../layouts.scss"],
    providers: [SharedService],
})
export class DefaultLayoutComponent implements OnInit, AfterViewInit {
    pageTitle: any;
    boxed: boolean;
    compress: boolean;
    menuStyle: string;
    layoutClasses: any;
    rtl: boolean;
    @Input()
    openedSidebar: boolean;
    public pricingTemplatesData: any[];
    public subscription: any;

    constructor(
        private sharedService: SharedService,
        private fboPricesService: FbopricesService,
        private pricingTemplatesService: PricingtemplatesService,
        public expiredPricingDialog: MatDialog
    ) {
        this.openedSidebar = false;
        this.boxed = false;
        this.compress = false;
        this.menuStyle = "style-3";
        this.rtl = false;

        if (this.sharedService.currentUser.fboId) {
            this.pricingTemplatesService
                .getByFbo(this.sharedService.currentUser.fboId)
                .subscribe((data: any) => (this.pricingTemplatesData = data));
            sharedService.titleChanged$.subscribe((title) => {
                this.pageTitle = title;
            });
        }
    }
    ngAfterViewInit() {
        this.subscription = this.sharedService.changeEmitted$.subscribe((message) => {
            if (message === fboChangedEvent && this.sharedService.currentUser.fboId) {
                this.pricingTemplatesService
                    .getByFbo(this.sharedService.currentUser.fboId)
                    .subscribe((data: any) => (this.pricingTemplatesData = data));
            }
        });
    }

    ngOnInit() {
        this.checkCurrentPrices();
        this.layoutClasses = this.getClasses();
    }

    getClasses() {
        const menu: string = this.menuStyle;

        return {
            ["menu-" + menu]: menu,
            boxed: this.boxed,
            "compress-vertical-navbar": this.compress,
            "open-sidebar": this.openedSidebar,
            rtl: this.rtl,
        };
    }

    sidebarState() {
        this.openedSidebar = !this.openedSidebar;
    }

    // Private Methods

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
}
