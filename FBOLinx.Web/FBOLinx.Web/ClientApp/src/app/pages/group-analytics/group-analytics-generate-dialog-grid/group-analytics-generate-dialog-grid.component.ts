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
        if (this.groupDistributionDataSource.filter && this.groupDistributionDataSource.filter != "") {
            forEach(this.groupDistributionDataSource.filteredData, (customer) => {
                customer.checked = this.selectAll;
            });

            this.selectAllClicked.emit(this.groupDistributionDataSource.filteredData);
        }
        else {
            forEach(this.groupDistributionDataSource.data, (customer) => {
                customer.checked = this.selectAll;
            });

            this.selectAllClicked.emit(this.groupDistributionDataSource.data);
        }
    }

    public customerSelected(customer) {
        if (customer.checked == null)
            customer.checked = true;
        else
            customer.checked = !customer.checked;
        this.customerSelectedClicked.emit(customer);
    }

    applyFilter(filter: string) {
        this.groupDistributionDataSource.filter = filter.trim().toLowerCase();
    }

    private refreshGroupDistributionDataSource() {
        if (!this.groupDistributionDataSource) {
            this.groupDistributionDataSource = new MatTableDataSource();
        }

        this.groupDistributionDataSource.data = this.groupDistributionGridData;
    }
}
