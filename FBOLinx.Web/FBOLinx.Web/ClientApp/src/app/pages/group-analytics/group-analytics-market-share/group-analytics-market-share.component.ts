import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as moment from 'moment';
import * as XLSX from 'xlsx';

// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvent from '../../../models/sharedEvents';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { ColumnType, TableSettingsComponent } from 'src/app/shared/components/table-settings/table-settings.component';
import { MatDialog } from '@angular/material/dialog';
import { CsvExportModalComponent, ICsvExportModalData } from 'src/app/shared/components/csv-export-modal/csv-export-modal.component';

@Component({
    selector: 'app-group-analytics-market-share',
    templateUrl: './group-analytics-market-share.component.html',
    styleUrls: ['./group-analytics-market-share.component.scss'],
})
export class GroupAnalyticsMarketShareComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    filterStartDate: Date;
    filterEndDate: Date;
    icao: string;
    fbo: string;
    icaoChangedSubscription: any;
    chartName = 'group-analytics-market-share';
    dataSource: MatTableDataSource<any[]>;

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
        this.refreshData();
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


    fetchData(startDate: Date, endDate: Date) {
        return this.fuelreqsService
            .getMarketShareFbosAirports(
                this.sharedService.currentUser.groupId,
                startDate,
                endDate
            );
    }

    initColumns() {
        this.tableLocalStorageKey = `group-analytics-market-share-${this.sharedService.currentUser.groupId}`;
        if (localStorage.getItem(this.tableLocalStorageKey)) {
            this.columns = JSON.parse(localStorage.getItem(this.tableLocalStorageKey));
        } else {
            this.columns = [
                {
                    id: 'icao',
                    name: 'ICAO',
                },
                {
                    id: 'airportOrders',
                    name: 'Total Orders at Airport',
                },
                {
                    id: 'fboOrders',
                    name: 'Your Orders at Airport',
                },
                {
                    id: 'marketShare',
                    name: 'Market Share',
                },
            ];
        }
    }

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fetchData(
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
        this.fetchData(startDate, endDate)
            .subscribe((data: any[]) => {
                const exportData = data.map((item) => {
                    const row = {
                        ICAO: item.company,
                        'Total Orders at Airport': item.airportOrders,
                        'Your Orders at Airport': item.fboOrders,
                        'Market Share': item.marketShare + '%',
                    };
                    return row;
                });
                const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
                const wb: XLSX.WorkBook = XLSX.utils.book_new();
                XLSX.utils.book_append_sheet(wb, ws, 'Market Share by Airport');

                /* save to file */
                XLSX.writeFile(wb, 'Market Share by Airport.xlsx');
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
