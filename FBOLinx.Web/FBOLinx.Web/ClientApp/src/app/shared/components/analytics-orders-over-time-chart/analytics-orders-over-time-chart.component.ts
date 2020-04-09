import { Component, OnInit, Input, OnChanges, SimpleChanges } from "@angular/core";
import * as _ from "lodash";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "app-analytics-orders-over-time",
    templateUrl: "./analytics-orders-over-time-chart.component.html",
    styleUrls: ["./analytics-orders-over-time-chart.component.scss"],
})
export class AnalyticsOrdersOverTimeChartComponent implements OnInit, OnChanges {
    @Input() startDate: Date;
    @Input() endDate: Date;
    // Public Members
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
        private sharedService: SharedService
    ) {}

    ngOnInit() {
    }

    ngOnChanges(changes: SimpleChanges) {
        this.refreshData();
    }

    public refreshData() {
        this.fuelreqsService
            .getOrders(this.sharedService.currentUser.fboId, this.startDate, this.endDate)
            .subscribe((data: any) => {
                this.totalOrdersData = data;
            }, (error: any) => {
            });
    }
}
