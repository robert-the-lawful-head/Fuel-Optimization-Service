import { Component, OnDestroy, OnInit } from '@angular/core';
import * as _ from 'lodash';
import * as moment from 'moment';
import { interval, Subscription } from 'rxjs';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { ServicesAndFeesService } from '../../../services/servicesandfees.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
    selector: 'app-fuelreqs-home',
    styleUrls: ['./fuelreqs-home.component.scss'],
    templateUrl: './fuelreqs-home.component.html',
})
export class FuelreqsHomeComponent implements OnDestroy, OnInit {
    // Public Members
    public pageTitle = 'Fuel & Service Orders';
    public chartName = "fuelreqs";

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
        private servicesAndFeesService: ServicesAndFeesService,
        private ngxLoader: NgxUiLoaderService
    ) {
        
        this.filterStartDate = new Date(
            moment().add(-30, 'd').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(
            moment().add(30, 'd').format('MM/DD/YYYY')
        );
    }

    ngOnDestroy() {
        this.stopFuelReqDataServe();
    }

    async ngOnInit() {
        await this.servicesAndFeesService.getFboServicesAndFees(this.sharedService.currentUser.fboId)
        .subscribe((data: any) => {
            data.forEach((service) => {
                service.servicesAndFees.forEach((serviceAndFee) => {
                    if (serviceAndFee.isActive)
                        this.servicesAndFees.push(serviceAndFee);
                });
            });
        });
        this.servicesAndFees.sort((a, b) => a.service.localeCompare(b.service));

        this.ngxLoader.startLoader(this.chartName);
        await this.loadFuelReqs();
        this.ngxLoader.stopLoader(this.chartName);
        this.startFuelReqDataServe();
    }

    public startFuelReqDataServe() {
        this.timer = interval(60000).subscribe(() => {
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

    public async dateFilterChanged(event): Promise<void>{
        this.filterStartDate = event.filterStartDate;
        this.filterEndDate = event.filterEndDate;
        this.restartFuelReqDataServe();
        this.fuelreqsData = null;
        this.ngxLoader.startLoader(this.chartName);
        await this.loadFuelReqs();
        this.ngxLoader.stopLoader(this.chartName);
    }

    // PRIVATE METHODS
    private async loadFuelReqs(): Promise<void>{
        let data = await this.fuelReqService
        .getForGroupFboAndDateRange(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
            this.filterStartDate,
            this.filterEndDate
        ).toPromise();
        this.fuelreqsData = data as any;
    }
}
