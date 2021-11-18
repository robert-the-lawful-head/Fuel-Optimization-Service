import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatSort, MatSortHeader, SortDirection } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { forEach } from 'lodash';

// Services
import { SharedService } from '../../../layouts/shared-service';
import {
    ColumnType,
    TableSettingsComponent,
} from '../../../shared/components/table-settings/table-settings.component';

const initialColumns: ColumnType[] = [
    {
        id: 'icao',
        name: 'ICAO',
    },
    {
        id: 'fbo',
        name: 'FBO'
    },
    {
        id: 'isNotFbo',
        name: 'Is Not FBO',
    },
    {
        id: 'needsAttention',
        name: 'Needs Attention',
    },
];

@Component({
    selector: 'app-fbo-geofencing-grid',
    styleUrls: ['./fbo-geofencing-grid.component.scss'],
    templateUrl: './fbo-geofencing-grid.component.html',
})
export class FboGeofencingGridComponent implements OnInit {
    @ViewChild('fboGeofencingTableContainer') table: ElementRef;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @Input() fboGeofencingData: any[];

    tableLocalStorageKey = 'fbo-geofencing-table-settings';

    fboGeofencingDataSource: any = null;
    pageIndex = 0;
    pageSize = 100;
    selectAll = false;
    columns: ColumnType[] = [];

    constructor(private router: Router, private sharedService: SharedService) {
       

    }

    ngOnInit() {
        if (localStorage.getItem(this.tableLocalStorageKey)) {
            this.columns = JSON.parse(
                localStorage.getItem(this.tableLocalStorageKey)
            );
        } else {
            this.columns = initialColumns;
        }

        this.refreshFboGeofencingDataSource();
    }

    onPageChanged(event: any) {
        localStorage.setItem('pageIndex', event.pageIndex);
        sessionStorage.setItem(
            'pageSizeValue',
            this.paginator.pageSize.toString()
        );
        this.selectAll = false;
        forEach(this.fboGeofencingData, (fbo) => {
            fbo.selectAll = false;
        });
    }

    getTableColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => column.id);
    }

    private refreshFboGeofencingDataSource() {
        if (!this.fboGeofencingDataSource) {
            this.fboGeofencingDataSource = new MatTableDataSource();
        }

        this.fboGeofencingDataSource.data = this.fboGeofencingData;
        this.fboGeofencingDataSource.sort = this.sort;
        this.fboGeofencingDataSource.paginator = this.paginator;
    }
}
