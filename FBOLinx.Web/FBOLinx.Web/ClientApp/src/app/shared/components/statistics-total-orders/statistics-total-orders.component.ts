import { Component, Input, OnInit } from '@angular/core';
// Components
import * as moment from 'moment';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';

@Component({
    selector: 'app-statistics-total-orders',
    styleUrls: ['./statistics-total-orders.component.scss'],
    templateUrl: './statistics-total-orders.component.html',
})
// statistics-total-orders component
export class StatisticsTotalOrdersComponent implements OnInit {
    @Input() options: any = {
        useCard: true,
    };
    @Input() startDate: any;
    @Input() endDate: any;

    // Public Members
    public totalOrders: number;
    public startDateString: string;
    public dayDifference: number = 30;
    public monthDifference: number = 1;

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {
        if (!this.options) {
            this.options = {};
        }
    }

    ngOnInit() {
        this.dayDifference = Math.abs(moment(this.endDate).diff(this.startDate, 'days'));
        this.monthDifference = Math.abs(moment(this.endDate).diff(this.startDate, 'months'));
        this.refreshData();
    }

    public refreshData() {
        this.startDateString = moment(this.startDate).format('L');
        this.fuelreqsService
            .getForFboCount(
                this.sharedService.currentUser.fboId,
                this.startDate
            )
            .subscribe(
                (data: any) => {
                    this.totalOrders = data;
                },
                () => {}
            );
    }
}
