import { Component, OnInit } from '@angular/core';
import * as _ from 'lodash';

//Services
import { AmChartsService } from '@amcharts/amcharts3-angular';
import { FbopricesService } from '../../../services/fboprices.service';
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-analytics-orders-quote',
    templateUrl: './analytics-orders-quote-chart.component.html',
    styleUrls: ['./analytics-orders-quote-chart.component.scss']
})
/** analysis-orders-quote-chart component*/
export class AnalyticsOrdersQuoteChartComponent implements OnInit {

    //Public Members
    public totalOrdersData: any[];
    colorScheme = {
        domain: ['#5AA454', '#E44D25', '#CFC0BB', '#7aa3e5', '#a8385d', '#aae3f5']
    };

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        let startDate = this.sharedService.dashboardSettings.filterStartDate;
        let endDate = this.sharedService.dashboardSettings.filterEndDate;
        this.fuelreqsService
            .getForFboCountsAndDateRange(this.sharedService.currentUser.fboId, startDate, endDate)
            .subscribe((data: any) => {
                this.totalOrdersData = data;
                _.each(this.totalOrdersData, order => {
                    order.series = _.map(order.series, serie => {
                        serie.name = new Date(serie.year, serie.month);
                        return serie;
                    });
                });
            });
    }
}
