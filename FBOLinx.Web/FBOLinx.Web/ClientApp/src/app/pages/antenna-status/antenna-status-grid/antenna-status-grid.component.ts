import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { forEach } from 'lodash';
import {
    ColumnType,
} from '../../../shared/components/table-settings/table-settings.component';

// Services
import { SharedService } from '../../../layouts/shared-service';


const initialColumns: ColumnType[] = [
    {
        id: 'boxName',
        name: 'Antenna Name',
    },
    {
        id: 'status',
        name: 'Status'
    },
    {
        id: 'lastUpdateRaw',
        name: 'Last Update Raw',
    },
    {
        id: 'lastUpdateCurated',
        name: 'Last Update Curated',
    },
    {
        id: 'fbolinxAccount',
        name: 'FBOlinx Account',
    }
];

@Component({
    selector: 'app-antenna-status-grid',
    styleUrls: ['./antenna-status-grid.component.scss'],
    templateUrl: './antenna-status-grid.component.html',
})
export class AntennaStatusGridComponent implements OnInit {
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @Input() antennaStatusData: any[];
    @Input() antennaStatusGridData: any[];

    tableLocalStorageKey = 'antenna-status-table-settings';

    antennaStatusDataSource: any = null;
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

        this.refreshAntennaStatusDataSource();
    }

    getTableColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => column.id);
    }

    private refreshAntennaStatusDataSource() {
        if (!this.antennaStatusDataSource) {
            this.antennaStatusDataSource = new MatTableDataSource();
        }

        this.antennaStatusDataSource.data = this.antennaStatusGridData;
        this.antennaStatusDataSource.sort = this.sort;
    }
}
