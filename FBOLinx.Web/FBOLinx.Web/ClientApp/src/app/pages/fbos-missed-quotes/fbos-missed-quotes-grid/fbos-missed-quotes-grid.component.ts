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
        id: 'missedQuotesCount',
        name: 'Missed Quotes Count',
    },
    {
        id: 'createdDate',
        name: 'Created Date',
    },
];

@Component({
    selector: 'app-fbos-missed-quotes-grid',
    styleUrls: ['./fbos-missed-quotes-grid.component.scss'],
    templateUrl: './fbos-missed-quotes-grid.component.html',
})
export class FbosMissedQuotesGridComponent implements OnInit {
    fbosMissedQuotesDataSource: any = null;
    columns: ColumnType[] = [];
    missedQuotesLoader = 'missed-quotes-loader';
    noMissedQuotes = false;

    constructor(private router: Router, private sharedService: SharedService, private fboMissedQuotesLogService: FbomissedquoteslogService, private NgxUiLoader: NgxUiLoaderService) {

    }

    ngOnInit() {
        this.columns = initialColumns;
        this.refreshMissedQuotesDataSource();
    }

    getTableColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => column.id);
    }

    private refreshMissedQuotesDataSource() {
        this.NgxUiLoader.startLoader(this.missedQuotesLoader);
        if (!this.fbosMissedQuotesDataSource) {
            this.fbosMissedQuotesDataSource = new MatTableDataSource();
        }

        this.fboMissedQuotesLogService.getRecentMissedQuotes(this.sharedService.currentUser.fboId)
            .subscribe(
                (data: any) => {
                    this.fbosMissedQuotesDataSource.data = data;
                    if (this.fbosMissedQuotesDataSource.data.length == 0)
                        this.noMissedQuotes = true;
                    this.NgxUiLoader.stopLoader(this.missedQuotesLoader);
                }
        );
    }
}
