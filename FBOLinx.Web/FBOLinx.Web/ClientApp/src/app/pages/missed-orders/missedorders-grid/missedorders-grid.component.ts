import {
    ChangeDetectionStrategy,
    Component,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { MatLegacyPaginator as MatPaginator } from '@angular/material/legacy-paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatLegacyTableDataSource as MatTableDataSource } from '@angular/material/legacy-table';
import * as moment from 'moment';
import { Subject, Subscription } from 'rxjs';
import { NgxUiLoaderService } from 'ngx-ui-loader';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { FbomissedquoteslogService } from '../../../services/fbomissedquoteslog.service';
import { ActivatedRoute } from '@angular/router';

// Shared components
import {
    ColumnType,
    TableSettingsComponent,
} from '../../../shared/components/table-settings/table-settings.component';
import { GridBase, csvFileOptions } from 'src/app/services/tables/GridBase';
import { SelectedDateFilter } from 'src/app/shared/components/preset-date-filter/preset-date-filter.component';
import { ReportFilterItems } from '../../analytics/analytics-report-popup/report-filters/report-filters.component';
import { debounceTime } from 'rxjs/operators';

const initialColumns: ColumnType[] = [
    {
        id: 'customerName',
        name: 'Flight Dept.',
    },
    {
        id: 'itpMarginTemplate',
        name: 'ITP Margin Template',
    },
    {
        id: 'eta',
        name: 'ETA'
    },
    {
        id: 'etd',
        name: 'ETD',
    },
    {
        id: 'volume',
        name: 'Volume (gal.)',
    },
    {
        id: 'tailNumber',
        name: 'Tail #',
    },
    {
        id: 'createdDate',
        name: 'Created',
    sort: 'desc',
    },
];

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-missedorders-grid',
    styleUrls: ['./missedorders-grid.component.scss'],
    templateUrl: './missedorders-grid.component.html',
})
export class MissedOrdersGridComponent extends GridBase implements OnInit {
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    chartName = 'missed-orders-table';

    searchText: string = '';

    filtersChanged: Subject<any> = new Subject<any>();

    tableLocalStorageKey = 'missed-orders-table-settings';

    missedOrdersData: any;
    missedOrdersDataSource: MatTableDataSource<any> = null;
    resultsLength = 0;
    columns: ColumnType[] = [];

    dashboardSettings: any;

    resetMissedOrdersSubscription: any;

    csvFileOptions: csvFileOptions = { fileName: 'Missed Orders', sheetName: 'Missed Orders' };
    reportHiddenItems: ReportFilterItems[] = [ReportFilterItems.icaoDropDown, ReportFilterItems.isCommercialAircraft];
    filtersChangedSubscription: Subscription;
    sortChangeSubscription: Subscription;
    routeQueryParamsSubscription: Subscription;
    constructor(
        private sharedService: SharedService,
        private tableSettingsDialog: MatDialog,
        private ngxLoader: NgxUiLoaderService,
        private fboMissedQuotesLogService: FbomissedquoteslogService,
        private route: ActivatedRoute
    ) {
        super();
        this.dashboardSettings = this.sharedService.dashboardSettings;

        this.routeQueryParamsSubscription = this.route.queryParams.subscribe((params) => {
            if (params.search && params.search) {
                this.searchText = params.search;
            }
        });
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
            this.paginator.pageIndex = 0;
        });

        if (localStorage.getItem('pageIndexMissedOrders')) {
            this.paginator.pageIndex = localStorage.getItem(
                'pageIndexMissedOrders'
            ) as any;
        } else {
            this.paginator.pageIndex = 0;
        }

        this.filtersChangedSubscription = this.filtersChanged
            .pipe(debounceTime(500))
            .subscribe(() => this.refreshTable());

        this.columns = this.getClientSavedColumns(this.tableLocalStorageKey, initialColumns);

        this.refreshTable();
    }

    ngOnDestroy() {
        this.routeQueryParamsSubscription?.unsubscribe();
        this.filtersChangedSubscription?.unsubscribe();
        if (this.resetMissedOrdersSubscription) {
            this.resetMissedOrdersSubscription.unsubscribe();
        }
    }

    getTableColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => {
                if (column.id == 'customer')
                    return 'customerName'
                return column.id
            });
    }
    get visibleColumns() {
        return this.columns
        .filter((column) => !column.hidden)
        .map((column) => {
            if (column.id == 'customer')
                return 'customerName'
            return column.id
        });
    }

    refreshTable() {
        this.ngxLoader.startLoader(this.chartName);
        this.fetchData().subscribe(
            (data: any[]) => {
                this.missedOrdersData = data;

                this.refreshDataSource();
            },
            () => { },
            () => {
                this.ngxLoader.stopLoader(this.chartName);
            }
        );
    }

    fetchData() {
        return this.fboMissedQuotesLogService.getMissedOrders(this.sharedService.currentUser.fboId, moment(this.filterStartDate).format('MM/DD/YYYY HH:ss'), moment(this.filterEndDate).format('MM/DD/YYYY HH:ss'));
    }

    refreshDataSource() {
        let filter = '';
        if (this.missedOrdersDataSource) {
            filter = this.missedOrdersDataSource.filter;
        }

        this.missedOrdersDataSource = new MatTableDataSource(this.missedOrdersData);
        this.missedOrdersDataSource.sort = this.sort;
        this.missedOrdersDataSource.paginator = this.paginator;
        this.missedOrdersDataSource.filter = filter;
        this.resultsLength = this.missedOrdersData.length;

        this.refreshSort();

        this.applyFilter(this.searchText);

        this.setVirtualScrollVariables(this.paginator, this.sort, this.missedOrdersDataSource.data);
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

    applyFilter(filterValue: string) {
        this.missedOrdersDataSource.filter = filterValue.trim().toLowerCase();
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
    exportCsv() {
        this.exportCsvFile(this.columns,this.csvFileOptions.fileName,this.csvFileOptions.sheetName,null);
    }
    applyPresetDateFilter(filter: SelectedDateFilter) {
        this.filterEndDate = filter.limitDate;
        this.filterStartDate = filter.offsetDate;
        this.refreshTable();
    }
}
