import { Component, OnInit, Input, OnChanges, SimpleChanges } from "@angular/core";
import * as _ from "lodash";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "app-analytics-orders-quote",
    templateUrl: "./analytics-orders-quote-chart.component.html",
    styleUrls: ["./analytics-orders-quote-chart.component.scss"],
})
export class AnalyticsOrdersQuoteChartComponent implements OnInit, OnChanges {
    @Input() startDate: Date;
    @Input() endDate: Date;
    // Public Members
    public ordersQuoteData: any[];
    public dollarSumData: any[];
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
            .getQuotesAndOrders(
                this.sharedService.currentUser.fboId,
                this.startDate,
                this.endDate
            )
            .subscribe((data: any) => {
                this.ordersQuoteData = data[0];
                this.dollarSumData = data[1];
                // _.each(this.ordersQuoteData, (order) => {
                //     order.series = _.map(order.series, (serie) => {
                //         serie.name = new Date(serie.year, serie.month);
                //         return serie;
                //     });
                // });
                // _.each(this.dollarSumData, (order) => {
                //     order.series = _.map(order.series, (serie) => {
                //         serie.name = new Date(serie.year, serie.month);
                //         return serie;
                //     });
                // })
            }, (error: any) => {
            });
    }
}
