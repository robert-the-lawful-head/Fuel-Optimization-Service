import { Component, AfterViewInit, OnDestroy } from "@angular/core";

import * as _ from "lodash";
import * as moment from "moment";
import * as XLSX from "xlsx";

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

    public export (event) {
        this.fuelReqService
            .getForGroupFboAndDateRange(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                event.filterStartDate,
                event.filterEndDate
            )
            .subscribe((data: any) => {
                const exportData = _.map(data, (item) => {
                    return {
                        ID: item.oid,
                        "Flight Dept.": item.customerName,
                        ETA: item.eta,
                        ICAO: item.icao,
                        "Tail #": item.tailNumber,
                        FBO: item.fboName,
                        Notes: item.notes,
                        Source: item.source,
                    };
                });
                const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
                const wb: XLSX.WorkBook = XLSX.utils.book_new();
                XLSX.utils.book_append_sheet(wb, ws, "Fuel Orders");

                /* save to file */
                XLSX.writeFile(wb, "FuelOrders.xlsx");
            });
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
