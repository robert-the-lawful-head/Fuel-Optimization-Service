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
import { MatSort } from '@angular/material/sort';
import * as moment from 'moment';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Observable, Subject } from 'rxjs';
import { ColumnType } from 'src/app/shared/components/table-settings/table-settings.component';

import { isCommercialAircraft } from '../../../../utils/aircraft';
import { AIRCRAFT_IMAGES } from '../../flight-watch/flight-watch-map/aircraft-images';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { FbosService } from '../../../services/fbos.service';

//Models
import { CustomersListType } from '../../../models/customer';
import { FlightWatchHistorical } from '../../../models/flight-watch-historical';
import {
    AircraftAssignModalComponent,
    NewCustomerAircraftDialogData,
} from '../../../shared/components/aircraft-assign-modal/aircraft-assign-modal.component';
import { csvFileOptions, GridBase } from 'src/app/services/tables/GridBase';
import { SelectedDateFilter } from 'src/app/shared/components/preset-date-filter/preset-date-filter.component';
import { localStorageAccessConstant } from 'src/app/models/LocalStorageAccessConstant';
import { CustomIcaoList } from '../analytics-report-popup/report-filters/report-filters.component';
import { FbosGridViewModel } from 'src/app/models/FbosGridViewModel';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'app-analytics-airport-arrivals-depatures',
    styleUrls: ['./analytics-airport-arrivals-depatures.component.scss'],
    templateUrl: './analytics-airport-arrivals-depatures.component.html',
})
export class AnalyticsAirportArrivalsDepaturesComponent
    extends GridBase
    implements OnInit
{
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;

    @Input() customers: CustomersListType[] = [];
    @Output() refreshCustomers = new EventEmitter();

    chartName = 'airport-arrivals-depatures-table';
    displayedColumns: string[] = [
        'company',
        'tailNumber',
        'flightNumber',
        'hexCode',
        'aircraftType',
        'dateTime',
        'status',
        'pastVisits',
        'visitsToMyFbo',
        'percentOfVisits',
    ];

    icao: string;
    selectedDateFilter: SelectedDateFilter;

    isCommercialInvisible = true;

    data: FlightWatchHistorical[];

    fbo: any;
    allFbos: FbosGridViewModel[] = [];

    filtersChanged: Subject<any> = new Subject<any>();

    aircraftTypes = AIRCRAFT_IMAGES;

    tableLocalStorageKey: string;

    columns: ColumnType[] = [];

    fboName: string = '';

    tailNumbers: any[] = [];

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
            hidden: true,
        },
        {
            id: 'aircraftType',
            name: `Aircraft`,
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
            id: 'isConfirmedVisit',
            name: 'Visit to My FBO?',
        },
        {
            id: 'percentOfVisits',
            name: 'Percent of Visits',
        },
    ];

    csvFileOptions: csvFileOptions = {
        fileName: 'Airport Departures and Arrivals',
        sheetName: 'Airport Departures and Arrivals',
    };

    constructor(
        private newCustomerAircraftDialog: MatDialog,
        private tableSettingsDialog: MatDialog,
        private airportWatchService: AirportWatchService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private fbosService: FbosService
    ) {
        super();
        this.icao = this.sharedService.getCurrentUserPropertyValue(
            localStorageAccessConstant.icao
        );
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
        this.fbosService
        .getFbosByIcao(this.icao)
        .subscribe((data: FbosGridViewModel[]) => {
            this.allFbos = data.filter((fbo) => fbo.acukwikFBOHandlerId == null);
            let otherOpt = { ...data[0] };
            otherOpt.oid = 0;
            otherOpt.acukwikFBOHandlerId = 0;
            otherOpt.fbo = 'Other';
            this.allFbos.push(otherOpt);
        });
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
        if (this.fboName && this.fboName != '') return;
        this.fbosService
            .get({
                oid: this.sharedService.currentUser.fboId,
            })
            .subscribe((data: any) => {
                this.fbo = data;
                this.fboName = data.fbo;
            });
    }

    getCustomersList() {
        if (this.customers && this.customers.length > 0) return;
        this.customerInfoByGroupService
            .getCustomersListByGroupAndFbo(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((customers: any[]) => {
                this.customers = customers;
            });
    }

    fetchData(startDate: Date, endDate: Date): Observable<FlightWatchHistorical[]>{
        return this.airportWatchService.getArrivalsDepartures(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
            {
                endDateTime: endDate,
                startDateTime: startDate,
            },
            this.icao
        );
    }

    fetchSwimData(startDate: Date, endDate: Date) {
        return this.airportWatchService.getArrivalsDeparturesSwim(
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
        this.columns = this.getClientSavedColumns(
            this.tableLocalStorageKey,
            this.initialColumns
        );
    }

    refreshData() {
        let endDate = this.getEndOfDayTime(this.filterEndDate, true);
        let startDate = this.getStartOfDayTime(this.filterStartDate, true);

        this.ngxLoader.startLoader(this.chartName);
        if (this.sharedService.currentUser.icao == this.icao) {
            this.fetchData(startDate, endDate).subscribe(
                (data: FlightWatchHistorical[]) => {
                    this.data = data;
                    this.refreshDataSource();
                },
                () => { },
                () => {
                    this.ngxLoader.stopLoader(this.chartName);
                }
            );
        }
        else {
            this.fetchData(startDate, endDate).subscribe(
                (data: FlightWatchHistorical[]) => {
                    this.data = data;
                    this.refreshDataSource();
                },
                () => { },
                () => {
                    this.ngxLoader.stopLoader(this.chartName);
                }
            );
        }
    }

    refreshDataSource() {
        const data = this.data.map((x) => ({
            ...x,
            aircraftTypeCode: this.getAircraftLabel(x.aircraftTypeCode),
            parkingAcukwikFBOHandlerId: (x.parkingAcukwikFBOHandlerId)?
             x.parkingAcukwikFBOHandlerId:
             this.fbo.acukwikFBOHandlerId
        }));

        this.setVirtualScrollVariables(this.paginator, this.sort, data);

        if (!this.dataSource) {
            this.dataSource.filteredData = [];
        }

        this.tailNumbers = [
            ...new Set(
                this.data
                    .filter(
                        (x) =>
                            !this.isCommercialInvisible ||
                            !isCommercialAircraft(x.aircraftTypeCode)
                    )
                    .map((x) => x.tailNumber)
            ),
        ].map((tailNumber) =>
            this.data.find((x) => x.tailNumber === tailNumber)
        );
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
                        this.refreshData();
                        this.refreshCustomers.emit();
                    }
                });
        }
    }

    exportCsv() {
        let computePropertyFnc = (item: any[], id: string): any => {
            if (id == 'aircraftTypeCode')
                item[id] = this.getAircraftLabel(item[id]);
            else return null;
        };
        this.exportCsvFile(
            this.columns,
            this.csvFileOptions.fileName,
            this.csvFileOptions.sheetName,
            computePropertyFnc
        );
    }

    clearAllFilters() {
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
        var _this = this;
        this.openSettingsDialog(
            this.tableSettingsDialog,
            this.columns,
            function (result) {
                _this.columns = result;
                _this.refreshSort(_this.sort, _this.columns);
                _this.saveSettings();
            }
        );
    }

    saveSettings() {
        localStorage.setItem(
            this.tableLocalStorageKey,
            JSON.stringify(this.columns)
        );
    }
    updateVisitedFbo($event: any , row: FlightWatchHistorical): void{
        if (row.airportWatchHistoricalParking == null) {
            row.airportWatchHistoricalParking = {
                airportWatchHistoricalDataId: row.airportWatchHistoricalDataId,
                acukwikFbohandlerId: $event.value,
                oid: 0,
            };
        }else{
            row.airportWatchHistoricalParking.acukwikFbohandlerId= $event.value;
        }

        row.airportWatchHistoricalParking.isConfirmed = row.isConfirmedVisit;

        if (row.airportWatchHistoricalParking.oid > 0) {
            this.airportWatchService
                .updateHistoricalParking(row)
                .subscribe((response: any) => {
                    this.refreshData();
                });
        } else {
            this.airportWatchService
                .createHistoricalParking(row)
                .subscribe((response: any) => {
                    this.refreshData();
                });
        }
    }
    changeIcaoFilter($event: string) {
        this.icao = $event;
        this.setColumns();
        this.refreshData();
    }
    applyPresetDateFilter(filter: SelectedDateFilter) {
        this.filterEndDate = filter.limitDate;
        this.filterStartDate = filter.offsetDate;
        this.refreshData();
    }
    private setColumns() {
        this.columns =
            this.icao == this.sharedService.currentUser.icao
                ? this.initialColumns
                : this.filteredColumns;
    }
    get filteredColumns() {
        return this.initialColumns.filter((column) => {
            return !this.hiddenColumns.includes(column.id);
        });
    }
}
