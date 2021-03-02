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

    public chartName = 'airport-watch-table';
    public displayedColumns: string[] = ['company', 'dateTime', 'tailNumber', 'flightNumber', 'hexCode', 'aircraftType', 'status', 'pastVisits',
        //'originated'
    ];

    public filterStartDate: Date;
    public filterEndDate: Date;
    public isFuelerlinx: boolean;
    public isCommercial: boolean;

    public filterValue: string;
    public data: FlightWatchHistorical[];
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
                this.data = data;

                this.refreshDataSource();
            }, () => {
            }, () => {
                this.ngxLoader.stopLoader(this.chartName);
            });
    }

    refreshDataSource() {
        let data = this.data.filter(x => !this.isFuelerlinx || x.isFuelerlinx === this.isFuelerlinx);
        data = data.filter(x => !this.isCommercial);
        this.dataSource = new MatTableDataSource(data);
        this.dataSource.sort = this.sort;
        this.dataSource.filterPredicate = (data, filter: string) =>
            data.company.toLowerCase().includes(filter) || data.tailNumber.toLowerCase().includes(filter);
        this.dataSource.filter = this.filterValue;
    }

    applyFilter(event: Event) {
        this.filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
        this.dataSource.filter = this.filterValue;
    }

    toggleFilter() {
        this.refreshDataSource();
    }
}
