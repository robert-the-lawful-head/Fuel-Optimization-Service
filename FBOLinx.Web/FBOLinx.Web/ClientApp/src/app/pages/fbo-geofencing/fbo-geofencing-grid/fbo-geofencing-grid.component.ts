import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSort, MatSortHeader, SortDirection } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';


// Services
import { SharedService } from '../../layouts/shared-service';
import {
    ColumnType,
    TableSettingsComponent,
} from '../../shared/components/table-settings/table-settings.component';

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
        id: 'needsAttention',
        name: 'Needs Attention',
    },
    {
        id: 'isNotFbo',
        name: 'Is Not FBO',
    },
];

@Component({
    selector: 'app-fbo-geofencing',
    styleUrls: ['./fbo-geofencing.component.scss'],
    templateUrl: './fbo-geofencing.component.html',
})
export class GeofencingComponent implements OnInit {
    @Input() customersData: any[];

    geofencingDataSource: any = null;
    pageIndex = 0;
    pageSize = 100;
    columns: ColumnType[] = [];

    constructor(private router: Router, private sharedService: SharedService) {
       

    }

    ngOnInit() {
        this.refreshGeofencingDataSource();
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

    private refreshGeofencingDataSource() {
        this.customersDataSource = new MatTableDataSource();
        this.customersDataSource.data = this.customersData.filter(
            (element: any) => {
                if (this.customerFilterType != 1) {
                    return true;
                }
                return element.needsAttention;

            }
        );

        this.customersDataSource.sort = this.sort;
        this.customersDataSource.paginator = this.paginator;
    }
}
