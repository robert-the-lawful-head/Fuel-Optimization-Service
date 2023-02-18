import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import * as moment from 'moment';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Subject } from 'rxjs';
import {
    ColumnType,
    TableSettingsComponent,
} from 'src/app/shared/components/table-settings/table-settings.component';
import * as XLSX from 'xlsx';

import { isCommercialAircraft } from '../../../../utils/aircraft';
import { AIRCRAFT_IMAGES } from '../../flight-watch/flight-watch-map/aircraft-images';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { FbosService } from '../../../services/fbos.service';

//Models
import { CustomersListType } from '../../../models/customer';
import {
    FlightWatchHistorical,
} from '../../../models/flight-watch-historical';
import {
    AircraftAssignModalComponent,
    NewCustomerAircraftDialogData,
} from '../../../shared/components/aircraft-assign-modal/aircraft-assign-modal.component';
import { VirtualScrollBase } from 'src/app/services/tables/VirtualScrollBase';


@Component({
    selector: 'app-analytics-airport-arrivals-depatures',
    styleUrls: ['./analytics-airport-arrivals-depatures.component.scss'],
    templateUrl: './analytics-airport-arrivals-depatures.component.html',
})
export class AnalyticsAirportArrivalsDepaturesComponent extends VirtualScrollBase implements OnInit {
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;

    @Input() customers: CustomersListType[] = [];
    @Input() tailNumbers: any[] = [];
    @Output() refreshCustomers = new EventEmitter();

    chartName = 'airport-arrivals-depatures-table';
    displayedColumns: string[] = [
        'company',
        'tailNumber',
        'flightNumber',
        'hexCode',
        'aircraftType',
        'aircraftTypeCode',
        'dateTime',
        'status',
        'pastVisits',
        'visitsToMyFbo',
        'percentOfVisits',
    ];

    filterStartDate: Date;
    filterEndDate: Date;

    isCommercialInvisible = true;

    data: FlightWatchHistorical[];

    selectedCustomers: number[] = [];
    selectedTailNumbers: string[] = [];

    filtersChanged: Subject<any> = new Subject<any>();

    aircraftTypes = AIRCRAFT_IMAGES;

    tableLocalStorageKey: string;

    columns: ColumnType[] = [];

    fboName: string = "";

    initialColumns: ColumnType[] = [
        {
            id: 'company',
            name: 'Company',
        },
        {
            id: 'tailNumber',
            name: 'Tail #',
        },
        {
            id: 'flightNumber',
            name: 'Flight #',
        },
        {
            id: 'hexCode',
            name: 'Hex #',
        },
        {
            id: 'aircraftType',
            name: `Aircraft`,
        },
        {
            id: 'aircraftTypeCode',
            name: 'Aircraft Type',
        },
        {
            id: 'dateTime',
            name: 'Date and Time',
            sort: 'desc',
        },
        {
            id: 'status',
            name: 'Departure / Arrival',
        },
        {
            id: 'pastVisits',
            name: 'Past Visits',
        },
        {
            id: 'visitsToMyFbo',
            name: 'Visits to My FBO',
        },
        {
            id: 'percentOfVisits',
            name: 'Percent of Visits',
        },
    ];

    constructor(
        private newCustomerAircraftDialog: MatDialog,
        private exportDialog: MatDialog,
        private tableSettingsDialog: MatDialog,
        private airportWatchService: AirportWatchService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private fbosService: FbosService
    ) {
        super();
        this.filterStartDate = new Date(
            moment().add(-1, 'M').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.filtersChanged
            .debounceTime(500)
            .subscribe(() => this.refreshDataSource());
        this.initColumns();
    }

    get visibleColumns() {
        return (
            this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.id) || []
        );
    }

    ngOnInit() {
        this.getFboName();
        this.getCustomersList();
        this.sort.sortChange.subscribe(() => {
            this.columns = this.columns.map((column) =>
                column.id === this.sort.active
                    ? { ...column, sort: this.sort.direction }
                    : {
                          hidden: column.hidden,
                          id: column.id,
                          name: column.name,
                      }
            );

            this.saveSettings();
        });

        this.refreshData();
    }
    getFboName() {
        if (this.fboName && this.fboName != "")
            return;
        this.fbosService
            .get({
                oid: this.sharedService.currentUser.fboId,
            })
            .subscribe((data: any) => {
                this.fboName = data.fbo;
            });
    }

