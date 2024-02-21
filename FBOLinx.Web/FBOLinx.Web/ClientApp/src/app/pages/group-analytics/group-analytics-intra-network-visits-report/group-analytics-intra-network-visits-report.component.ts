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

import { isCommercialAircraft, isCommercialAircraftInFlightNumbers } from '../../../../utils/aircraft';
import { AIRCRAFT_IMAGES } from '../../flight-watch/flight-watch-map/aircraft-images';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { FbosService } from '../../../services/fbos.service';

//Models
import { CustomersListType } from '../../../models/customer';
import { csvFileOptions, GridBase } from 'src/app/services/tables/GridBase';
import { IntraNetworkVisitsReportItem } from 'src/app/models/intra-network-visits-report-item';
import { IntraNetworkVisitsReportByAirportItem } from 'src/app/models/intra-network-visits-report-by-airport-item';

@Component({
    selector: 'app-group-analytics-intra-network-visits-report',
    templateUrl: './group-analytics-intra-network-visits-report.component.html',
})
export class GroupAnalyticsIntraNetworkVisitsReportComponent extends GridBase implements OnInit {
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    //@ViewChild('tableSettings') tableSettings: TableSettingsComponent;

    @Input() groupId: number;
    @Input() fboId: number;
    @Input() startDate: Date;
    @Input() endDate: Date;

    @Output() onFilterChange = new EventEmitter();

    chartName = 'intra-network-visits-report-table';
    availableColumns: ColumnType[] = [
        { id: 'tailNumber', name: 'Tail Number' },
        { id: 'company', name: 'Company' },
        { id: 'aircraftType', name: 'Aircraft' }
    ];
    dynamicColumns: ColumnType[] = [];
    columns: ColumnType[] = [];

    csvFileOptions: csvFileOptions = { fileName: 'FBO Network Arrival/Departures', sheetName: 'FBO Network Arrival/Departures' };

    filterStartDate: Date;
    filterEndDate: Date;
    data: IntraNetworkVisitsReportItem[];
    customers: any[] = [];
    tailNumbers: any[] = [];
    fbos: any[] = [];
    isCommercialInvisible = true;
    selectedCustomers: string[] = [];
    selectedTailNumbers: string[] = [];
    selectedFbos: any[] = [];

    filtersChanged: Subject<any> = new Subject<any>();
    aircraftTypes = AIRCRAFT_IMAGES;
    tableLocalStorageKey: string;

    constructor(private airportWatchSerice: AirportWatchService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private tableSettingsDialog: MatDialog,
        private fboService: FbosService
    ) {
        super();
        this.filterStartDate = new Date(
            moment().add(-1, 'M').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.filtersChanged
            .debounceTime(500)
            .subscribe(() => this.refreshDataSource());
    }

    get visibleColumns() {
        return (
            (this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.id) || [])
            );
    }

    get getAllColumns() {
        return this.availableColumns
            .concat(this.dynamicColumns);
    }

    ngOnInit() {
        this.loadFbos();
    }

    public initColumns() {
        this.tableLocalStorageKey = `group-analytics-intra-network-visits-report-${this.sharedService.currentUser.fboId}`;
        this.columns = this.getClientSavedColumns(this.tableLocalStorageKey, this.getAllColumns);
    }

