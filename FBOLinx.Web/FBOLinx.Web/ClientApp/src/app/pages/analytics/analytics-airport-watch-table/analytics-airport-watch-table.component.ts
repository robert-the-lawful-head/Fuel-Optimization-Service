import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as moment from 'moment';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { FlightWatchHistorical } from 'src/app/models/flight-watch-historical';

@Component({
    selector: 'app-analytics-airport-watch-table',
    templateUrl: './analytics-airport-watch-table.component.html',
    styleUrls: ['./analytics-airport-watch-table.component.scss'],
})
export class AnalyticsAirportWatchTableComponent implements OnInit {
    @ViewChild(MatSort) sort: MatSort;

    public filterStartDate: Date;
    public filterEndDate: Date;
    public chartName = 'airport-watch-table';
    public displayedColumns: string[] = ['company', 'dateTime', 'tailNumber', 'flightNumber', 'hexCode', 'aircraftType', 'status', 'pastVisits', 'originated'];
    public dataSource: MatTableDataSource<FlightWatchHistorical>;

    constructor(
        private airportWatchService: AirportWatchService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
    ) {
        this.filterStartDate = new Date(moment().add(-1, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
    }

    ngOnInit() {
        this.refreshData();
    }

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.airportWatchService.getHistoricalData(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
            this.filterStartDate,
            this.filterEndDate,
        )
            .subscribe((data) => {
                this.dataSource = new MatTableDataSource(data);
                this.dataSource.sort = this.sort;
                this.dataSource.filterPredicate = (data, filter: string) =>
                    data.company.toLowerCase().includes(filter) || data.tailNumber.toLowerCase().includes(filter);
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
