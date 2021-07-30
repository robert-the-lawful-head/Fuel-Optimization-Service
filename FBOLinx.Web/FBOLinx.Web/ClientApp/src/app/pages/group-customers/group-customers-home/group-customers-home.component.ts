import { Component, OnInit } from '@angular/core';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/customers',
        title: 'Customers',
    },
];

@Component({
    selector: 'app-group-customers-home',
    styleUrls: [ './group-customers-home.component.scss' ],
    templateUrl: './group-customers-home.component.html',
})
export class GroupCustomersHomeComponent implements OnInit {
    // Public Members
    pageTitle = 'Customers';
    breadcrumb: any[] = BREADCRUMBS;
    customersData: any[];

    constructor(
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService,
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.loadCustomers();
    }

    ngOnInit(): void {
    }

    // Private Methods
    private loadCustomers() {
        this.customersData = null;
        this.customerInfoByGroupService
            .getByGroup(this.sharedService.currentUser.groupId)
            .subscribe((data: any) => {
                this.customersData = data;
            });
    }
}
