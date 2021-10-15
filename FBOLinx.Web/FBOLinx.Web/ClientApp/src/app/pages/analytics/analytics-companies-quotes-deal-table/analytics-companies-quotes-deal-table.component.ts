import { Router } from '@angular/router';
import {
    AfterViewInit,
    Component,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
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

@Component({
    selector: 'app-analytics-companies-quotes-deal',
    styleUrls: ['./analytics-companies-quotes-deal-table.component.scss'],
    templateUrl: './analytics-companies-quotes-deal-table.component.html',
})
export class AnalyticsCompaniesQuotesDealTableComponent
    implements OnInit, AfterViewInit, OnDestroy
{
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
        'totalOrders',
        'airportOrders',
        'lastPullDate',
    ];
    dataSource: MatTableDataSource<any[]>;

    tableLocalStorageKey: string;
    columns: ColumnType[] = [];

    constructor(
        private fuelreqsService: FuelreqsService,
        private fbosService: FbosService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private exportDialog: MatDialog,
        private router: Router,
        private tableSettingsDialog: MatDialog
    ) {
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
        this.tableLocalStorageKey = `analytics-companies-quotes-deal-${this.sharedService.currentUser.fboId}`;
        if (localStorage.getItem(this.tableLocalStorageKey)) {
            this.columns = JSON.parse(
                localStorage.getItem(this.tableLocalStorageKey)
            );
        } else {
            this.columns = [
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
                    id: 'totalOrders',
                    name: `Total Orders at ${this.fbo}`,
                },
                {
                    id: 'airportOrders',
                    name: `Total Orders at ${this.sharedService.currentUser.icao}`,
                },
                {
                    id: 'lastPullDate',
                    name: `${this.sharedService.currentUser.icao} Last Quoted`,
                    sort: 'desc',
                },
            ];
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
                this.dataSource = new MatTableDataSource(data);
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

       this.router.navigate(['/default-layout/customers' , element.oid ], {state : {tab : 3  , customerId : element.customerId}  });
    }

    onExport() {
        const dialogRef = this.exportDialog.open<
            CsvExportModalComponent,
            ICsvExportModalData,
            ICsvExportModalData
        >(CsvExportModalComponent, {
            data: {
                filterEndDate: this.filterEndDate,
                filterStartDate: this.filterStartDate,
                title: 'Export Customer Statistics',
            },
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.exportCsv(result.filterStartDate, result.filterEndDate);
        });
    }

    exportCsv(startDate: Date, endDate: Date) {
        this.fetchData(startDate, endDate).subscribe((data: any) => {
            const exportData = data.map((item) => {
                const row = {
                    Company: item.company,
                    'Conversion Rate': item.conversionRate + '%',
                    'Direct Orders': item.directOrders,
                    'Number of Quotes': item.companyQuotesTotal,
                };
                row[`Total Orders at ${this.fbo}`] = item.totalOrders;
                row[`Total Orders at ${this.icao}`] = item.airportOrders;
                row[`${this.icao} Last Quoted`] = item.lastPullDate;
                return row;
            });
            const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
            const wb: XLSX.WorkBook = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, 'Customer Statistics');

            /* save to file */
            XLSX.writeFile(wb, 'Customer Statistics.xlsx');
        });
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
