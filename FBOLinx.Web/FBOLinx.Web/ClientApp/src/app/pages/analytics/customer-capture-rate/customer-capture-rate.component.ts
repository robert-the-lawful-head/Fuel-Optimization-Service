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
    reportHiddenItems: ReportFilterItems[] = [
        ReportFilterItems.icaoDropDown
    ];
    tableLocalStorageKey: string;

    chartName = 'customer-capture-rate-table';

    csvFileOptions: csvFileOptions = { fileName: 'Customer Capture Rate', sheetName: 'Customer Capture Rate ' };

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
            name: '% of Customer\'s Business',
        }
    ];

    constructor(private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private tableSettingsDialog: MatDialog,
        private ngxLoader: NgxUiLoaderService) {
        super();
        this.tableLocalStorageKey = `analytics-customer-capture-rate-${this.sharedService.currentUser.fboId}`;
        this.groupId = Number(this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.groupId));
        this.fboId = Number(this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.fboId));
        this.fbo = this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.fbo);
        this.icao = this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.icao);

        this.tableLocalStorageKey = `analytics-companies-quotes-deal-${this.sharedService.currentUser.fboId}`;
        this.columns = this.getClientSavedColumns(
            this.tableLocalStorageKey,
            this.columns
        );

        this.filterStartDate = new Date(
            moment().add(-7, 'd').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date();
        this.selectedDateFilter = {
            selectedFilter: null,
            offsetDate: this.filterStartDate,
            limitDate: this.filterEndDate,
        }
        this.refreshData();
    }
    ngOnInit(): void {
    }
    refreshData() {
        let endDate = this.getEndOfDayTime(this.filterEndDate, true);
        let startDate = this.getStartOfDayTime(this.filterStartDate, true);

        this.ngxLoader.startLoader(this.chartName);
        this.fuelreqsService.getCustomerCaptureRate(this.groupId,this.fboId, startDate, endDate).subscribe((data: CustomerCaptureRateReport[]) => {
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
    exportCsv() {
        this.exportCsvFile(this.columns,this.csvFileOptions.fileName,this.csvFileOptions.sheetName,null);
    }

    applyPresetDateFilter(filter: SelectedDateFilter) {
        this.filterEndDate = filter.limitDate;
        this.filterStartDate = filter.offsetDate;
        this.refreshData();
    }
    openSettings() {
        var _this = this;
        this.openSettingsDialog(this.tableSettingsDialog, this.columns, function (result) {
            _this.columns = result;
            _this.refreshSort(_this.sort, _this.columns);
            _this.saveSettings(this.tableLocalStorageKey,this.columns);
        });
    }
    get visibleColumns() {
        return (
            this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.id) || []
        );
    }
}
