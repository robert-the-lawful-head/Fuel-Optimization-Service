import { Component, OnInit } from '@angular/core';

// Services
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { SharedService } from '../../../layouts/shared-service';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '/default-layout',
    },
    {
        title: 'Customers',
        link: '/default-layout/customers',
    },
];

@Component({
    selector: 'app-group-customers-home',
    templateUrl: './group-customers-home.component.html',
    styleUrls: [ './group-customers-home.component.scss' ],
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
