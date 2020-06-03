import { Component, OnInit, Input } from "@angular/core";
import * as _ from "lodash";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";
import { NgxUiLoaderService } from "ngx-ui-loader";

@Component({
    selector: "app-analytics-orders-over-time",
    templateUrl: "./analytics-orders-over-time-chart.component.html",
    styleUrls: ["./analytics-orders-over-time-chart.component.scss"],
})
export class AnalyticsOrdersOverTimeChartComponent implements OnInit {
    @Input() startDate: Date;
    @Input() endDate: Date;
    // Public Members
    public chartName = "orders-over-time-chart";
    public totalOrdersData: any[];
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
            "#ad6886",
        ],
    };

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService
    ) {}

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fuelreqsService
            .getOrders(this.sharedService.currentUser.fboId, this.startDate, this.endDate)
            .subscribe((data: any) => {
                this.totalOrdersData = data;
            }, () => {
            }, () => {
                this.ngxLoader.stopLoader(this.chartName);
            });
    }
}
