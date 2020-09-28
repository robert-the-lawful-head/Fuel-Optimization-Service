import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as _ from 'lodash';
import * as moment from 'moment';

// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvent from '../../../models/sharedEvents';

@Component({
    selector: 'app-analytics-companies-quotes-deal',
    templateUrl: './analytics-companies-quotes-deal-table.component.html',
    styleUrls: ['./analytics-companies-quotes-deal-table.component.scss'],
})
export class AnalyticsCompaniesQuotesDealTableComponent implements OnInit, AfterViewInit, OnDestroy {
    public filterStartDate: Date;
    public filterEndDate: Date;
    public icao: string;
    public icaoChangedSubscription: any;
    public chartName = 'companies-quotes-deal-table';
    public displayedColumns: string[] = ['company', 'directOrders', 'companyQuotesTotal', 'conversionRate', 'totalOrders', 'airportOrders', 'lastPullDate'];
    public dataSource: any;

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService
    ) {
        this.icao = this.sharedService.currentUser.icao;
        this.filterStartDate = new Date(moment().add(-12, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
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

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fuelreqsService
            .getCompaniesQuotingDealStatistics(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.filterStartDate,
                this.filterEndDate
            )
            .subscribe((data: any) => {
                this.dataSource = new MatTableDataSource(data);
            }, () => {
            }, () => {
                this.ngxLoader.stopLoader(this.chartName);
            });
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }
}
