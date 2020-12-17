import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
    ChangeDetectionStrategy,
    OnChanges,
    SimpleChanges
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

// Services
import { SharedService } from '../../../layouts/shared-service';

// Shared components
import { FuelReqsExportModalComponent } from '../../../shared/components/fuelreqs-export/fuelreqs-export.component';
import { ColumnType, TableSettingsComponent } from '../../../shared/components/table-settings/table-settings.component';

const initialColumns: ColumnType[] = [
    {
        id: 'customer',
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
];

@Component({
    selector: 'app-fuelreqs-grid',
    templateUrl: './fuelreqs-grid.component.html',
    styleUrls: ['./fuelreqs-grid.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FuelreqsGridComponent implements OnInit, OnChanges {
    @Output() dateFilterChanged = new EventEmitter<any>();
    @Output() exportTriggered = new EventEmitter<any>();
    @Input() fuelreqsData: any[];
    @Input() filterStartDate: Date;
    @Input() filterEndDate: Date;

    tableLocalStorageKey = 'fuel-req-table-settings';

    pageIndex = 0;
    pageSize = 100;


    fuelreqsDataSource: MatTableDataSource<any> = null;
    resultsLength = 0;
    visibleColumns: ColumnType[] = [];
    invisibleColumns: ColumnType[] = [];

    dashboardSettings: any;

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    constructor(
        private sharedService: SharedService,
        private exportDialog: MatDialog,
        private tableSettingsDialog: MatDialog,
    ) {
        this.dashboardSettings = this.sharedService.dashboardSettings;
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.fuelreqsData) {
            this.refreshTable();
        }
    }

    ngOnInit() {
        this.sort.sortChange.subscribe(() => {
            this.visibleColumns = this.visibleColumns.map(column =>
                column.id === this.sort.active ? { ...column, sort: this.sort.direction} : { id: column.id, name: column.name }
            );
            this.paginator.pageIndex = 0
            this.saveSettings();
        });

        if (localStorage.getItem('pageIndexFuelReqs')) {
            this.paginator.pageIndex = localStorage.getItem('pageIndexFuelReqs') as any;
        } else {
            this.paginator.pageIndex = 0;
        }

        if (sessionStorage.getItem('pageSizeValueFuelReqs')) {
            this.pageSize = sessionStorage.getItem('pageSizeValueFuelReqs') as any;
        } else {
            this.pageSize = 100;
        }

        if (localStorage.getItem(this.tableLocalStorageKey)) {
            const { visibleColumns, invisibleColumns } = JSON.parse(localStorage.getItem(this.tableLocalStorageKey));
            this.visibleColumns = visibleColumns;
            this.invisibleColumns = invisibleColumns;
        } else {
            this.visibleColumns = initialColumns;
            this.invisibleColumns = [];
        }
        this.refreshTable();
    }

    getTableColumns() {
        return this.visibleColumns.map(column => column.id);
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
    }

    refreshSort() {
        const sortedColumn = this.visibleColumns.find(column => column.sort);
        this.sort.sort({ id: null, start: sortedColumn?.sort || "asc", disableClear: false });
        this.sort.sort({ id: sortedColumn?.id, start: sortedColumn?.sort || "asc", disableClear: false });
        (this.sort.sortables.get(sortedColumn?.id) as MatSortHeader)?._setAnimationTransitionState({ toState: 'active' });
    }

    applyFilter(filterValue: string) {
        this.fuelreqsDataSource.filter = filterValue.trim().toLowerCase();
    }

    applyDateFilterChange() {
        this.dateFilterChanged.emit({
            filterStartDate: this.filterStartDate,
            filterEndDate: this.filterEndDate,
        });
    }

    export() {
        const dialogRef = this.exportDialog.open(
            FuelReqsExportModalComponent,
            {
                data: {
                    filterStartDate: this.filterStartDate,
                    filterEndDate: this.filterEndDate,
                },
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.exportTriggered.emit(result);
        });
    }

    onPageChanged(event: any) {
        localStorage.setItem('pageIndexFuelReqs', event.pageIndex);
        sessionStorage.setItem(
            'pageSizeValueFuelReqs',
            this.paginator.pageSize.toString()
        );
    }

    openSettings() {
        const dialogRef = this.tableSettingsDialog.open(TableSettingsComponent, {
            data: {
                visibleColumns: this.visibleColumns,
                invisibleColumns: this.invisibleColumns,
            }
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                const { visibleColumns, invisibleColumns } = result;
                this.visibleColumns = visibleColumns;
                this.invisibleColumns = invisibleColumns;
                this.refreshSort();
                this.saveSettings();
            }
        })
    }

    saveSettings() {
        localStorage.setItem(this.tableLocalStorageKey, JSON.stringify({
            visibleColumns: this.visibleColumns,
            invisibleColumns: this.invisibleColumns,
        }));
    }
}
