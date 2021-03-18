import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Subject } from 'rxjs';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { FlightWatchHistorical } from '../../../models/flight-watch-historical';
import { AircraftAssignModalComponent, NewCustomerAircraftDialogData } from '../../../shared/components/aircraft-assign-modal/aircraft-assign-modal.component';
import { CustomersListType } from '../../../models/customer';

@Component({
    selector: 'app-analytics-airport-arrivals-depatures',
    templateUrl: './analytics-airport-arrivals-depatures.component.html',
    styleUrls: ['./analytics-airport-arrivals-depatures.component.scss'],
})
export class AnalyticsAirportArrivalsDepaturesComponent implements OnInit {
    @ViewChild(MatSort) sort: MatSort;

    @Input() customers: CustomersListType[] = [];
    @Input() tailNumbers: any[] = [];
    @Output() refreshCustomers = new EventEmitter();

    public chartName = 'airport-arrivals-depatures-table';
    public displayedColumns: string[] = ['company', 'tailNumber', 'flightNumber', 'hexCode', 'aircraftType', 'aircraftTypeCode', 'dateTime', 'status'];

    public commercialAircraftTypeCodes = ['A3', 'A5'];
    public commercialAircraftFlightNumber = ['ASA', 'UPS', 'SKW', 'FDX', 'UAL', 'AAL', 'DAL', 'SWA', 'GTI'];

    public isFbolinxCustomers = true;
    public isCommercial = true;

    public data: FlightWatchHistorical[];
    public dataSource: MatTableDataSource<FlightWatchHistorical>;

    public selectedCustomers: number[] = [];
    public selectedTailNumbers: string[] = [];

    public filtersChanged: Subject<any> = new Subject<any>();

    constructor(
        public newCustomerAircraftDialog: MatDialog,
        private airportWatchService: AirportWatchService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
    ) {
        this.filtersChanged
            .debounceTime(500)
            .subscribe(() => this.refreshDataSource());
    }

    ngOnInit() {
        this.refreshData();
    }

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.airportWatchService.getArrivalsDepartures(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
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
            (!this.selectedCustomers.length || this.selectedCustomers.includes(x.customerInfoByGroupID)) &&
            (!this.selectedTailNumbers.length || this.selectedTailNumbers.includes(x.tailNumber))
        );

        this.dataSource = new MatTableDataSource(data);
        this.dataSource.sort = this.sort;
    }

    filterChanged() {
        this.filtersChanged.next();
    }

    onClickAircraft(row: FlightWatchHistorical) {
        if (!row.company) {
            const dialogRef = this.newCustomerAircraftDialog.open<AircraftAssignModalComponent, Partial<NewCustomerAircraftDialogData>>(
                AircraftAssignModalComponent,
                {
                    width: '450px',
                    data: {
                        tailNumber: row.flightNumber,
                        customers: this.customers,
                    },
                    panelClass: 'aircraft-assign-modal'
                }
            );

            dialogRef.afterClosed().subscribe((result: Partial<FlightWatchHistorical>) => {
                if (result) {
                    // const aircrafts = this.data.filter(record => record.flightNumber === row.flightNumber);
                    for (let i = 0; i < this.data.length; i++){
                        if (this.data[i].flightNumber === row.flightNumber) {
                            this.data[i] = {
                                ...this.data[i],
                                ...result,
                                tailNumber: row.flightNumber,
                            };
                        }
                    }
                    this.refreshDataSource();
                    this.refreshCustomers.emit();
                }
            });
        }
    }
}
