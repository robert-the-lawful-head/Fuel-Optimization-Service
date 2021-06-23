import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild, } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { forEach, map, sortBy } from 'lodash';
import * as XLSX from 'xlsx';

// Components
import { ColumnType, TableSettingsComponent } from '../../../shared/components/table-settings/table-settings.component';

import { AirportWatchService } from '../../../services/airportwatch.service';


const initialColumns: ColumnType[] = [
    {
        id: 'selectAll',
        name: 'Select All',
    },
    {
        id: 'company',
        name: 'Company',
        sort: 'asc',
    },
    {
        id: 'isFuelerLinxCustomer',
        name: 'FuelerLinx Network',
    },
    {
        id: 'certificateTypeDescription',
        name: 'Certificate Type',
    },
];

@Component({
    selector: 'app-group-customers-grid',
    templateUrl: './group-customers-grid.component.html',
    styleUrls: [ './group-customers-grid.component.scss' ],
})
export class GroupCustomersGridComponent implements OnInit {
    // Input/Output Bindings
    @Input() customersData: any[];

    @Output() editCustomerClicked = new EventEmitter<any>();
    @Output() customerDeleted = new EventEmitter<any>();

    // Members
    @ViewChild('customerTableContainer') table: ElementRef;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    tableLocalStorageKey = 'group-customer-manager-table-settings';

    customersDataSource: any = null;
    customerFilterType: number = undefined;
    selectAll = false;
    selectedRows: number;
    pageIndex = 0;
    pageSize = 100;
    columns: ColumnType[] = [];
    airportWatchStartDate: Date = new Date();

    LICENSE_KEY = '9eef62bd-4c20-452c-98fd-aa781f5ac111';

    results = '[]';

    constructor(
        private tableSettingsDialog: MatDialog,
        private airportWatchService: AirportWatchService,
    ) {
    }

    ngOnInit() {
        if (localStorage.getItem(this.tableLocalStorageKey)) {
            this.columns = JSON.parse(localStorage.getItem(this.tableLocalStorageKey));
        } else {
            this.columns = initialColumns;
        }

        this.refreshCustomerDataSource();

        this.airportWatchService.getStartDate()
            .subscribe((date) => {
                this.airportWatchStartDate = new Date(date);
            });
    }

    onPageChanged(event: any) {
        localStorage.setItem('pageIndex', event.pageIndex);
        sessionStorage.setItem(
            'pageSizeValue',
            this.paginator.pageSize.toString()
        );
        this.selectAll = false;
        forEach(this.customersData, (customer) => {
            customer.selectAll = false;
        });
    }

    selectAction() {
        const pageCustomersData = this.customersDataSource.connect().value;
        forEach(pageCustomersData, (customer) => {
            customer.selectAll = this.selectAll;
        });
        this.selectedRows = this.selectAll ? pageCustomersData.length : 0;
    }

    selectUnique() {
        if (this.selectedRows === this.customersData.length) {
            this.selectAll = false;
            this.selectedRows = this.selectedRows - 1;
        }
    }

    exportCustomersToExcel() {
        // Export the filtered results to an excel spreadsheet
        const filteredList = this.customersDataSource.filteredData.filter((item) => item.selectAll === true);
        let exportData;
        if (filteredList.length > 0) {
            exportData = filteredList;
        } else {
            exportData = this.customersDataSource.filteredData;
        }
        exportData = map(exportData, (item) => ({
            Company: item.company,
            'FuelerLinx Network': item.isFuelerLinxCustomer ? 'YES' : 'NO',
            'Certificate Type': item.certificateTypeDescription,
        }));
        exportData = sortBy(exportData, [
            (item) => item.Company.toLowerCase(),
        ]);
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'Customers');

        /* save to file */
        XLSX.writeFile(wb, 'Customers.xlsx');
    }

    anySelected() {
        const filteredList = this.customersData.filter((item) => item.selectAll === true);
        return filteredList.length > 0;
    }

    getTableColumns() {
        return this.columns.filter(column => !column.hidden).map(column => column.id);
    }

    openSettings() {
        const dialogRef = this.tableSettingsDialog.open(TableSettingsComponent, {
            data: this.columns
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.columns = [ ...result ];
                this.refreshSort();
                this.saveSettings();
            }
        });
    }

    saveSettings() {
        localStorage.setItem(this.tableLocalStorageKey, JSON.stringify(this.columns));
    }

    private refreshCustomerDataSource() {
        this.sort.sortChange.subscribe(() => {
            this.columns = this.columns.map(column =>
                column.id === this.sort.active
                    ? { ...column, sort: this.sort.direction }
                    : { id: column.id, name: column.name, hidden: column.hidden }
            );
            this.paginator.pageIndex = 0;
            this.saveSettings();
        });
        if (!this.customersDataSource) {
            this.customersDataSource = new MatTableDataSource();
        }
        this.customersDataSource.data = this.customersData;
        this.sort.active = 'company';
        this.customersDataSource.sort = this.sort;
        this.customersDataSource.paginator = this.paginator;
    }

    private refreshSort() {
        const sortedColumn = this.columns.find(column => !column.hidden && column.sort);
        this.sort.sort({ id: null, start: sortedColumn?.sort || 'asc', disableClear: false });
        this.sort.sort({ id: sortedColumn?.id, start: sortedColumn?.sort || 'asc', disableClear: false });
        (this.sort.sortables.get(sortedColumn?.id) as MatSortHeader)?._setAnimationTransitionState({ toState: 'active' });
    }
}
