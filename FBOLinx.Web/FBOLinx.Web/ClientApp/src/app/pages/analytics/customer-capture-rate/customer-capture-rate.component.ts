import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { GridBase, csvFileOptions } from 'src/app/services/tables/GridBase';
import { ReportFilterItems } from '../analytics-report-popup/report-filters/report-filters.component';
import { SelectedDateFilter } from 'src/app/shared/components/preset-date-filter/preset-date-filter.component';
import { ColumnType } from 'src/app/shared/components/table-settings/table-settings.component';
import { FuelreqsService } from 'src/app/services/fuelreqs.service';
import { SharedService } from 'src/app/layouts/shared-service';
import { localStorageAccessConstant } from 'src/app/models/LocalStorageAccessConstant';
import { CustomerCaptureRateReport } from 'src/app/models/customer-capture-rate-report';
import { MatDialog } from '@angular/material/dialog';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as moment from 'moment';

@Component({
    selector: 'app-customer-capture-rate',
    templateUrl: './customer-capture-rate.component.html',
    styleUrls: ['./customer-capture-rate.component.scss'],
})
export class CustomerCaptureRateComponent extends GridBase implements OnInit {
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;

    data: CustomerCaptureRateReport[];
    groupId: number;
    fboId: number;
    fbo: string;
    icao: string;
    reportHiddenItems: ReportFilterItems[] = [ReportFilterItems.icaoDropDown];
    tableLocalStorageKey: string;

    chartName = 'customer-capture-rate-table';

    csvFileOptions: csvFileOptions = {
        fileName: 'Customer Capture Rate',
        sheetName: 'Customer Capture Rate ',
    };

    columns: ColumnType[] = [
        {
            id: 'company',
            name: 'Company',
        },
        {
            id: 'airportOrders',
            name: 'Total Orders at airport',
        },
        {
            id: 'totalOrders',
            name: `Total Orders at FBO`,
        },
        {
            id: 'percentCustomerBusiness',
            name: "% of Customer's Business",
        },
    ];

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private tableSettingsDialog: MatDialog,
        private ngxLoader: NgxUiLoaderService
    ) {
        super();
        this.tableLocalStorageKey = `analytics-customer-capture-rate-${this.sharedService.currentUser.fboId}`;
        this.initColumns();
        this.groupId = Number(
            this.sharedService.getCurrentUserPropertyValue(
                localStorageAccessConstant.groupId
            )
        );
        this.fboId = Number(
            this.sharedService.getCurrentUserPropertyValue(
                localStorageAccessConstant.fboId
            )
        );
        this.fbo = this.sharedService.getCurrentUserPropertyValue(
            localStorageAccessConstant.fbo
        );
        this.icao = this.sharedService.getCurrentUserPropertyValue(
            localStorageAccessConstant.icao
        );

        this.tableLocalStorageKey = `analytics-companies-quotes-deal-${this.sharedService.currentUser.fboId}`;
        this.columns = this.getClientSavedColumns(
            this.tableLocalStorageKey,
            this.columns
        );

        this.refreshData();
    }
    ngOnInit(): void {
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

            this.saveSettings(this.tableLocalStorageKey, this.columns);
        });
    }
    initColumns() {
        this.tableLocalStorageKey = `analytics-airport-arrivals-depatures-${this.sharedService.currentUser.fboId}`;
        this.columns = this.getClientSavedColumns(
            this.tableLocalStorageKey,
            this.columns
        );
    }
    refreshData() {
        let endDate = this.getEndOfDayTime(this.filterEndDate, true);
        let startDate = this.getStartOfDayTime(this.filterStartDate, true);

        this.ngxLoader.startLoader(this.chartName);
        this.fuelreqsService
            .getCustomerCaptureRate(
                this.groupId,
                this.fboId,
                startDate,
                endDate
            )
            .subscribe(
                (data: CustomerCaptureRateReport[]) => {
                    this.dataSource.data = data;
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
    // Custom sorting function
    private getPropertyValue(item: any, property: string): any {
        const properties = property.split('.');
        return properties.reduce((obj, prop) => obj && obj[prop], item);
    }
    exportCsv() {
        const data = this.dataSource.data.slice();

        if (!this.sort.active || this.sort.direction === '') {
            this.dataSource.data = data;
        }

        // Custom sorting logic for three columns
        this.dataSource.data = data.sort((a, b) => {
            const firstLevel = 'percentCustomerBusiness';
            const valueA = this.getPropertyValue(a, firstLevel);
            const valueB = this.getPropertyValue(b, firstLevel);

            if (valueA < valueB) {
                return this.sort.direction === 'asc' ? -1 : 1;
            } else if (valueA > valueB) {
                return this.sort.direction === 'asc' ? 1 : -1;
            }

            // If values at the first level are equal, go to the next level
            const secondLevel = 'totalOrders';
            const secondLevelA = this.getPropertyValue(a, secondLevel);
            const secondLevelB = this.getPropertyValue(b, secondLevel);

            if (secondLevelA < secondLevelB) {
                return this.sort.direction === 'asc' ? -1 : 1;
            } else if (secondLevelA > secondLevelB) {
                return this.sort.direction === 'asc' ? 1 : -1;
            }

            // If values at the second level are equal, go to the next level
            const thirdLevel = 'airportOrders';
            const thirdLevelA = this.getPropertyValue(a, thirdLevel);
            const thirdLevelB = this.getPropertyValue(b, thirdLevel);

            if (thirdLevelA < thirdLevelB) {
                return this.sort.direction === 'asc' ? -1 : 1;
            } else if (thirdLevelA > thirdLevelB) {
                return this.sort.direction === 'asc' ? 1 : -1;
            }

            const fouthLevel = 'company';
            const fouthLevelA = this.getPropertyValue(a, fouthLevel);
            const fouthLevelB = this.getPropertyValue(b, fouthLevel);

            if (fouthLevelA < fouthLevelB) {
                return this.sort.direction === 'asc' ? -1 : 1;
            } else if (fouthLevelA > fouthLevelB) {
                return this.sort.direction === 'asc' ? 1 : -1;
            }

            // If values at the third level are equal, return 0
            return 0;
        });
        let computePropertyFnc = (item: any[], id: string): any => {
            if (id == 'percentCustomerBusiness') {
                let value = item[id] == null ? '' : item[id] + '%';

                if(item[id] == null) return { v: value };

            } else {
                return { v: item[id] };
            }
        };

        this.exportCsvFile(
            this.columns,
            this.csvFileOptions.fileName,
            this.csvFileOptions.sheetName,
            computePropertyFnc
        );
    }

    applyPresetDateFilter(filter: SelectedDateFilter) {
        this.filterEndDate = filter.limitDate;
        this.filterStartDate = filter.offsetDate;
        this.refreshData();
    }
    openSettings() {
        var _this = this;
        this.openSettingsDialog(
            this.tableSettingsDialog,
            this.columns,
            function (result) {
                _this.columns = result;
                _this.refreshSort(_this.sort, _this.columns);
                _this.saveSettings(this.tableLocalStorageKey, this.columns);
            }
        );
    }
    get visibleColumns() {
        return (
            this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.id) || []
        );
    }
}
