import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { forEach } from 'lodash';
import {
    ColumnType,
} from '../../../shared/components/table-settings/table-settings.component';
import { NgxUiLoaderService } from 'ngx-ui-loader';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { FbomissedquoteslogService } from '../../../services/fbomissedquoteslog.service';


const initialColumns: ColumnType[] = [
    {
        id: 'customerName',
        name: 'Customer Name',
    },
    {
        id: 'tailNumber',
        name: 'Tail Number'
    },
    {
        id: 'missedOrdersCount',
        name: 'Missed Orders Count',
    },
    {
        id: 'createdDate',
        name: 'Created Date',
    },
];

@Component({
    selector: 'app-fbos-missed-orders-grid',
    styleUrls: ['./fbos-missed-orders-grid.component.scss'],
    templateUrl: './fbos-missed-orders-grid.component.html',
})
export class FbosMissedOrdersGridComponent implements OnInit {
    fbosMissedOrdersDataSource: any = null;
    columns: ColumnType[] = [];
    missedOrdersLoader = 'missed-orders-loader';
    noMissedOrders = false;

    constructor(private router: Router, private sharedService: SharedService, private fboMissedQuotesLogService: FbomissedquoteslogService, private NgxUiLoader: NgxUiLoaderService) {

    }

    ngOnInit() {
        this.columns = initialColumns;
        this.refreshMissedOrdersDataSource();
    }

    getTableColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => column.id);
    }

    private refreshMissedOrdersDataSource() {
        this.NgxUiLoader.startLoader(this.missedOrdersLoader);
        if (!this.fbosMissedOrdersDataSource) {
            this.fbosMissedOrdersDataSource = new MatTableDataSource();
        }

        this.fboMissedQuotesLogService.getRecentMissedOrders(this.sharedService.currentUser.fboId)
            .subscribe(
                (data: any) => {
                    this.fbosMissedOrdersDataSource.data = data;
                    if (this.fbosMissedOrdersDataSource.data.length == 0)
                        this.noMissedOrders = true;
                    this.NgxUiLoader.stopLoader(this.missedOrdersLoader);
                }
            );
    }
}
