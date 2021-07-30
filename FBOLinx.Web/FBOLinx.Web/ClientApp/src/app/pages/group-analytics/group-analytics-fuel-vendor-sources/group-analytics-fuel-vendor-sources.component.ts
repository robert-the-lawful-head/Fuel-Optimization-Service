import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { flatMap, isEqual, uniq } from 'lodash';
import * as moment from 'moment';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as XLSX from 'xlsx';

import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvent from '../../../models/sharedEvents';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { CsvExportModalComponent, ICsvExportModalData } from '../../../shared/components/csv-export-modal/csv-export-modal.component';
import { ColumnType, TableSettingsComponent } from '../../../shared/components/table-settings/table-settings.component';

@Component({
    selector: 'app-group-analytics-fuel-vendor-sources',
    styleUrls: ['./group-analytics-fuel-vendor-sources.component.scss'],
    templateUrl: './group-analytics-fuel-vendor-sources.component.html',
})
export class GroupAnalyticsFuelVendorSourcesComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    filterStartDate: Date;
    filterEndDate: Date;
    icao: string;
    fbo: string;
    icaoChangedSubscription: any;
    chartName = 'group-analytics-fuel-vendor-sources';
    dataSource: MatTableDataSource<any[]>;
    vendors: string[] = [];

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

        this.refreshColumns();
    }

    get visibleColumns() {
        return this.columns.filter(column => !column.hidden).map(column => column.id) || [];
    }

    ngOnInit() {
        this.sort.sortChange.subscribe(() => {
            this.columns = this.columns.map(column =>
                column.id === this.sort.active
                    ? { ...column, sort: this.sort.direction }
                    : { hidden: column.hidden, id: column.id, name: column.name }
            );
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
        this.sort.sort({ disableClear: false, id: null, start: sortedColumn?.sort || 'asc' });
        this.sort.sort({ disableClear: false, id: sortedColumn?.id, start: sortedColumn?.sort || 'asc' });
        (this.sort.sortables.get(sortedColumn?.id) as MatSortHeader)?._setAnimationTransitionState({ toState: 'active' });
    }


    fetchData(startDate: Date, endDate: Date) {
        return this.fuelreqsService
            .getFuelVendorSourcesByAirports(
                this.sharedService.currentUser.groupId,
                startDate,
                endDate
            );
    }

    refreshColumns() {
        // this.tableLocalStorageKey = `group-analytics-fuel-vendor-sources-${this.sharedService.currentUser.groupId}`;
        this.columns = [
            {
                id: 'icao',
                name: 'ICAO',
            },
            {
                id: 'directOrders',
                name: 'Directs',
            },
            ...this.vendors.map(vendor => ({
                id: vendor,
                name: vendor
            }))
        ];
    }

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fetchData(
            this.filterStartDate,
            this.filterEndDate,
        ).subscribe((data: any) => {
            const vendors = uniq(flatMap(data, row => row.vendorOrders).map(v => v.contractFuelVendor));
            if (!isEqual(this.vendors, vendors)) {
                this.vendors = vendors;
            }

            const tableData = data.map(row => {
                const tableRow = {
                    directOrders: row.directOrders,
                    icao: row.icao
                };
                for (const vendor of row.vendorOrders) {
                    tableRow[vendor.contractFuelVendor] = vendor.transactionsCount;
                }
                return tableRow;
            });

            this.dataSource = new MatTableDataSource(tableData);
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

            this.refreshColumns();
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
                    filterEndDate: this.filterEndDate,
                    filterStartDate: this.filterStartDate,
                    title: 'Export Customer Statistics',
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
                const vendors = uniq(flatMap(data, row => row.vendorOrders).map(v => v.contractFuelVendor));

                const tableData = data.map(row => {
                    const tableRow = {
                        directOrders: row.directOrders,
                        icao: row.icao
                    };
                    for (const vendor of row.vendorOrders) {
                        tableRow[vendor.contractFuelVendor] = vendor.transactionsCount;
                    }
                    return tableRow;
                });
                const exportData = tableData.map((item) => {
                    const row = {
                        'Direct Orders': item.directOrders,
                        ICAO: item.icao,
                    };
                    for (const vendor of vendors) {
                        row[vendor] = item[vendor];
                    }
                    return row;
                });
                const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
                const wb: XLSX.WorkBook = XLSX.utils.book_new();
                XLSX.utils.book_append_sheet(wb, ws, 'Fuel Vendor Share by Airport');

                /* save to file */
                XLSX.writeFile(wb, 'Fuel Vendor Share by Airport.xlsx');
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
