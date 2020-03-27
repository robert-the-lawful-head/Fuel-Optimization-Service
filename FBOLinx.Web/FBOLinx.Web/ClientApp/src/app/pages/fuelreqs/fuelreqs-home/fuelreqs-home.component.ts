import { Component, AfterViewInit, OnDestroy } from "@angular/core";

import * as moment from "moment";
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
    public filterStartDate: Date;
    public filterEndDate: Date;

    constructor(
        private fuelReqService: FuelreqsService,
        private sharedService: SharedService
    ) {
        this.sharedService.emitChange(this.pageTitle);
        this.filterStartDate = new Date(
            moment().format("MM/DD/YYYY")
        );
        this.filterEndDate = new Date(
            moment().add(1, "d").format("MM/DD/YYYY")
        );
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
        this.filterStartDate = event.filterStartDate;
        this.filterEndDate = event.filterEndDate;
        this.loadFuelReqData();
    }

    private loadFuelReqData() {
        this.fuelreqsData = null;
        this.fuelReqService
            .getForGroupFboAndDateRange(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.filterStartDate,
                this.filterEndDate
            )
            .subscribe((data: any) => {
                this.fuelreqsData = data;
            });
    }
}
