import { Component, OnInit, Input } from '@angular/core';
import * as _ from 'lodash';

// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';
import { MatTableDataSource } from '@angular/material/table';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
    selector: 'app-analytics-companies-quotes-deal',
    templateUrl: './analytics-companies-quotes-deal-table.component.html',
    styleUrls: ['./analytics-companies-quotes-deal-table.component.scss'],
})
export class AnalyticsCompaniesQuotesDealTableComponent implements OnInit {
    @Input() startDate: Date;
    @Input() endDate: Date;

    // Public Members
    public chartName = 'companies-quotes-deal-table';
    public displayedColumns: string[] = ['company', 'directOrders', 'companyQuotesTotal', 'conversionRate', 'totalOrders', 'airportOrders', 'lastPullDate'];
    public dataSource: any;

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService
    ) {}

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fuelreqsService
            .getCompaniesQuotingDealStatistics(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.startDate,
                this.endDate
            )
            .subscribe((data: any) => {
                this.dataSource = new MatTableDataSource(data);
            }, () => {
            }, () => {
                this.ngxLoader.stopLoader(this.chartName);
            });
    }

    public applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }
}
