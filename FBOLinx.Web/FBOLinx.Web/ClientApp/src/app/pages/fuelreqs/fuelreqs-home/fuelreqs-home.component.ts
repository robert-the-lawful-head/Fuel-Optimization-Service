import { Component, OnDestroy } from "@angular/core";

import * as _ from "lodash";
import * as moment from "moment";
import * as XLSX from "xlsx";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";

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
export class FuelreqsHomeComponent implements OnDestroy {
    // Public Members
    public pageTitle = "Fuel Orders";
    public breadcrumb: any[] = BREADCRUMBS;
    public fuelreqsData: Array<any>;
    public filterStartDate: Date;
    public filterEndDate: Date;
    public timer: any;

    constructor(
        private fuelReqService: FuelreqsService,
        private sharedService: SharedService
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.filterStartDate = new Date(
            moment().format("MM/DD/YYYY")
        );
        this.filterEndDate = new Date(
            moment().add(1, "d").format("MM/DD/YYYY")
        );
        this.startFuelReqDataServe(fuelReqService);
    }

    ngOnDestroy() {
        this.stopFuelReqDataServe();
    }

    public startFuelReqDataServe(fuelReqService: FuelreqsService) {
        fuelReqService
            .getForGroupFboAndDateRange(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.filterStartDate,
                this.filterEndDate
            )
            .subscribe((data: any) => {
                this.fuelreqsData = data;
                this.timer = setTimeout(() => {
                    this.startFuelReqDataServe(fuelReqService);
                }, 5000);
            });
    }

    public restartFuelReqDataServe() {
        this.stopFuelReqDataServe();
        this.startFuelReqDataServe(this.fuelReqService);
    }

    public stopFuelReqDataServe() {
        if (this.timer) {
            clearTimeout(this.timer);
        }
    }

    public dateFilterChanged(event) {
        this.filterStartDate = event.filterStartDate;
        this.filterEndDate = event.filterEndDate;
        this.restartFuelReqDataServe();
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
}
