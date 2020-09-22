import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';

// Components
import * as moment from 'moment';

@Component({
    selector: 'app-statistics-total-orders',
    templateUrl: './statistics-total-orders.component.html',
    styleUrls: ['./statistics-total-orders.component.scss'],
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

    constructor(
        private router: Router,
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {
        if (!this.options) {
            this.options = {};
        }
    }

    ngOnInit() {
        this.refreshData();
    }

    public redirectClicked() {
        this.router.navigate(['/default-layout/fuelreqs']);
    }

    public refreshData() {
        this.startDateString = moment(this.startDate).format('L');
        this.fuelreqsService
            .getForFboCount(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.totalOrders = data;
            }, () => {
            });
    }
}
