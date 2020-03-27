import { Component, AfterViewInit, OnDestroy } from "@angular/core";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";

import * as SharedEvents from "../../../models/sharedEvents";

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "#/default-layout",
    },
    {
        title: "Fuel Orders",
        link: "#/default-layout/fuelreqs",
    },
];

@Component({
    selector: "app-fuelreqs-home",
    templateUrl: "./fuelreqs-home.component.html",
    styleUrls: ["./fuelreqs-home.component.scss"],
})
export class FuelreqsHomeComponent implements AfterViewInit, OnDestroy {
    // Public Members
    public pageTitle = "Fuel Orders";
    public breadcrumb: any[] = BREADCRUMBS;
    public fuelreqsData: Array<any>;
    public locationChangedSubscription: any;

    constructor(
        private fuelReqService: FuelreqsService,
        private sharedService: SharedService
    ) {
        this.sharedService.emitChange(this.pageTitle);
        this.loadFuelReqData();
    }

    ngAfterViewInit() {
        this.locationChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.loadFuelReqData();
                }
            }
        );
    }

    ngOnDestroy() {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }

    public dateFilterChanged(event) {
        this.loadFuelReqData();
    }

    private loadFuelReqData() {
        this.fuelreqsData = null;
        this.fuelReqService
            .getForGroupFboAndDateRange(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.sharedService.dashboardSettings.filterStartDate,
                this.sharedService.dashboardSettings.filterEndDate
            )
            .subscribe((data: any) => {
                this.fuelreqsData = data;
            });
    }
}
