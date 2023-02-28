import { Router } from '@angular/router';
import {
    AfterViewInit,
    Component,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import {
    CsvExportModalComponent,
    ICsvExportModalData,
} from 'src/app/shared/components/csv-export-modal/csv-export-modal.component';
import {
    ColumnType,
    TableSettingsComponent,
} from 'src/app/shared/components/table-settings/table-settings.component';
import * as XLSX from 'xlsx';

import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvent from '../../../models/sharedEvents';
import { FbosService } from '../../../services/fbos.service';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { csvFileOptions, VirtualScrollBase } from 'src/app/services/tables/VirtualScrollBase';
import { basename } from 'path';

@Component({
    selector: 'app-analytics-companies-quotes-deal',
    styleUrls: ['./analytics-companies-quotes-deal-table.component.scss'],
    templateUrl: './analytics-companies-quotes-deal-table.component.html',
})
export class AnalyticsCompaniesQuotesDealTableComponent extends VirtualScrollBase
    implements OnInit, AfterViewInit, OnDestroy
{
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    filterStartDate: Date;
    filterEndDate: Date;
    icao: string;
    fbo: string;
    icaoChangedSubscription: any;
    chartName = 'companies-quotes-deal-table';
    displayedColumns: string[] = [
        'company',
        'directOrders',
        'companyQuotesTotal',
        'conversionRate',
        'conversionRateTotal',
        'totalOrders',
        'airportOrders',
        'customerBusiness',
        'lastPullDate',
        'airportVisits',
        'visitsToFbo',
        'percentVisits',
    ];
    tableLocalStorageKey: string;
    columns: ColumnType[] = [];

    dataLength = 0;

    csvFileOptions: csvFileOptions = { fileName: 'Customer Statistics.csv', sheetName: 'Customer Statistics' };

    constructor(
        private fuelreqsService: FuelreqsService,
        private fbosService: FbosService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private exportDialog: MatDialog,
        private router: Router,
        private tableSettingsDialog: MatDialog
    ) {
        super();
        this.icao = this.sharedService.currentUser.icao;
        this.filterStartDate = new Date(
            moment().add(-12, 'M').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(
            moment().add(7, 'd').format('MM/DD/YYYY')
        );
        this.initColumns();
    }

    ngOnInit() {
        this.fbosService
            .get({ oid: this.sharedService.currentUser.fboId })
            .subscribe((data: any) => {
                this.fbo = data.fbo;
                this.initColumns();
            });

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

    ngAfterViewInit() {
        this.icaoChangedSubscription =
            this.sharedService.changeEmitted$.subscribe((message) => {
                if (message === SharedEvent.icaoChangedEvent) {
                    this.icao = this.sharedService.currentUser.icao;
                }
            });
    }

    ngOnDestroy() {
        if (this.icaoChangedSubscription) {
            this.icaoChangedSubscription.unsubscribe();
        }
    }

    get visibleColumns() {
        return (
            this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.id) || []
        );
    }

    initColumns() {
        var initialColumns = this.columns = [
            {
                id: 'company',
                name: 'Company',
            },
            {
                id: 'directOrders',
                name: 'Direct Orders',
            },
            {
                id: 'companyQuotesTotal',
                name: 'Number of Quotes',
            },
            {
                id: 'conversionRate',
                name: 'Conversion Rate',
            },
            {
                id: 'conversionRateTotal',
                name: 'Conversion Rate (Total)',
            },
            {
                id: 'totalOrders',
                name: `Total Orders at ${this.fbo}`,
            },
            {
                id: 'airportOrders',
                name: `Total Orders at ${this.sharedService.currentUser.icao}`,
            },
            {
                id: 'customerBusiness',
                name: '% of Customer\'s Business',
            },
            {
                id: 'lastPullDate',
                name: `${this.sharedService.currentUser.icao} Last Quoted`,
                sort: 'desc',
            },
            {
                id: 'airportVisits',
                name: `Arrivals`,
            },
            {
                id: 'visitsToFbo',
                name: `Visits to ${this.fbo}`,
            },
            {
                id: 'percentVisits',
                name: `% of visits to ${this.fbo}`,
            },
        ];

        this.tableLocalStorageKey = `analytics-companies-quotes-deal-${this.sharedService.currentUser.fboId}`;
        if (localStorage.getItem(this.tableLocalStorageKey)) {
            this.columns = JSON.parse(
                localStorage.getItem(this.tableLocalStorageKey));

            if (this.columns.length === 9) {
                this.columns = initialColumns;
            }
            else {
                this.columns = JSON.parse(
                    localStorage.getItem(this.tableLocalStorageKey)
                );
            }
        } else {
            this.columns = initialColumns;
        }
    }

    fetchData(startDate: Date, endDate: Date) {
        return this.fuelreqsService.getCompaniesQuotingDealStatistics(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
            startDate,
            endDate
        );
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

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fetchData(this.filterStartDate, this.filterEndDate).subscribe(
            (data: any) => {
                this.dataSource.data = data;
                 this.dataSource.sortingDataAccessor = (item, property) => {
                    switch (property) {
                        case 'lastPullDate':
                            if (item[property] === 'N/A') {
                                if (this.sort.direction === 'asc') {
                                    return new Date(8640000000000000);
                                } else {
                                    return new Date(-8640000000000000);
                                }
                            }
                            return new Date(item[property]);
                        default:
                            return item[property];
                    }
                };

                this.dataSource.sort = this.sort;
                this.paginator.pageIndex = 0;
                this.dataSource.paginator = this.paginator;

                this.refreshSort();
            },
            () => {},
            () => {
                this.ngxLoader.stopLoader(this.chartName);
            }
        );
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    CustomerAnalitycs(element: any )
    {
     this.router.navigate(['/default-layout/customers' , element.oid ], {state : {tab : 3}  });
    }

    exportCsv() {
        let computePropertyFnc = (item: any[], id: string): any => {
            if(id == "company")
                return item[id];
            else
                return null;
        }
        this.exportCsvFile(this.columns,this.csvFileOptions.fileName,this.csvFileOptions.sheetName,computePropertyFnc);
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