    getCustomersList() {
        if (this.customers && this.customers.length > 0)
            return;
        this.customerInfoByGroupService
            .getCustomersListByGroupAndFbo(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((customers: any[]) => {
                this.customers = customers;
            });
    }

    fetchData(startDate: Date, endDate: Date) {
        return this.airportWatchService.getArrivalsDepartures(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
            {
                endDateTime: endDate,
                startDateTime: startDate,
            }
        );
    }

    initColumns() {
        this.tableLocalStorageKey = `analytics-airport-arrivals-depatures-${this.sharedService.currentUser.fboId}`;

        if (localStorage.getItem(this.tableLocalStorageKey)) {
            this.columns = JSON.parse(
                localStorage.getItem(this.tableLocalStorageKey)
            );
            if (this.columns.length !== this.initialColumns.length) {
                this.columns = this.initialColumns;
            }
        } else {
            this.columns = this.initialColumns;
        }
    }

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fetchData(this.filterStartDate, this.filterEndDate).subscribe(
            (data: any[]) => {
                this.data = data;

                this.refreshDataSource();
            },
            () => {},
            () => {
                this.ngxLoader.stopLoader(this.chartName);
            }
        );
    }

    refreshDataSource() {
        const data = this.data
            .filter(
                (x) =>
                    (!this.isCommercialInvisible ||
                        !isCommercialAircraft(
                            x.aircraftTypeCode
                        )) &&
                    (!this.selectedCustomers.length ||
                        this.selectedCustomers.includes(
                            x.customerInfoByGroupID
                        )) &&
                    (!this.selectedTailNumbers.length ||
                        this.selectedTailNumbers.includes(x.tailNumber))
            )
            .map((x) => ({
                ...x,
                aircraftTypeCode: this.getAircraftLabel(x.aircraftTypeCode),
            }));

        this.setVirtualScrollVariables(this.paginator,this.sort,data)

        if (!this.dataSource) {
            this.dataSource.filteredData = [];
        }

    }

    refreshSort() {
        const sortedColumn = this.columns.find(
            (column) => !column.hidden && column.sort
        );
        this.sort.sort({
            disableClear: false,
            id: null,
            start: sortedColumn?.sort || 'asc',
        });
        this.sort.sort({
            disableClear: false,
            id: sortedColumn?.id,
            start: sortedColumn?.sort || 'asc',
        });
        (
            this.sort.sortables.get(sortedColumn?.id) as MatSortHeader
        )?._setAnimationTransitionState({ toState: 'active' });
    }

    filterChanged() {
        this.filtersChanged.next();
    }

    onClickAircraft(row: FlightWatchHistorical) {
        if (!row.company) {
            const dialogRef = this.newCustomerAircraftDialog.open<
                AircraftAssignModalComponent,
                Partial<NewCustomerAircraftDialogData>
            >(AircraftAssignModalComponent, {
                data: {
                    customers: this.customers,
                    tailNumber: row.flightNumber,
                },
                panelClass: 'aircraft-assign-modal',
                width: '450px',
            });

            dialogRef
                .afterClosed()
                .subscribe((result: Partial<FlightWatchHistorical>) => {
                    if (result) {
                        // const aircrafts = this.data.filter(record => record.flightNumber === row.flightNumber);
                        for (let i = 0; i < this.data.length; i++) {
                            if (
                                this.data[i].flightNumber === row.flightNumber
                            ) {
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

    exportCsv() {
        let fileName ='Airport Departures and Arrivals';
        let sheetName = 'Airport Departures and Arrivals';
        let computePropertyFnc = (item: any[], id: string): any => {
            if(id == "aircraftTypeCode")
                    item[id] = this.getAircraftLabel(item[id]);
            else
                return null;
        }
        this.exportCsvFile(this.columns,fileName,sheetName,computePropertyFnc);
}

    clearAllFilters() {
        this.selectedCustomers = [];
        this.selectedTailNumbers = [];
        this.isCommercialInvisible = true;

        this.dataSource.filter = '';
        for (const filter of this.dataSource.filterCollection) {
            if (filter.isGlobal) {
                continue;
            }

            filter.dateFilter = {
                endDate: null,
                startDate: null,
            };
            filter.stringFilter = '';
            filter.numberRangeFilter = {
                end: null,
                start: null,
            };
            filter.isFiltered = false;
        }

        this.filterChanged();
    }

    getAircraftLabel(type: string) {
        const found = this.aircraftTypes.find((a) => a.id === type);
        if (found) {
            return found.label;
        } else {
            return 'Other';
        }
    }

    openSettings() {
        const dialogRef = this.tableSettingsDialog.open(
            TableSettingsComponent,
            {
                data: this.columns,
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.columns = [...result];

            this.refreshSort();
            this.saveSettings();
        });
    }

    saveSettings() {
        localStorage.setItem(
            this.tableLocalStorageKey,
            JSON.stringify(this.columns)
        );
    }
}
