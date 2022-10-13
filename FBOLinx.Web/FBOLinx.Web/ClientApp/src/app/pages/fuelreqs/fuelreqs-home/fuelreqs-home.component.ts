import { Component, OnDestroy, OnInit } from '@angular/core';
import * as _ from 'lodash';
import * as moment from 'moment';
import { interval, Subscription } from 'rxjs';
import * as XLSX from 'xlsx';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { ActivatedRoute } from '@angular/router';

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

    public export(event) {
        this.fuelReqService
            .getForGroupFboAndDateRange(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                event.filterStartDate,
                event.filterEndDate
            )
            .subscribe((data: any) => {
                const exportData = _.map(data, (item) => ({
                    ETA: item.eta,
                    ETD: item.etd,
                    Email: item.email,
                    'Flight Dept.': item.customerName,
                    'Fuelerlinx ID': item.sourceId,
                    ID: item.oid,
                    'ITP Margin Template': item.pricingTemplateName,
                    PPG: item.quotedPpg,
                    Phone: item.phoneNumber,
                    Source: item.source,
                    'Tail #': item.tailNumber,
                    'Transaction Status': item.cancelled ? 'Cancelled' : 'Live',
                    'Volume (gal.)': item.quotedVolume,
                }));
                const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
                const wb: XLSX.WorkBook = XLSX.utils.book_new();
                XLSX.utils.book_append_sheet(wb, ws, 'Fuel Orders');

                /* save to file */
                XLSX.writeFile(wb, 'FuelOrders.xlsx');
            });
    }

    onTabClick(event) {
        if (event.tab.textLabel == "Fuel Orders")
            this.isFuelOrdersShowing = true;
        else
            this.isFuelOrdersShowing = false;
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
