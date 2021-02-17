import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as moment from 'moment';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportWatchService } from '../../../services/airportwatch.service';

@Component({
    selector: 'app-analytics-airport-watch-table',
    templateUrl: './analytics-airport-watch-table.component.html',
    styleUrls: ['./analytics-airport-watch-table.component.scss'],
})
export class AnalyticsAirportWatchTableComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild(MatSort) sort: MatSort;

    public filterStartDate: Date;
    public filterEndDate: Date;
    public chartName = 'airport-watch-table';
    public displayedColumns: string[] = ['company', 'directOrders', 'companyQuotesTotal', 'conversionRate', 'totalOrders', 'airportOrders', 'lastPullDate'];
    public dataSource: MatTableDataSource<any[]>;

    constructor(
        private airportWatchService: AirportWatchService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
    ) {
        this.filterStartDate = new Date(moment().add(-12, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().add(7, 'd').format('MM/DD/YYYY'));
    }

    ngOnInit() {
        this.refreshData();
    }

    ngAfterViewInit() {
    }

    ngOnDestroy() {
    }

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.airportWatchService.getHistoricalData(this.sharedService.currentUser.groupId)
            .subscribe((data: any) => {
                console.log(data);
                // this.dataSource = new MatTableDataSource(data);
                // this.dataSource.sortingDataAccessor = (item, property) => {
                //     switch (property) {
                //         case 'lastPullDate':
                //             if (item[property] === 'N/A') {
                //                 if (this.sort.direction === 'asc') {
                //                     return new Date(8640000000000000);
                //                 } else {
                //                     return new Date(-8640000000000000);
                //                 }
                //             }
                //             return new Date(item[property]);
                //         default:
                //             return item[property];
                //     }
                // };
                // this.dataSource.sort = this.sort;
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
