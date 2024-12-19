import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table'
import { MatTableDataSource } from '@angular/material/table';
import { forEach } from 'lodash';
import {
    ColumnType,
} from '../../../shared/components/table-settings/table-settings.component';

// Services
import { SharedService } from '../../../layouts/shared-service';


const initialColumns: ColumnType[] = [
    {
        id: 'select',
        name: 'Select',
    },
    {
        id: 'customer',
        name: 'Customer'
    }
];

@Component({
    selector: 'app-group-analytics-generate-dialog-grid',
    styleUrls: ['./group-analytics-generate-dialog-grid.component.scss'],
    templateUrl: './group-analytics-generate-dialog-grid.component.html',
})
export class GroupAnalyticsGenerateDialogGridComponent implements OnInit {
    @Input() groupDistributionData: any[];
    @Input() groupDistributionGridData: any[];
    @Output() customerSelectedClicked = new EventEmitter<any>();
    @Output() selectAllClicked = new EventEmitter<any>();

    groupDistributionDataSource: any = null;
    columns: ColumnType[] = [];

    selectAll = false;

    constructor(private router: Router, private sharedService: SharedService) {

    }

    ngOnInit() {
        this.columns = initialColumns;

        this.refreshGroupDistributionDataSource();
    }

    ngOnDestroy() {
        forEach(this.groupDistributionDataSource.data, (customer) => {
            if (customer.checked)
                customer.checked = false;
        });
    }

    getTableColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => column.id);
    }

    selectAction() {
        var customers;

        if (this.groupDistributionDataSource.filter && this.groupDistributionDataSource.filter != "") {
            forEach(this.groupDistributionDataSource.filteredData, (customer) => {
                customer.checked = this.selectAll;
            });

            customers = this.groupDistributionDataSource.filteredData;

            this.selectAllClicked.emit(this.groupDistributionDataSource.filteredData);
        }
        else {
            forEach(this.groupDistributionDataSource.data, (customer) => {
                customer.checked = this.selectAll;
            });

            customers = this.groupDistributionDataSource.data;

            this.selectAllClicked.emit(this.groupDistributionDataSource.data);
        }

        this.updateSelectAll(customers);
    }

    updateSelectAll(customers) {
        const selectedCount = customers.filter(c => c.checked == true).length;
        const totalItems = customers.length;

        if (selectedCount === totalItems) {
            this.selectAll = true;
        } else if (selectedCount > 0) {
            this.selectAll = false;
        } else {
            this.selectAll = false;
        }
    }

    public customerSelectedRow(customer) {
        if (customer.checked == null)
            customer.checked = true;
        else
            customer.checked = !customer.checked;

        this.customerSelected();
    }

    public customerSelected() {
        var customers;

        if (this.groupDistributionDataSource.filter && this.groupDistributionDataSource.filter != "")
            customers = this.groupDistributionDataSource.filteredData;
        else
            customers = this.groupDistributionDataSource.data;

        this.updateSelectAll(customers);

        this.customerSelectedClicked.emit(this.groupDistributionDataSource.data);
    }

    applyFilter(filter: string) {
        this.groupDistributionDataSource.filter = filter.trim().toLowerCase();
    }

    // Function to determine if "Select All" checkbox should be indeterminate
    public isIndeterminate(): boolean {
        var selectedCount;
        var totalItems;
        if (this.groupDistributionDataSource.filter && this.groupDistributionDataSource.filter != "") {
            selectedCount = this.groupDistributionDataSource.filteredData.filter((customer) => customer.checked).length;
            totalItems = this.groupDistributionDataSource.filteredData.length;
        }
        else {
            selectedCount = this.groupDistributionDataSource.data.filter((customer) => customer.checked).length;
            totalItems = this.groupDistributionDataSource.data.length;
        }
        return selectedCount > 0 && selectedCount < totalItems;
    }

    private refreshGroupDistributionDataSource() {
        if (!this.groupDistributionDataSource) {
            this.groupDistributionDataSource = new MatTableDataSource();
        }

        this.groupDistributionDataSource.data = this.groupDistributionGridData;
    }
}
