import { Component, Input, OnInit } from '@angular/core';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';

import * as moment from 'moment';

@Component({
    selector: 'app-statistics-orders-by-location',
    styleUrls: ['./statistics-orders-by-location.component.scss'],
    templateUrl: './statistics-orders-by-location.component.html',
})
export class StatisticsOrdersByLocationComponent implements OnInit {
    @Input() options: any = {
        useCard: true,
    };
    @Input() startDate: any;
    @Input() endDate: any;

    // Public Members
    public totalOrders: number;
    public icao: string;
    public dayDifference: number = 30;
    public monthDifference: number = 1;

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        this.dayDifference = Math.abs(moment(this.endDate).diff(this.startDate, 'days'));
        this.monthDifference = Math.abs(moment(this.endDate).diff(this.startDate, 'months'));
        this.refreshData();
    }

    public refreshData() {
        this.fuelreqsService
            .getOrdersByLocation({
                endDateTime: this.endDate,
                fboId: this.sharedService.currentUser.fboId,
                icao: '',
                startDateTime: this.startDate,
            })
            .subscribe(
                (data: any) => {
                    this.totalOrders = 0;
                    if (data) {
                        if (data.totalOrders) {
                            this.totalOrders = data.totalOrders;
                        }
                        if (data.icao) {
                            this.icao = data.icao;
                        }
                    }
                },
                (error: any) => {}
            );
    }
}
