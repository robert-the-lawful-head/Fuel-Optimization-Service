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
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Observable, Subject, Subscription } from 'rxjs';
import { ColumnType } from '../../../shared/components/table-settings/table-settings.component';
import { JetNetInformationComponent } from '../../../shared/components/jetnet-information/jetnet-information.component';


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
import { csvFileOptions, GridBase } from '../../../services/tables/GridBase';
import { localStorageAccessConstant } from '../../../constants/LocalStorageAccessConstant';
import { SelectedDateFilter } from '../../../shared/components/preset-date-filter/preset-date-filter.component';
import { FbosGridViewModel } from '../../../models/FbosGridViewModel';

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

    icao: string;
    selectedDateFilter: SelectedDateFilter;

    hideCommercialAicrafts = false;

    data: FlightWatchHistorical[];

    fbo: any;
    allFbos: FbosGridViewModel[] = [];
    selectFbos: FbosGridViewModel[] = [];

    filtersChanged: Subject<any> = new Subject<any>();

    aircraftTypes = AIRCRAFT_IMAGES;

    tableLocalStorageKey: string;

    columns: ColumnType[] = [];

    fboName: string = '';

    tailNumbers: any[] = [];

    hiddenColumns: string[] = ['hexCode', 'flightNumber', 'pastVisits', 'visitsToMyFbo', 'isConfirmedVisit', 'percentOfVisits'];

    initialColumns: ColumnType[] = [
        {
            id: 'company',
            name: 'Company',
        },
        {
            id: 'customerActionStatus',
            name: 'Customer Action Status',
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
        {
            id: 'originated',
            name: 'Origin ICAO',
        }
    ];

    csvFileOptions: csvFileOptions = {
        fileName: 'Airport Departures and Arrivals',
        sheetName: 'Airport Departures and Arrivals',
    };

    filtersChangeSubscription: Subscription;
    sortChangeSubscription: Subscription;

    constructor(
        private newCustomerAircraftDialog: MatDialog,
        private tableSettingsDialog: MatDialog,
        private airportWatchService: AirportWatchService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private fbosService: FbosService,
        private jetNetInformationDialog: MatDialog,
    ) {
        super();
        this.icao = this.sharedService.getCurrentUserPropertyValue(
            localStorageAccessConstant.icao
        );
        this.filtersChangeSubscription = this.filtersChanged
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

    get isJetNetIntegrationEnabled() {
        return this.sharedService.currentUser.isJetNetIntegrationEnabled;
    }

    ngOnInit() {
        this.fbosService
        .getFbosByIcao(this.icao)
        .subscribe((data: FbosGridViewModel[]) => {
            this.allFbos = data.filter((fbo) => fbo.acukwikFboHandlerId != null);
            let currentFbo = this.allFbos.find((fbo) =>  fbo.oid == this.sharedService.currentUser.fboId);
            this.selectFbos.push(currentFbo);
            let otherOpt = { ...currentFbo };
            otherOpt.oid = 0;
            otherOpt.acukwikFboHandlerId = 0;
            otherOpt.fbo = 'Other';
            this.selectFbos.push(otherOpt);
            this.fbo = currentFbo;
            this.fboName = currentFbo.fbo;
        });
        this.getCustomersList();
        this.sortChangeSubscription = this.sort.sortChange.subscribe(() => {
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

    fetchSwimData(startDate: Date, endDate: Date, icao: string) {
        return this.airportWatchService.getArrivalsDeparturesSwim(
            this.sharedService.currentUser.fboId,
            {
                endDateTime: endDate,
                startDateTime: startDate,
            },
            this.icao
        );
    }

    initColumns() {
        this.tableLocalStorageKey = `analytics-airport-arrivals-depatures-${this.sharedService.currentUser.fboId}`;
        this.columns = this.getClientSavedColumns(
            this.tableLocalStorageKey,
            this.initialColumns
        );
    }
    ngOnDestroy() {
        this.sortChangeSubscription?.unsubscribe();
        this.filtersChangeSubscription?.unsubscribe();
    }
    refreshData(isLoaderActive: boolean = false) {
        let endDate = this.getEndOfDayTime(this.filterEndDate, true);
        let startDate = this.getStartOfDayTime(this.filterStartDate, true);

        if (!isLoaderActive)
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
            this.fetchSwimData(startDate, endDate, this.icao).subscribe(
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
        this.ngxLoader.startLoader(this.chartName);
        const data = this.data.filter(
            (x) => { return (!this.hideCommercialAicrafts)? true:  !isCommercialAircraft(x.flightNumber)}
        ).map((x) => ({
            ...x,
            aircraftTypeCode: this.getAircraftLabel(x.aircraftTypeCode),
            isParkedWithinGeofence: x.parkingAcukwikFBOHandlerId == this.fbo.acukwikFboHandlerId
        }));

        this.setVirtualScrollVariables(this.paginator, this.sort, data);

        if (!this.dataSource) {
            this.dataSource.filteredData = [];
        }

        this.tailNumbers = [
            ...new Set(
                this.data
                .filter(
                    (x) => { return (!this.hideCommercialAicrafts)? true:  (!isCommercialAircraft(x.flightNumber) || !isCommercialAircraft(x.tailNumber))}
                ).map((x) => x.tailNumber)
            ),
        ].map((tailNumber) =>
            this.data.find((x) => x.tailNumber === tailNumber)
        );
        this.ngxLoader.stopAllLoader(this.chartName);
    }

    filterChanged(value: any = null) {
        if(typeof value == "boolean")
            this.hideCommercialAicrafts = value;

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
        this.hideCommercialAicrafts = false;

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
    confirmedVisitToggled(row: FlightWatchHistorical): void{
        if (row.airportWatchHistoricalParking == null) {
            row.airportWatchHistoricalParking = {
                airportWatchHistoricalDataId: row.airportWatchHistoricalDataId,
                acukwikFbohandlerId: 0,
                oid: 0,
            };
        }

        row.airportWatchHistoricalParking.acukwikFbohandlerId = (row.isParkedWithinGeofence)?this.fbo.acukwikFboHandlerId: 0;

        row.airportWatchHistoricalParking.isConfirmed = true;

        this.ngxLoader.startLoader(this.chartName);
        if (row.airportWatchHistoricalParking.oid > 0) {
            this.airportWatchService
                .updateHistoricalParking(row)
                .subscribe((response: any) => {
                    this.refreshData(true);
                });
        } else {
            this.airportWatchService
                .createHistoricalParking(row)
                .subscribe((response: any) => {
                    this.refreshData(true);
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
    tailNumberSearch(tailNumber: any) {
        if (tailNumber.trim() != "") {
            const dialogRef = this.jetNetInformationDialog.open(JetNetInformationComponent, {
                width: '1100px',
                data: tailNumber.trim()
            });
            dialogRef
                .afterClosed()
                .subscribe((result: any) => {

                });
        }
    }

    private setColumns() {
        this.columns = this.filteredColumns;
    }
    get filteredColumns() {
        var filteredColumns = this.initialColumns;
        //if (!filteredColumns.find((column) => column.id === 'originated')) {
        //    filteredColumns.push({
        //        id: 'originated',
        //        name: 'Origin ICAO',
        //    });
        //};

        if (this.icao != this.sharedService.currentUser.icao)
            return filteredColumns.filter((column) => {
                return !this.hiddenColumns.includes(column.id);
            });
        else
            return filteredColumns;
    }
}
