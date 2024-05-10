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
import { Subject } from 'rxjs';
import {
    ColumnType,
} from 'src/app/shared/components/table-settings/table-settings.component';

import { AIRCRAFT_IMAGES } from '../../flight-watch/flight-watch-map/aircraft-images';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { FbosService } from '../../../services/fbos.service';

//Models
import { csvFileOptions, GridBase } from 'src/app/services/tables/GridBase';
import { IntraNetworkVisitsReportItem } from 'src/app/models/intra-network-visits-report-item';
import { SelectedDateFilter } from 'src/app/shared/components/preset-date-filter/preset-date-filter.component';
import { localStorageAccessConstant } from 'src/app/models/LocalStorageAccessConstant';
import { ReportFilterItems } from '../../analytics/analytics-report-popup/report-filters/report-filters.component';
import { isCommercialAircraftInFlightNumbers } from 'src/utils/aircraft';

@Component({
    selector: 'app-group-analytics-intra-network-visits-report',
    templateUrl: './group-analytics-intra-network-visits-report.component.html',
    styleUrls: ['./group-analytics-intra-network-visits-report.component.scss'],
})
export class GroupAnalyticsIntraNetworkVisitsReportComponent extends GridBase implements OnInit {
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

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

    csvFileOptions: csvFileOptions = { fileName: 'FBO Network Arrival & Departures', sheetName: 'FBO Network Arrival&Departures' };


    data: IntraNetworkVisitsReportItem[];
    fbos: any[] = [];
    selectedFbos: any[] = [];

    filtersChanged: Subject<any> = new Subject<any>();
    aircraftTypes = AIRCRAFT_IMAGES;
    tableLocalStorageKey: string;

    icao:string;
    reportHiddenItems: ReportFilterItems[] = [ReportFilterItems.icaoDropDown, ReportFilterItems.searchInput];

    isCommercialInvisible = false;

    constructor(private airportWatchSerice: AirportWatchService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private tableSettingsDialog: MatDialog,
        private fboService: FbosService
    ) {
        super();
        this.filtersChanged
            .debounceTime(500)
            .subscribe(() => this.refreshDataSource());

        this.icao = this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.icao);

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
        this.selectedFbos = [];
        this.isCommercialInvisible = false;
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
                this.sharedService.currentUser.fboId,
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

    filterChanged(value: any = null) {
        console.log("🚀 ~ filterChanged ~ value:", value)
        if(typeof value == "boolean")
            this.isCommercialInvisible = value;

        console.log("🚀 ~ filterChanged ~ filterChanged:")
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
            this.dynamicColumns = this.dynamicColumns.filter((v,i,a)=>a.findIndex(v2=>(v2.id===v.id))===i)

            this.refreshData(true);
        });
    }

    //Builds the datasource from the data - creating dynamic columns for each distinct FBO and airport loaded
    private refreshDataSource() {
        var dataSource = [];

        var filteredData = this.data.filter(
            (x) => { return (!this.isCommercialInvisible)? true:  !isCommercialAircraftInFlightNumbers(x.flightNumbers)}
        );

        for (let item of filteredData) {

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

        this.fbos.sort((a, b) => (a.fbo)?.localeCompare(b.fbo));
        this.columns = this.getAllColumns;
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
    private setColumns(){
        this.columns = (this.icao == this.sharedService.currentUser.icao) ? this.getAllColumns : this.filteredColumns;
    }
    get filteredColumns() {
        return this.getAllColumns.filter((column) => {
            return !this.hiddenColumns.includes(column.id);
        });
    }
}
