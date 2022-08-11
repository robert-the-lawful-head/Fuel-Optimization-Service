import {
    ChangeDetectionStrategy,
    Component,
    OnChanges,
    OnInit,
    Output,
    Input,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { Subject } from 'rxjs';
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
export class MissedOrdersGridComponent implements OnInit {
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    chartName = 'missed-orders-table';



    searchText: string = '';
    filterStartDate: Date;
    filterEndDate: Date;
    filtersChanged: Subject<any> = new Subject<any>();

    tableLocalStorageKey = 'missed-orders-table-settings';

    pageIndex = 0;
    pageSize = 100;

    missedOrdersData: any;
    missedOrdersDataSource: MatTableDataSource<any> = null;
    resultsLength = 0;
    columns: ColumnType[] = [];

    dashboardSettings: any;

    constructor(
        private sharedService: SharedService,
        private tableSettingsDialog: MatDialog,
        private ngxLoader: NgxUiLoaderService,
        private fboMissedQuotesLogService: FbomissedquoteslogService,
        private route: ActivatedRoute
    ) {
        this.dashboardSettings = this.sharedService.dashboardSettings;

        this.route.queryParams.subscribe((params) => {
            if (params.search && params.search) {
                this.searchText = params.search;
            }
        });
    }

    ngOnInit() {
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
            this.paginator.pageIndex = 0;
        });

        if (localStorage.getItem('pageIndexMissedOrders')) {
            this.paginator.pageIndex = localStorage.getItem(
                'pageIndexMissedOrders'
            ) as any;
        } else {
            this.paginator.pageIndex = 0;
        }

        if (sessionStorage.getItem('pageSizeValueFuelReqs')) {
            this.pageSize = sessionStorage.getItem(
                'pageSizeValueFuelReqs'
            ) as any;
        } else {
            this.pageSize = 100;
        }

        this.filterStartDate = new Date(
            moment().add(-1, 'week').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(moment().add(3, 'days').format('MM/DD/YYYY'));
        this.filtersChanged
            .debounceTime(500)
            .subscribe(() => this.refreshTable());
        if (localStorage.getItem(this.tableLocalStorageKey)) {
            this.columns = JSON.parse(
                localStorage.getItem(this.tableLocalStorageKey)
            );
        } else {
            this.columns = initialColumns;
        }

        this.refreshTable();
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

    refreshTable() {
        this.missedOrdersData = null;
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

    onPageChanged(event: any) {
        localStorage.setItem('pageIndexMissedOrders', event.pageIndex);
        sessionStorage.setItem(
            'pageSizeValueMissedOrders',
            this.paginator.pageSize.toString()
        );
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
