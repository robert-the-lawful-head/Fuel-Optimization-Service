import { Component, Input, AfterViewInit, OnDestroy, OnInit } from "@angular/core";
import { FbopricesService } from "../../../services/fboprices.service";
import { SharedService } from "../../../layouts/shared-service";

import * as SharedEvents from "../../../models/sharedEvents";
import { Subscription } from "rxjs";

// Interfaces

@Component({
    selector: "app-fbo-prices-panel",
    templateUrl: "./fbo-prices-panel.component.html",
    styleUrls: ["./fbo-prices-panel.component.scss"],
})
export class FboPricesPanelComponent implements OnInit, AfterViewInit, OnDestroy {
    @Input() load = true;

    public retail = 0;
    public cost = 0;
    public isLoading = false;
    private subscription: Subscription;

    constructor(
        private fboPricesService: FbopricesService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        console.log(this.load);
        if (this.load === true) {
            this.loadFboPrices();
        } else {
            this.isLoading = true;
        }
    }

    ngAfterViewInit() {
        this.subscription = this.sharedService.valueChanged$.subscribe((value: any) => {
            if (value.message === SharedEvents.fboPricesUpdatedEvent) {
                this.isLoading = false;
                this.cost = value.JetACost;
                this.retail = value.JetARetail;
            }
        });
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    private loadFboPrices() {
        this.isLoading = true;
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
                this.isLoading = false;
            });
    }
}
