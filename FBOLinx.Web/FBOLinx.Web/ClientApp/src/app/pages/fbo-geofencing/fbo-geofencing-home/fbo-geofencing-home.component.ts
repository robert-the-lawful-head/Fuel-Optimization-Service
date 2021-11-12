import { Component, OnDestroy, OnInit } from '@angular/core';
import * as _ from 'lodash';
import * as moment from 'moment';
import { interval, Subscription } from 'rxjs';
import * as XLSX from 'xlsx';

import { SharedService } from '../../../layouts/shared-service';
// Services

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/fuelreqs',
        title: 'Fuel Orders',
    },
];

@Component({
    selector: 'app-fbo-geofencing-home',
    styleUrls: ['./fbo-geofencing-home.component.scss'],
    templateUrl: './fbo-geofencing-home.component.html',
})
export class FuelreqsHomeComponent implements OnDestroy, OnInit {
    // Public Members
    public pageTitle = 'FBO Geofencing';
    public breadcrumb: any[] = BREADCRUMBS;
    public fboGeofencingData: any[];

    constructor(
        private sharedService: SharedService
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }


    ngOnInit() {
        this.loadFbogeofencing();
    }

    // PRIVATE METHODS
    private loadFbogeofencing() {
        this.fuelReqService
            .getForGroupFboAndDateRange(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.filterStartDate,
                this.filterEndDate
            )
            .subscribe((data: any) => {
                this.fuelreqsData = data;
            });
    }
}
