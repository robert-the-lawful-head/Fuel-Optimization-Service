import { Component, OnDestroy, OnInit } from '@angular/core';
import * as _ from 'lodash';
import * as moment from 'moment';
import { interval, Subscription } from 'rxjs';
import * as XLSX from 'xlsx';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { ActivatedRoute } from '@angular/router';
import * as SharedEvent from '../../../constants/sharedEvents';

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
    selector: 'app-fuelreqs-home',
    styleUrls: ['./fuelreqs-home.component.scss'],
    templateUrl: './fuelreqs-home.component.html',
})
export class FuelreqsHomeComponent implements OnDestroy, OnInit {
    // Public Members
    public pageTitle = 'Fuel Orders';
    public breadcrumb: any[] = BREADCRUMBS;
    public fuelreqsData: any[];
    public filterStartDate: Date;
    public filterEndDate: Date;
    public timer: Subscription;
    public isFuelOrdersShowing: boolean = true;
    public missedOrdersData: any[];
    public selectedTabIndex: number = 0;
    public resetMissedOrders: boolean = false;

    constructor(
        private fuelReqService: FuelreqsService,
        private sharedService: SharedService,
        private route: ActivatedRoute
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.filterStartDate = new Date(
            moment().add(-30, 'd').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(
            moment().add(30, 'd').format('MM/DD/YYYY')
        );

        this.route.queryParams.subscribe((params) => {
            if (params.tab && params.tab) {
                this.selectedTabIndex = parseInt(params.tab);
            }
        });

        this.startFuelReqDataServe();
    }

    ngOnDestroy() {
        this.stopFuelReqDataServe();
    }

    ngOnInit() {
        this.loadFuelReqs();
    }

    public startFuelReqDataServe() {
        this.timer = interval(30000).subscribe(() => {
            this.loadFuelReqs();
        });
    }

    public restartFuelReqDataServe() {
        this.stopFuelReqDataServe();
        this.startFuelReqDataServe();
    }

    public stopFuelReqDataServe() {
        if (this.timer) {
            this.timer.unsubscribe();
        }
    }

    public dateFilterChanged(event) {
        this.filterStartDate = event.filterStartDate;
        this.filterEndDate = event.filterEndDate;
        this.restartFuelReqDataServe();
        this.fuelreqsData = null;
        this.loadFuelReqs();
    }

    onTabClick(event) {
        if (event.tab.textLabel == "Fuel Orders") {
            this.isFuelOrdersShowing = true;
            this.sharedService.emitChange(SharedEvent.resetMissedOrders);
        }
        else {
            this.filterStartDate = new Date(
                moment().add(-30, 'd').format('MM/DD/YYYY')
            );
            this.filterEndDate = new Date(
                moment().add(30, 'd').format('MM/DD/YYYY')
            );

            this.loadFuelReqs();
            this.isFuelOrdersShowing = false;
        }
    }

    // PRIVATE METHODS
    private loadFuelReqs() {
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
