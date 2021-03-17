import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
// import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as moment from 'moment';
import { Subject } from 'rxjs';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { FlightWatchHistorical } from '../../../models/flight-watch-historical';
import { Input } from '@angular/core';

@Component({
    selector: 'app-analytics-airport-visits',
    templateUrl: './analytics-airport-visits.component.html',
    styleUrls: ['./analytics-airport-visits.component.scss'],
})
export class AnalyticsAirportVisitsComponent implements OnInit {
    @ViewChild(MatSort) sort: MatSort;
    // @ViewChild(MatPaginator) paginator: MatPaginator;

    @Input() customers: any[] = [];
    @Input() tailNumbers: any[] = [];

    public chartName = 'airport-visits-table';
    public displayedColumns: string[] = ['company', 'tailNumber', 'flightNumber', 'hexCode', 'aircraftType', 'aircraftTypeCode', 'dateTime', 'pastVisits'];

    public commercialAircraftTypeCodes = ['A3', 'A5'];
    public commercialAircraftFlightNumber = ['ASA', 'UPS', 'SKW', 'FDX', 'UAL', 'AAL', 'DAL', 'SWA', 'GTI'];

    public filterStartDate: Date;
    public filterEndDate: Date;
    public isFbolinxCustomers = true;
    public isCommercial = true;

    public data: FlightWatchHistorical[];
    public dataSource: MatTableDataSource<FlightWatchHistorical>;

    public selectedCustomers: number[] = [];

    public selectedTailNumbers: string[] = [];

    public filtersChanged: Subject<any> = new Subject<any>();

    constructor(
        private airportWatchService: AirportWatchService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
    ) {
        this.filterStartDate = new Date(moment().add(-1, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));

        this.filtersChanged
            .debounceTime(500)
            .subscribe(() => this.refreshDataSource());
    }

    ngOnInit() {
        this.refreshData();
    }

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.airportWatchService.getVisits(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
            {
                startDateTime: this.filterStartDate,
                endDateTime: this.filterEndDate,
            }
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
        const data = this.data.filter(x =>
            (!this.isFbolinxCustomers || x.company) &&
            (this.isCommercial ||
                !(this.commercialAircraftTypeCodes.includes(x.aircraftTypeCode) || this.commercialAircraftFlightNumber.find(startNum => x.flightNumber.startsWith(startNum)))
            ) &&
            (!this.selectedCustomers.length || this.selectedCustomers.includes(x.companyId)) &&
            (!this.selectedTailNumbers.length || this.selectedTailNumbers.includes(x.tailNumber))
        );

        this.dataSource = new MatTableDataSource(data);
        this.dataSource.sort = this.sort;
        // this.dataSource.paginator = this.paginator;
    }

    filterChanged() {
        this.filtersChanged.next();
    }
}
