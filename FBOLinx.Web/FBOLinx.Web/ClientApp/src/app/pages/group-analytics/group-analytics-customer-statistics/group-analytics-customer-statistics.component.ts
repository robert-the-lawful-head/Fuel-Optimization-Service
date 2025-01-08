import {
    AfterViewInit,
    Component,
    Input,
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
import { Subject, Subscription, interval } from 'rxjs';
import { debounceTime, takeWhile } from 'rxjs/operators';
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
import * as SharedEvent from '../../../constants/sharedEvents';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';

@Component({
    selector: 'app-group-analytics-customer-statistics',
    styleUrls: ['./group-analytics-customer-statistics.component.scss'],
    templateUrl: './group-analytics-customer-statistics.component.html',
})
export class GroupAnalyticsCustomerStatisticsComponent
    implements OnInit, AfterViewInit, OnDestroy
{
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @Input() fbos: any[];

    filterStartDate: Date;
    filterEndDate: Date;
    icao: string;
    fbo: string;
    icaoChangedSubscription: any;
    chartName = 'group-analytics-customer-statistics';
    dataSource: MatTableDataSource<any[]> = new MatTableDataSource();
    filtersChanged: Subject<any> = new Subject<any>();

    selectedFbos: any[];

    tableLocalStorageKey: string;
    columns: ColumnType[] = [];

    pageIndex = 0;
    pageSize = 10;
    dataLength = 0;
    intervalSubscription: Subscription;
    sortChangeSubscription: Subscription;
    filterChangeSubscription: Subscription;
    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private exportDialog: MatDialog,
        private tableSettingsDialog: MatDialog
    ) {
        this.icao = this.sharedService.currentUser.icao;
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.filterStartDate = new Date(
            moment().add(-1, 'M').format('MM/DD/YYYY')
        );


        this.filterChangeSubscription = this.filtersChanged
            .pipe(debounceTime(2000))
            .subscribe(() => this.refreshData());

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

        if (this.fbos == undefined) {
            var isFbosLoaded = false;
            this.intervalSubscription = interval(1000)
                .pipe(takeWhile(() => !isFbosLoaded))
                .subscribe(() => {
                    if (this.fbos == undefined)
                        this.ngxLoader.startLoader(this.chartName);
                    else {
                        this.ngxLoader.stopLoader(this.chartName);
                        this.selectedFbos = this.fbos;
                        this.refreshData();
                        isFbosLoaded = true;
                    }
                });
        }
        else {
            this.ngxLoader.stopLoader(this.chartName);
            this.selectedFbos = this.fbos;
            this.refreshData();
        }
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
        this.sortChangeSubscription?.unsubscribe();
        this.filterChangeSubscription?.unsubscribe();   
        this.intervalSubscription?.unsubscribe();
        if (this.icaoChangedSubscription) {
            this.icaoChangedSubscription.unsubscribe();
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

    fetchData(fboIds: number[], startDate: Date, endDate: Date) {
        return this.fuelreqsService.getCompaniesQuotingDealStatisticsForGroupFbos(
            this.sharedService.currentUser.groupId,
            fboIds,
            startDate,
            endDate
        );
    }

    initColumns() {


        this.tableLocalStorageKey = `group-analytics-customer-statistics-${this.sharedService.currentUser.groupId}`;
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
                    name: 'Total Orders',
                },
                {
                    id: 'lastPullDate',
                    name: 'Last Quoted',
                    sort: 'desc',
                },
            ];
        }
    }

    refreshData() {
         this.ngxLoader.startLoader(this.chartName);
        this.fetchData(
            this.selectedFbos.map((fbo) => fbo.oid),
            this.filterStartDate,
            this.filterEndDate
        ).subscribe(
            (data: any) => {
                this.dataLength = data.length;
                
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

    filterChanged() {
        this.filtersChanged.next({});
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
        this.fetchData(
            this.fbos.map((fbo) => fbo.oid),
            startDate,
            endDate
        ).subscribe((data: any[]) => {
            const exportData = data.map((item) => {
                const row = {
                    Company: item.company,
                    'Conversion Rate': item.conversionRate + '%',
                    'Direct Orders': item.directOrders,
                    'Last Quoted': item.lastPullDate,
                    'Number of Quotes': item.companyQuotesTotal,
                    'Total Orders': item.totalOrders,
                };
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

    onPageChanged(event: any) {
        localStorage.setItem('pageIndex', event.pageIndex);
        sessionStorage.setItem(
            'pageSizeValue',
            this.paginator.pageSize.toString()
        );
    }
}
