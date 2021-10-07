import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { FbosService } from 'src/app/services/fbos.service';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';

@Component({
    selector: 'app-analytics-fuel-vendor-source',
    styleUrls: ['./analytics-fuel-vendor-source-chart.component.scss'],
    templateUrl: './analytics-fuel-vendor-source-chart.component.html',
})
export class AnalyticsFuelVendorSourceChartComponent implements OnInit {
    public filterStartDate: Date;
    public filterEndDate: Date;

    // Public Members
    public fbo = '';
    public chartName = 'fuel-vendor-sources-chart';
    public totalOrdersData: any[];
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
        private fbosService: FbosService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService
    ) {
        this.filterStartDate = new Date(
            moment().add(-12, 'M').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.fbosService
            .get({ oid: this.sharedService.currentUser.fboId })
            .subscribe((data: any) => {
                this.fbo = data.fbo;
            });
    }

    ngOnInit() {
        this.refreshData();
    }

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fuelreqsService
            .getFuelVendorSources(
                this.sharedService.currentUser.fboId,
                this.filterStartDate,
                this.filterEndDate
            )
            .subscribe(
                (data: any) => {
                    this.totalOrdersData = data;
                },
                () => {},
                () => {
                    this.ngxLoader.stopLoader(this.chartName);
                }
            );
    }
}
