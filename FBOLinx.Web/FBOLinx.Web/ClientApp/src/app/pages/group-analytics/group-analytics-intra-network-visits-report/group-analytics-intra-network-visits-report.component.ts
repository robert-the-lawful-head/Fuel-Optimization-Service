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
        /*{ id: 'faaRegisteredOwner', name: 'FAA Registered Owner' },*/
        { id: 'aircraftType', name: 'Aircraft' },
        { id: 'aircraftTypeCode', name: 'Aircraft Type' }
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
    selectedFbos: any[] = [];

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
            else if (!item[id] && this.dynamicColumns.some(x => x.id == id))
                return 0;
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

    public removeFboSelectionClicked(fbo: any) {
        this.selectedFbos.splice(this.selectedFbos.indexOf(fbo), 1);
        this.filterChanged();
    }


    //Builds the datasource from the data - creating dynamic columns for each distinct FBO and airport loaded
    private refreshDataSource() {
        this.customers = [];
        this.tailNumbers = [];
        this.fbos = [];
        var dataSource = [];
        this.dynamicColumns = [];

        var filteredData = this.data.filter(x => (this.selectedCustomers.length == 0 || this.selectedCustomers.includes(x.customerInfoByGroupId)) && (this.selectedTailNumbers.length == 0 || this.selectedTailNumbers.includes(x.tailNumber)));

        for (let item of filteredData) {

            this.populateCustomersFromReportItem(item);
            this.populateTailNumbersFromReportItem(item);

            var newRow = {
                company: item.company,
                tailNumber: item.tailNumber,
                aircraftType: item.aircraftType,
                aircraftTypeCode: item.aircraftTypeCode,
                customerInfoByGroupId: item.customerInfoByGroupId,
            };
            
            for (let airport of item.visitsByAirport) {

                this.populateFbosFromAirportVisitsItem(airport);

                if (this.selectedFbos.length > 0 && !this.selectedFbos.some(x => x.acukwikFboHandlerId == airport.acukwikFboHandlerId))
                    continue;

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

    private populateFbosFromAirportVisitsItem(airportVisitsItem: IntraNetworkVisitsReportByAirportItem) {
        var fboOption = this.fbos.find(x => x.acukwikFboHandlerId == airportVisitsItem.acukwikFboHandlerId);
        if (fboOption == null)
            this.fbos.push({ fbo: airportVisitsItem.fboName, acukwikFboHandlerId: airportVisitsItem.acukwikFboHandlerId, icao: airportVisitsItem.icao });
    }

    private populateCustomersFromReportItem(item: IntraNetworkVisitsReportItem) {
        var customerOption = this.customers.find(x => x.customerInfoByGroupId == item.customerInfoByGroupId);
        if (customerOption == null)
            this.customers.push({ company: item.company, customerInfoByGroupId: item.customerInfoByGroupId });
    }

    private populateTailNumbersFromReportItem(item: IntraNetworkVisitsReportItem) {
        this.tailNumbers.push({ tailNumber: item.tailNumber });
    }
}
