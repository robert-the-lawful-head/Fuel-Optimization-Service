import { Component, Input, OnInit } from '@angular/core';

// Services
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-statistics-total-customers',
    templateUrl: './statistics-total-customers.component.html',
    styleUrls: ['./statistics-total-customers.component.scss'],
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
            .subscribe((data: any) => {
                this.totalCustomers = data;
            }, () => {
            });
    }
}
