import { Component, OnInit } from '@angular/core';
import { MatSliderChange } from '@angular/material/slider';
import * as moment from 'moment';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
    selector: 'app-analytics-volumes-nearby-airport',
    templateUrl: './analytics-volumes-nearby-airport-chart.component.html',
    styleUrls: ['./analytics-volumes-nearby-airport-chart.component.scss'],
})
export class AnalyticsVolumesNearbyAirportChartComponent implements OnInit {
    public filterStartDate: Date;
    public filterEndDate: Date;

    // Public Members
    public chartName: 'volumes-nearby-airport-chart';
    public totalOrdersData: any[];
    public mile = 50;
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
            .getVolumesNearbyAirport(this.sharedService.currentUser.fboId, this.filterStartDate, this.filterEndDate, this.mile)
            .subscribe((data: any) => {
                this.totalOrdersData = data;
            }, () => {
            }, () => {
                this.ngxLoader.stopLoader(this.chartName);
            });
    }

    public formatLabel(value: number) {
        return value + 'mi';
    }

    public changeMile(event: MatSliderChange) {
        this.mile = event.value;
        this.refreshData();
    }
}