    public clearAllFilters() {
        this.selectedCustomers = [];
        this.selectedTailNumbers = [];
        this.selectedFbos = [];
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

    public refreshData(initializeSavedColumns: boolean = false) {
        this.ngxLoader.startLoader(this.chartName);
        this.airportWatchSerice
            .getIntraNetworkVisitsReport(
                this.sharedService.currentUser.groupId,
                this.filterStartDate,
                this.filterEndDate
            )
            .subscribe((data: IntraNetworkVisitsReportItem[]) => {
                this.data = data;
                this.refreshDataSource();
                if (initializeSavedColumns)
                    this.initColumns();
                this.ngxLoader.stopLoader(this.chartName);
            });
    }

    public filterChanged() {
        this.filtersChanged.next();
    }

    public exportCsv() {
        let computePropertyFnc = (item: any[], id: string): any => {
            if (id == "aircraftTypeCode")
                item[id] = this.getAircraftLabel(item[id]);
            else if (!item[id] && this.dynamicColumns.some(x => x.id == id))
                item[id] = 0;
        }
        this.exportCsvFile(this.columns, this.csvFileOptions.fileName, this.csvFileOptions.sheetName, computePropertyFnc);
    }

    public getAircraftLabel(type: string) {
        const found = this.aircraftTypes.find((a) => a.id === type);
        if (found) {
            return found.label;
        } else {
            return 'Other';
        }
    }

    public removeFboSelectionClicked(fbo: any) {
        this.selectedFbos.splice(this.selectedFbos.indexOf(fbo), 1);
        this.filterChanged();
    }

    public openSettings() {
        var _this = this;
        this.openSettingsDialog(this.tableSettingsDialog, this.columns, function (result) {
            _this.columns = result;
            var allColumns = _this.getAllColumns;
            for (var i = 0; i < _this.columns.length; i++) {
                try {
                    allColumns.find(x => x.id == _this.columns[i].id).hidden = _this.columns[i].hidden;
                }
                catch (e) {

                }
            }
            _this.refreshSort(_this.sort, _this.columns);
            _this.saveSettings(_this.tableLocalStorageKey, _this.columns);
        });
    }

    private loadFbos() {
        this.fboService.getForGroup(this.sharedService.currentUser.groupId).subscribe((data: any[]) => {
            this.fbos = data.map((x) => ({ fbo: x.fbo, acukwikFboHandlerId: x.acukwikFboHandlerId, icao: x.icao }));
            for (let fbo of this.fbos) {
                var columnForAirport: ColumnType = { id: fbo.icao, name: 'Visits to ' + fbo.icao };
                var columnForFbo: ColumnType = { id: fbo.icao + fbo.acukwikFboHandlerId, name: 'Visits to ' + fbo.fbo + ' ' + fbo.icao };
                this.dynamicColumns.push(columnForAirport);
                this.dynamicColumns.push(columnForFbo);
            }
            this.refreshData(true);
        });
    }

    //Builds the datasource from the data - creating dynamic columns for each distinct FBO and airport loaded
    private refreshDataSource() {
        var populateCustomersDataSource = this.customers.length == 0;
        var populateTailNumbersDataSource = this.tailNumbers.length == 0;
        //this.fbos = [];
        var dataSource = [];



        var filteredData = this.data.filter(x => (this.selectedCustomers.length == 0 || this.selectedCustomers.some(s => s.toLowerCase() == x.company.toLowerCase())) &&
            (this.selectedTailNumbers.length == 0 || this.selectedTailNumbers.some(s => s.toLowerCase() == x.tailNumber.toLowerCase())) &&
            (!this.isCommercialInvisible || !isCommercialAircraftInFlightNumbers(x.flightNumbers)));

        for (let item of filteredData) {

            if (populateCustomersDataSource)
                this.populateCustomersFromReportItem(item);
            if (populateTailNumbersDataSource)
                this.populateTailNumbersFromReportItem(item);


            var newRow = {
                company: item.company,
                tailNumber: item.tailNumber,
                aircraftType: item.aircraftType,
                aircraftTypeCode: item.aircraftTypeCode,
                customerInfoByGroupId: item.customerInfoByGroupId,
            };

            for (let airport of item.visitsByAirport) {

                if (this.selectedFbos.length > 0 && !this.selectedFbos.some(x => x.acukwikFboHandlerId == airport.acukwikFboHandlerId))
                    continue;

                var columnForAirport: ColumnType = { id: airport.icao, name: 'Visits to ' + airport.icao };
                var columnForFbo: ColumnType = { id: airport.icao + airport.acukwikFboHandlerId, name: 'Visits to ' + airport.fboName + ' ' + airport.icao };
                newRow[columnForAirport.id] = !airport.visitsToAirport ? 0 : airport.visitsToAirport;
                newRow[columnForFbo.id] = !airport.visitsToFbo ? 0 : airport.visitsToFbo;
            }
            dataSource.push(newRow);
        }
        this.setVirtualScrollVariables(this.paginator, this.sort, dataSource)

        if (!this.dataSource) {
            this.dataSource.filteredData = [];
        }

        this.customers.sort((a, b) => a.company?.localeCompare(b.company));
        this.tailNumbers.sort((a, b) => a.tailNumber?.localeCompare(b.tailNumber));
        this.fbos.sort((a, b) => (a.fbo)?.localeCompare(b.fbo));
        this.columns = this.getAllColumns;
    }

    private populateCustomersFromReportItem(item: IntraNetworkVisitsReportItem) {
        var customerOption = this.customers.find(x => x.company == item.company);
        if (customerOption == null && item.company && item.company != '')
            this.customers.push({ company: item.company });
    }

    private populateTailNumbersFromReportItem(item: IntraNetworkVisitsReportItem) {
        this.tailNumbers.push({ tailNumber: item.tailNumber });
    }
}
