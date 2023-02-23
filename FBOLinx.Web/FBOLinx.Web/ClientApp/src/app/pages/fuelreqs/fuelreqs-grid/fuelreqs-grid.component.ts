import { CurrencyPipe, DatePipe } from '@angular/common';
import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnInit,
    Output,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { isEqual } from 'lodash';
import { csvFileOptions, VirtualScrollBase } from 'src/app/services/tables/VirtualScrollBase';

// Services
import { SharedService } from '../../../layouts/shared-service';
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
        id: 'pricingTemplateName',
        name: 'ITP Margin Template',
    },
    {
        id: 'eta',
        name: 'ETA',
        sort: 'desc',
    },
    {
        id: 'etd',
        name: 'ETD',
    },
    {
        id: 'quotedVolume',
        name: 'Volume (gal.)',
    },
    {
        id: 'quotedPpg',
        name: 'PPG',
    },
    {
        id: 'tailNumber',
        name: 'Tail #',
    },
    {
        id: 'phoneNumber',
        name: 'Phone',
    },
    {
        id: 'source',
        name: 'Source',
    },
    {
        id: 'email',
        name: 'Email',
    },
    {
        id: 'oid',
        name: 'ID',
    },
    {
        id: 'sourceId',
        name: 'Fuelerlinx ID',
    },
    {
        id: 'cancelled',
        name: 'Transaction Status',
    },
    {
        id: 'dateCreated',
        name: 'Created',
    },
    {
        id: 'fuelOn',
        name: 'Uplift Request',
    },
];

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-fuelreqs-grid',
    styleUrls: ['./fuelreqs-grid.component.scss'],
    templateUrl: './fuelreqs-grid.component.html',
})
export class FuelreqsGridComponent extends VirtualScrollBase implements OnInit, OnChanges {
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @Output() dateFilterChanged = new EventEmitter<any>();
    @Input() fuelreqsData: any[];
    @Input() filterStartDate: Date;
    @Input() filterEndDate: Date;

    tableLocalStorageKey = 'fuel-req-table-settings';

    fuelreqsDataSource: MatTableDataSource<any> = null;
    resultsLength = 0;
    columns: ColumnType[] = [];

    dashboardSettings: any;

    csvFileOptions: csvFileOptions = { fileName: 'FuelOrders', sheetName: 'Fuel Orders' };

    constructor(
        private sharedService: SharedService,
        private tableSettingsDialog: MatDialog,
        private datePipe: DatePipe,
        private currencyPipe: CurrencyPipe,
    ) {
        super();
        this.dashboardSettings = this.sharedService.dashboardSettings;
    }

    ngOnChanges(changes: SimpleChanges): void {
        var searchFilter = localStorage.getItem("fuel-orders-filters");
        if (
            (searchFilter == null || searchFilter == '') &&
            changes.fuelreqsData &&
            !isEqual(
                changes.fuelreqsData.currentValue,
                changes.fuelreqsData.previousValue
            )
        ) {
            this.refreshTable();
        }
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

        if (localStorage.getItem('pageIndexFuelReqs')) {
            this.paginator.pageIndex = localStorage.getItem(
                'pageIndexFuelReqs'
            ) as any;
        } else {
            this.paginator.pageIndex = 0;
        }


        if (localStorage.getItem(this.tableLocalStorageKey)) {
            if (this.columns.length === 13) {
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
        this.refreshTable();
    }

    getTableColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => {
                if(column.id == 'customer')
                    return 'customerName'
                return column.id
            });
    }

    refreshTable() {
        let filter = '';
        if (this.fuelreqsDataSource) {
            filter = this.fuelreqsDataSource.filter;
        }
        this.fuelreqsDataSource = new MatTableDataSource(this.fuelreqsData);
        this.fuelreqsDataSource.sort = this.sort;
        this.fuelreqsDataSource.paginator = this.paginator;
        this.fuelreqsDataSource.filter = filter;
        this.resultsLength = this.fuelreqsData.length;

        this.refreshSort();

        this.setVirtualScrollVariables(this.paginator, this.sort, this.fuelreqsDataSource.data);
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
        this.fuelreqsDataSource.filter = filterValue.trim().toLowerCase();
    }

    applyDateFilterChange() {
        this.dateFilterChanged.emit({
            filterEndDate: this.filterEndDate,
            filterStartDate: this.filterStartDate,
        });
    }

    exportCsv() {
        let computePropertyFnc = (item: any[], id: string): any => {
            if(id == "eta" || id == "etd" || id == "dateCreated")
                return this.datePipe.transform(item[id]);
            else if(id == "quotedPpg")
                return this.getPPGDisplayString(item);
            else
                return null;
        }
        this.exportCsvFile(this.columns,this.csvFileOptions.fileName,this.csvFileOptions.sheetName,computePropertyFnc);
    }
    getPPGDisplayString(fuelreq: any): any{
        return fuelreq.oid == 0
        ? "CONFIDENTIAL"
        : this.currencyPipe.transform(fuelreq.quotedPpg,"USD","symbol","1.4-4");
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
