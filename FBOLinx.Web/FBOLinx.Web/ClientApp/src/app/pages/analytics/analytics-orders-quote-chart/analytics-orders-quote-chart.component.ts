import { Component, OnInit } from '@angular/core';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as moment from 'moment';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-analytics-orders-quote',
    templateUrl: './analytics-orders-quote-chart.component.html',
    styleUrls: ['./analytics-orders-quote-chart.component.scss'],
})
export class AnalyticsOrdersQuoteChartComponent implements OnInit {
    public filterStartDate: Date;
    public filterEndDate: Date;
    // Public Members
    public chartName = 'orders-quote-chart';
    public ordersQuoteData: any[];
    public dollarSumData: any[];
    public colorScheme = {
        domain: [
            '#a8385d',
            '#7aa3e5',
            '#a27ea8',
            '#aae3f5',
            '#adcded',
            '#a95963',
            '#8796c0',
            '#7ed3ed',
            '#50abcc',
            '#ad6886',
        ],
    };

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService
    ) {
        this.filterStartDate = new Date(moment().add(-12, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
    }

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fuelreqsService
            .getQuotesAndOrders(
                this.sharedService.currentUser.fboId,
                this.filterStartDate,
                this.filterEndDate
            )
            .subscribe((data: any) => {
                this.ordersQuoteData = data[0];
                this.dollarSumData = data[1];
            }, () => {
            }, () => {
                this.ngxLoader.stopLoader(this.chartName);
            });
    }
}
