import { Component, OnInit } from "@angular/core";
import * as _ from "lodash";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";
import { MatButtonToggleChange } from "@angular/material/button-toggle";

@Component({
    selector: "app-analytics-customer-breakdown",
    templateUrl: "./analytics-customer-breakdown-chart.component.html",
    styleUrls: ["./analytics-customer-breakdown-chart.component.scss"],
})
export class AnalyticsCustomerBreakdownChartComponent implements OnInit {
    // Public Members
    public totalOrdersData: any[];
    public chartType: string;
    public colorScheme = {
        domain: [
            "#a8385d",
            "#7aa3e5",
            "#a27ea8",
            "#aae3f5",
            "#adcded",
            "#a95963",
            "#8796c0",
            "#7ed3ed",
            "#50abcc",
            "#ad6886"
        ]
    };

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        this.chartType = "order";
        this.refreshData();
    }

    public refreshData() {
        const startDate = this.sharedService.dashboardSettings.filterStartDate;
        const endDate = this.sharedService.dashboardSettings.filterEndDate;
        this.fuelreqsService
            .getFBOCustomersBreakdown(this.sharedService.currentUser.fboId, startDate, endDate, this.chartType)
            .subscribe((data: any) => {
                this.totalOrdersData = this.switchDataType(data);
            });
    }

    public changeType(event: MatButtonToggleChange) {
        this.chartType = event.value;
        this.totalOrdersData = this.switchDataType(this.totalOrdersData);
    }

    private switchDataType(data: string[]) {
        return _.map(data, (item: any) => {
            let newItem: any;
            if (this.chartType === "order") {
                newItem = _.assign({}, item, { value: item.orders });
            } else {
                newItem = _.assign({}, item, { value: item.volume });
            }
            return newItem;
        });
    }
}
