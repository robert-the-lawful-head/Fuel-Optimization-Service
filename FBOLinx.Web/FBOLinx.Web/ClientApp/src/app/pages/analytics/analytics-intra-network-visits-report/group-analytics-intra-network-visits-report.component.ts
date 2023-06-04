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

import { isCommercialAircraft } from '../../../../utils/aircraft';
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
    selector: 'app-analytics-intra-network-visits-report',
    templateUrl: './analytics-intra-network-visits-report.component.html',
})
export class AnalyticsIntraNetworkVisitsReportComponent extends GridBase implements OnInit {
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
        /*{ id: 'faaRegisteredOwner', name: 'FAA Registered Owner' },*/
        { id: 'aircraftType', name: 'Aircraft Type' },
        { id: 'aircraftSize', name: 'Aircraft Size' }
    ];
    dynamicColumns: ColumnType[] = [];

    csvFileOptions: csvFileOptions = { fileName: 'Intra-Network ADS-B Data', sheetName: 'Intra-Network ADS-B Data' };

    filterStartDate: Date;
    filterEndDate: Date;    
    data: IntraNetworkVisitsReportItem[];
    customers: any[] = [];
    tailNumbers: any[] = [];
    fbos: any[] = [];
    isCommercialInvisible = true;
    selectedCustomers: number[] = [];
    selectedTailNumbers: string[] = [];
    selectedFbos: number[] = [];

    filtersChanged: Subject<any> = new Subject<any>();
    aircraftTypes = AIRCRAFT_IMAGES;

    constructor(private airportWatchSerice: AirportWatchService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService
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
            (this.getAllColumns
                .filter((column) => !column.hidden)
                .map((column) => column.id) || [])
            );
    }

    get getAllColumns() {
        return this.availableColumns
            .concat(this.dynamicColumns);
    }

    ngOnInit() {
        this.refreshData();
    }

    public clearAllFilters() {
        this.selectedCustomers = [];
        this.selectedTailNumbers = [];
        this.selectedFbos = [];
        //this.isCommercialInvisible = true;

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

    public refreshData() {
        this.ngxLoader.start();
        this.airportWatchSerice
            .getIntraNetworkVisitsReport(
                this.sharedService.currentUser.groupId,
                this.filterStartDate,
                this.filterEndDate
            )
            .subscribe((data: IntraNetworkVisitsReportItem[]) => {
                this.data = data;
                this.refreshDataSource();
                this.ngxLoader.stop();
            });
    }

    public filterChanged() {
        this.filtersChanged.next();
    }

    public exportCsv() {
        let computePropertyFnc = (item: any[], id: string): any => {
            if (id == "aircraftTypeCode")
                item[id] = this.getAircraftLabel(item[id]);
            else
                return null;
        }
        this.exportCsvFile(this.getAllColumns, this.csvFileOptions.fileName, this.csvFileOptions.sheetName, computePropertyFnc);
    }

    public getAircraftLabel(type: string) {
        const found = this.aircraftTypes.find((a) => a.id === type);
        if (found) {
            return found.label;
        } else {
            return 'Other';
        }
    }

    private refreshDataSource() {
        this.customers = [];
        this.tailNumbers = [];
        var dataSource = [];
        this.dynamicColumns = [];
        for (let item of this.data) {

            this.populateCustomersFromDataItem(item);
            this.populateTailNumbersFromDataItem(item);

            var newRow = {
                company: item.company,
                tailNumber: item.tailNumber,
                aircraftType: item.aircraftType,
                aircraftSize: item.aircraftSize,
                customerInfoByGroupId: item.customerInfoByGroupId,
            };
            
            for (let airport of item.visitsByAirport) {
                var columnForAirport: ColumnType = { id: airport.icao, name: 'Visits to ' + airport.icao };
                var columnForFbo: ColumnType = { id: airport.icao + airport.acukwikFboHandlerId, name: 'Visits to ' + airport.fboName + ' ' + airport.icao };
                if (!this.dynamicColumns.some(x => x.id == columnForAirport.id)) {
                    this.dynamicColumns.push(columnForAirport);
                    this.dynamicColumns.push(columnForFbo);
                }
                newRow[columnForAirport.id] = airport.visitsToAirport;
                newRow[columnForFbo.id] = airport.visitsToFbo;
            }
            dataSource.push(newRow);            
        }
        this.setVirtualScrollVariables(this.paginator, this.sort, dataSource)

        if (!this.dataSource) {
            this.dataSource.filteredData = [];
        }

        this.customers.sort((a, b) => a.company?.localeCompare(b.company));
        this.tailNumbers.sort((a, b) => a.tailNumber?.localeCompare(b.tailNumber));
    }

    private populateCustomersFromDataItem(item: IntraNetworkVisitsReportItem) {
        var customerOption = this.customers.find(x => x.customerInfoByGroupId == item.customerInfoByGroupId);
        if (customerOption == null)
            this.customers.push({ company: item.company, customerInfoByGroupId: item.customerInfoByGroupId });
    }

    private populateTailNumbersFromDataItem(item: IntraNetworkVisitsReportItem) {
        this.tailNumbers.push({ tailNumber: item.tailNumber });
    }
}
