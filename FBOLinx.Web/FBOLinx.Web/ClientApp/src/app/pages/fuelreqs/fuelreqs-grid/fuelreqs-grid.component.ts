import { trigger, state, style } from '@angular/animations';
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
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { isEqual } from 'lodash';
import { csvFileOptions, GridBase } from 'src/app/services/tables/GridBase';

// Services
import { SharedService } from '../../../layouts/shared-service';
// Shared components
import {
    ColumnType,
    TableSettingsComponent,
} from '../../../shared/components/table-settings/table-settings.component';
import { ServiceOrdersDialogOrderItemsComponent } from '../../service-orders/service-orders-dialog-order-items/service-orders-dialog-order-items.component';

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
    //not show services untill full feature is merged into develop
    // {
    //     id: 'services',
    //     name: 'Services',
    // }
];

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-fuelreqs-grid',
    styleUrls: ['./fuelreqs-grid.component.scss'],
    templateUrl: './fuelreqs-grid.component.html',
    animations: [
        trigger('detailExpand', [
            state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
            state('expanded', style({ height: '*' }))
          ])
    ]
})
export class FuelreqsGridComponent extends GridBase implements OnInit, OnChanges {
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

    allColumnsToDisplay: string[];

    expandedElement: any[] = [];

    constructor(
        private sharedService: SharedService,
        private tableSettingsDialog: MatDialog,
        private datePipe: DatePipe,
        private currencyPipe: CurrencyPipe,
        private serviceOrderItemDialog: MatDialog
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
            this.allColumnsToDisplay = this.getVisibleColumns();
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

        this.columns = this.getClientSavedColumns(this.tableLocalStorageKey, initialColumns);

        this.allColumnsToDisplay = this.getVisibleColumns();

        this.refreshTable();
    }
    getVisibleDataColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => {
                if(column.id == 'customer')
                    return 'customerName'
                return column.id
            }) || [];
    }
    getVisibleColumns() {
        var result = ['expand-icon'];
        result.push(...
            this.getVisibleDataColumns()
        );

        return result;
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
        return fuelreq.oid == 0 || fuelreq.quotedPpg == 0
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
    getNoDataToDisplayString(){
        return"No Fuel Request set on the selected range of dates";
    }
    isRowExpanded(elementId: any){
        return this.expandedElement.includes(elementId);
    }
    toogleExpandedRows(elementId: any){
        if(this.isRowExpanded(elementId)){
            this.expandedElement = this.expandedElement.filter(function(item) {
                return item !== elementId
            })
        }else{
            this.expandedElement.push(elementId);
        }

    }

    manageServiceOrderClicked(fuelreq: any) {
        const config: MatDialogConfig = {
            disableClose: true,
            data: fuelreq.serviceOrder
        };
        const dialogRef = this.serviceOrderItemDialog.open(ServiceOrdersDialogOrderItemsComponent, config);
    }
}
