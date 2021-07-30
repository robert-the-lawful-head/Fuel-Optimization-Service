import { Component, OnInit } from '@angular/core';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import * as _ from 'lodash';
import * as moment from 'moment';
import { NgxUiLoaderService } from 'ngx-ui-loader';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';

@Component({
    selector: 'app-analytics-customer-breakdown',
    styleUrls: ['./analytics-customer-breakdown-chart.component.scss'],
    templateUrl: './analytics-customer-breakdown-chart.component.html',
})
export class AnalyticsCustomerBreakdownChartComponent implements OnInit {
    public filterStartDate: Date;
    public filterEndDate: Date;

    // Public Members
    public chartName = 'customer-breakdown-chart';
    public totalOrdersData: any[];
    public chartType: string;
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
        this.chartType = 'order';
        this.refreshData();
    }

    public refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fuelreqsService
            .getFBOCustomersBreakdown(this.sharedService.currentUser.fboId, this.filterStartDate, this.filterEndDate, this.chartType)
            .subscribe((data: any) => {
                this.totalOrdersData = this.switchDataType(data);
            }, () => {
            }, () => {
                this.ngxLoader.stopLoader(this.chartName);
            });
    }

    public changeType(event: MatButtonToggleChange) {
        this.chartType = event.value;
        this.totalOrdersData = this.switchDataType(this.totalOrdersData);
    }

    private switchDataType(data: string[]) {
        return _.map(data, (item: any) => {
            let newItem: any;
            if (this.chartType === 'order') {
                newItem = _.assign({}, item, {value: item.orders});
            } else {
                newItem = _.assign({}, item, {value: item.volume});
            }
            return newItem;
        });
    }
}
