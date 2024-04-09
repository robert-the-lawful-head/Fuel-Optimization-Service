import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import * as _ from 'lodash';
import * as moment from 'moment';
import { interval, Subscription } from 'rxjs';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { ActivatedRoute } from '@angular/router';
import { ServicesAndFeesService } from '../../../services/servicesandfees.service';

@Component({
    selector: 'app-fuelreqs-home',
    styleUrls: ['./fuelreqs-home.component.scss'],
    templateUrl: './fuelreqs-home.component.html',
})
export class FuelreqsHomeComponent implements OnDestroy, OnInit {
    // Public Members
    public pageTitle = 'Fuel & Service Orders';
    public breadcrumb: any[] = [
        {
            link: '/default-layout',
            title: 'Main',
        },
        {
            link: '/default-layout/fuelreqs',
            title: 'Fuel & Service Orders',
        },
    ];
    public fuelreqsData: any[];
    public filterStartDate: Date;
    public filterEndDate: Date;
    public timer: Subscription;
    public isFuelOrdersShowing: boolean = true;
    public missedOrdersData: any[];
    public resetMissedOrders: boolean = false;
    public servicesAndFees: any[] = [];

    constructor(
        private fuelReqService: FuelreqsService,
        private sharedService: SharedService,
        private route: ActivatedRoute,
        private servicesAndFeesService: ServicesAndFeesService,
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.filterStartDate = new Date(
            moment().add(-30, 'd').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(
            moment().add(30, 'd').format('MM/DD/YYYY')
        );

        this.startFuelReqDataServe();
    }

    ngOnDestroy() {
        this.stopFuelReqDataServe();
    }

    async ngOnInit() {
        var servicesAndFees = await this.servicesAndFeesService.getFboServicesAndFees(this.sharedService.currentUser.fboId).toPromise();
        servicesAndFees.forEach((service) => {
            service.servicesAndFees.forEach((serviceAndFee) => {
                if (serviceAndFee.isActive)
                    this.servicesAndFees.push(serviceAndFee);
            });
        });

        this.servicesAndFees.sort((a, b) => a.service.localeCompare(b.service));
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
