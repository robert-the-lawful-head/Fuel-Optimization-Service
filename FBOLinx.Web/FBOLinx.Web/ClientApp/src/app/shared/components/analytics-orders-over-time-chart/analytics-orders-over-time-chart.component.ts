import { Component, OnInit } from "@angular/core";
import * as _ from "lodash";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "app-analytics-orders-over-time",
    templateUrl: "./analytics-orders-over-time-chart.component.html",
    styleUrls: ["./analytics-orders-over-time-chart.component.scss"],
})
export class AnalyticsOrdersOverTimeChartComponent implements OnInit {
    // Public Members
    public totalOrdersData: any[];
    colorScheme = {
        domain: [
            "#5AA454",
            "#E44D25",
            "#CFC0BB",
            "#7aa3e5",
            "#a8385d",
            "#aae3f5",
        ],
    };

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        const startDate = this.sharedService.dashboardSettings.filterStartDate;
        const endDate = this.sharedService.dashboardSettings.filterEndDate;
        this.fuelreqsService
            .getOrders(this.sharedService.currentUser.fboId, startDate, endDate)
            .subscribe((data: any) => {
                this.totalOrdersData = data;
            });
    }
}
