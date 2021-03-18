import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { CustomersListType } from '../../../models/customer';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '/default-layout',
    },
    {
        title: 'Analytics',
        link: '/default-layout/analytics',
    },
];

@Component({
    selector: 'app-analytics-home',
    templateUrl: './analytics-home.component.html',
    styleUrls: [ './analytics-home.component.scss' ],
})
export class AnalyticsHomeComponent implements OnInit {
    public pageTitle = 'Analytics';
    public breadcrumb: any[] = BREADCRUMBS;
    public filterStartDate: Date;
    public filterEndDate: Date;
    public pastThirtyDaysStartDate: Date;
    public customers: CustomersListType[] = [];
    public tailNumbers: any[] = [];

    constructor(
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService
    ) {
        this.filterStartDate = new Date(moment().add(-12, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.pastThirtyDaysStartDate = new Date(moment().add(-30, 'days').format('MM/DD/YYYY'));
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {
        this.getCustomersList();
        this.getAircrafts();
    }

    getCustomersList() {
        this.customerInfoByGroupService.getCustomersListByGroupAndFbo(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
        ).subscribe((customers: any[]) => {
            this.customers = customers;
        });
    }

    getAircrafts() {
        this.customerAircraftsService.getAircraftsListByGroupAndFbo(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
        ).subscribe((tailNumbers: any[]) => {
            this.tailNumbers = tailNumbers;
        });
    }
}
