import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import * as moment from 'moment';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvent from '../../../models/sharedEvents';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
    selector: 'app-analytics-market-share-fbo-airport',
    templateUrl: './analytics-market-share-fbo-airport-chart.component.html',
    styleUrls: ['./analytics-market-share-fbo-airport-chart.component.scss'],
})
export class AnalyticsMarketShareFboAirportChartComponent implements OnInit, AfterViewInit, OnDestroy {
    public filterStartDate: Date;
    public filterEndDate: Date;

    // Public Members
    public chartName = 'market-share-fbo-airport-chart';
    public totalOrdersData: any[];
    public icao: string;
    public icaoChangedSubscription: any;
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
        this.icao = this.sharedService.currentUser.icao;
    }

    ngOnInit() {
        this.refreshData();
    }

    ngAfterViewInit() {
        this.icaoChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvent.icaoChangedEvent) {
                    this.icao = this.sharedService.currentUser.icao;
                }
            }
        );
    }

    ngOnDestroy() {
        if (this.icaoChangedSubscription) {
            this.icaoChangedSubscription.unsubscribe();
        }
    }

    public refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fuelreqsService
            .getMarketShareFboAirport(this.sharedService.currentUser.fboId, this.filterStartDate, this.filterEndDate)
            .subscribe((data: any) => {
                this.totalOrdersData = data;
            }, () => {
            }, () => {
                this.ngxLoader.stopLoader(this.chartName);
            });
    }
}
