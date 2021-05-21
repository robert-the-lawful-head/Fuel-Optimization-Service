import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as moment from 'moment';
import * as XLSX from 'xlsx';

// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvent from '../../../models/sharedEvents';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { Subject } from 'rxjs';
import { ColumnType, TableSettingsComponent } from 'src/app/shared/components/table-settings/table-settings.component';
import { MatDialog } from '@angular/material/dialog';
import { CsvExportModalComponent, ICsvExportModalData } from 'src/app/shared/components/csv-export-modal/csv-export-modal.component';

@Component({
    selector: 'app-group-analytics-customer-statistics',
    templateUrl: './group-analytics-customer-statistics.component.html',
    styleUrls: ['./group-analytics-customer-statistics.component.scss'],
})
export class GroupAnalyticsCustomerStatisticsComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @Input() fbos: any[];

    filterStartDate: Date;
    filterEndDate: Date;
    icao: string;
    fbo: string;
    icaoChangedSubscription: any;
    chartName = 'group-analytics-customer-statistics';
    displayedColumns: string[] = ['company', 'directOrders', 'companyQuotesTotal', 'conversionRate', 'totalOrders', 'lastPullDate'];
    dataSource: MatTableDataSource<any[]>;
    filtersChanged: Subject<any> = new Subject<any>();

    selectedFbos: any[] = [];

    tableLocalStorageKey: string;
    columns: ColumnType[] = [];

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private exportDialog: MatDialog,
        private tableSettingsDialog: MatDialog,
    ) {
        this.icao = this.sharedService.currentUser.icao;
        this.filterStartDate = new Date(moment().add(-12, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().add(7, 'd').format('MM/DD/YYYY'));

        this.filtersChanged
            .debounceTime(2000)
            .subscribe(() => this.refreshData());

        this.initColumns();
    }

    get visibleColumns() {
        return this.columns.filter(column => !column.hidden).map(column => column.id) || [];
    }

    ngOnInit() {
        this.sort.sortChange.subscribe(() => {
            this.columns = this.columns.map(column =>
                column.id === this.sort.active
                    ? { ...column, sort: this.sort.direction }
                    : { id: column.id, name: column.name, hidden: column.hidden }
            );

            this.saveSettings();
        });
    }

    ngAfterViewInit() {
        this.icaoChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvent.icaoChangedEvent) {
                    this.icao = this.sharedService.currentUser.icao;
                }
            }
        );
    }

    ngOnDestroy() {
        if (this.icaoChangedSubscription) {
            this.icaoChangedSubscription.unsubscribe();
        }
    }

    refreshSort() {
        const sortedColumn = this.columns.find(column => !column.hidden && column.sort);
        this.sort.sort({ id: null, start: sortedColumn?.sort || 'asc', disableClear: false });
        this.sort.sort({ id: sortedColumn?.id, start: sortedColumn?.sort || 'asc', disableClear: false });
        (this.sort.sortables.get(sortedColumn?.id) as MatSortHeader)?._setAnimationTransitionState({ toState: 'active' });
    }


    fetchData(fboIds: number[], startDate: Date, endDate: Date) {
        return this.fuelreqsService
            .getCompaniesQuotingDealStatisticsForGroupFbos(
                this.sharedService.currentUser.groupId,
                fboIds,
                this.filterStartDate,
                this.filterEndDate
            );
    }

    initColumns() {
        this.tableLocalStorageKey = `group-analytics-customer-statistics-${this.sharedService.currentUser.groupId}`;
        if (localStorage.getItem(this.tableLocalStorageKey)) {
            this.columns = JSON.parse(localStorage.getItem(this.tableLocalStorageKey));
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
            this.selectedFbos.map(fbo => fbo.oid),
            this.filterStartDate,
            this.filterEndDate,
        ).subscribe((data: any) => {
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
        }, () => {
        }, () => {
            this.ngxLoader.stopLoader(this.chartName);
        });
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    filterChanged() {
        this.filtersChanged.next();
    }

    onExport() {
        const dialogRef = this.exportDialog.open<CsvExportModalComponent, ICsvExportModalData, ICsvExportModalData>(
            CsvExportModalComponent,
            {
                data: {
                    title: 'Export Customer Statistics',
                    filterStartDate: this.filterStartDate,
                    filterEndDate: this.filterEndDate,
                },
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.exportCsv(result.filterStartDate, result.filterEndDate);
        });
    }

    exportCsv(startDate: Date, endDate: Date) {
        this.fetchData(this.fbos.map(fbo => fbo.oid), startDate, endDate)
            .subscribe((data: any[]) => {
                const exportData = data.map((item) => {
                    const row = {
                        Company: item.company,
                        'Direct Orders': item.directOrders,
                        'Number of Quotes': item.companyQuotesTotal,
                        'Conversion Rate': item.conversionRate + '%',
                        'Total Orders': item.totalOrders,
                        'Last Quoted': item.lastPullDate,
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
        const dialogRef = this.tableSettingsDialog.open(TableSettingsComponent, {
            data: this.columns
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.columns = [ ...result ];

            this.refreshSort();
            this.saveSettings();
        });
    }

    saveSettings() {
        localStorage.setItem(this.tableLocalStorageKey, JSON.stringify(this.columns));
    }
}
