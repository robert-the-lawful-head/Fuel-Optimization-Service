import { Component, OnInit } from '@angular/core';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
@Component({
    selector: 'app-group-customers-home',
    styleUrls: ['./group-customers-home.component.scss'],
    templateUrl: './group-customers-home.component.html',
})
export class GroupCustomersHomeComponent implements OnInit {
    // Public Members
    pageTitle = 'Customers';
    customersData: any[];
    chartName = 'Customers';

    constructor(
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.loadCustomers();
    }

    ngOnInit(): void {}

    // Private Methods
    private loadCustomers() {
        this.ngxLoader.startLoader(this.chartName);

        this.customersData = null;
        this.customerInfoByGroupService
            .getCustomersViewModelByGroup(this.sharedService.currentUser.groupId)
            .subscribe((data: any) => {
                this.customersData = data;
                this.ngxLoader.stopLoader(this.chartName);

            });
    }
}
