import { Component, OnInit, Input, OnChanges, SimpleChanges } from "@angular/core";
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
export class AnalyticsCustomerBreakdownChartComponent implements OnInit, OnChanges {
    @Input() startDate: Date;
    @Input() endDate: Date;

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
            "#ad6886",
        ],
    };

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        this.chartType = "order";
        this.refreshData();
    }

    ngOnChanges(changes: SimpleChanges) {
        this.refreshData();
    }

    public refreshData() {
        this.fuelreqsService
            .getFBOCustomersBreakdown(this.sharedService.currentUser.fboId, this.startDate, this.endDate, this.chartType)
            .subscribe((data: any) => {
                this.totalOrdersData = this.switchDataType(data);
            }, (error: any) => {
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
