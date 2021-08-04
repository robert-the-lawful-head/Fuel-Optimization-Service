import { Component, Input, OnInit } from '@angular/core';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';

@Component({
    selector: 'app-statistics-total-customers',
    styleUrls: ['./statistics-total-customers.component.scss'],
    templateUrl: './statistics-total-customers.component.html',
})
// statisticsTotalCustomers component
export class StatisticsTotalCustomersComponent implements OnInit {
    @Input() options: any = {
        useCard: true,
    };
    @Input() startDate: any;
    @Input() endDate: any;

    // Public Members
    public totalCustomers: number;

    constructor(
        private customerinfobygroupService: CustomerinfobygroupService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        this.customerinfobygroupService
            .getCustomerCountByGroupAndFBO(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe(
                (data: any) => {
                    this.totalCustomers = data;
                },
                () => {}
            );
    }
}
