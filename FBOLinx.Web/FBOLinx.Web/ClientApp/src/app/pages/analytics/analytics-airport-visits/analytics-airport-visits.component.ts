import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as moment from 'moment';
import { Subject } from 'rxjs';
import * as XLSX from 'xlsx';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { FlightWatchHistorical } from '../../../models/flight-watch-historical';
import { CustomersListType } from '../../../models/customer';
import { AircraftAssignModalComponent, NewCustomerAircraftDialogData } from '../../../shared/components/aircraft-assign-modal/aircraft-assign-modal.component';
import { AircraftIcons } from '../../flight-watch/flight-watch-map/aircraft-icons';
import { CsvExportModalComponent, ICsvExportModalData } from 'src/app/shared/components/csv-export-modal/csv-export-modal.component';

@Component({
    selector: 'app-analytics-airport-visits',
    templateUrl: './analytics-airport-visits.component.html',
    styleUrls: ['./analytics-airport-visits.component.scss'],
})
export class AnalyticsAirportVisitsComponent implements OnInit {
    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;

    @Input() customers: CustomersListType[] = [];
    @Input() tailNumbers: any[] = [];
    @Output() refreshCustomers = new EventEmitter();

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

    public aircraftTypes = AircraftIcons;

    constructor(
        public newCustomerAircraftDialog: MatDialog,
        public exportDialog: MatDialog,
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

    fetchData(startDate: Date, endDate: Date) {
        return this.airportWatchService.getVisits(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
            {
                startDateTime: startDate,
                endDateTime: endDate,
            }
        );
    }

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fetchData(this.filterStartDate, this.filterEndDate)
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
        this.dataSource.paginator = this.paginator;
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
                                pastVisits: 0,
                            };
                        }
                    }
                    this.refreshDataSource();
                    this.refreshCustomers.emit();
                }
            });
        }
    }

    onExport() {
        const dialogRef = this.exportDialog.open<CsvExportModalComponent, ICsvExportModalData, ICsvExportModalData>(
            CsvExportModalComponent,
            {
                data: {
                    title: 'Export Airport Departures and Arrivals',
                    filterStartDate: this.filterStartDate,
                    filterEndDate: this.filterEndDate,
                },
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.exportCsv(result.filterStartDate, result.filterEndDate);
        });
    }

    exportCsv(startDate: Date, endDate: Date) {
        this.fetchData(startDate, endDate)
            .subscribe((data) => {
                const exportData = data.map((item) => ({
                    Company: item.company,
                    'Tail #': item.tailNumber,
                    'Flight #': item.flightNumber,
                    'Hex #': item.hexCode,
                    Aircraft: item.aircraftType,
                    'Aircraft Type': this.aircraftTypes[item.aircraftTypeCode].label,
                    'Last Seen': item.dateTime,
                    'Past Visits': item.pastVisits,
                }));
                const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
                const wb: XLSX.WorkBook = XLSX.utils.book_new();
                XLSX.utils.book_append_sheet(wb, ws, 'Visits by Company & Aircraft');

                /* save to file */
                XLSX.writeFile(wb, 'Visits by Company/Aircraft.xlsx');
            });
    }
}
