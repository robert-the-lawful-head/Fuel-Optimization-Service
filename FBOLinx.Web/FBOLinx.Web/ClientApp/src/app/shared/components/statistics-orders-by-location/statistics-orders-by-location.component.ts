import { Component, Input, OnInit } from '@angular/core';

// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-statistics-orders-by-location',
    templateUrl: './statistics-orders-by-location.component.html',
    styleUrls: ['./statistics-orders-by-location.component.scss'],
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

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {
    }

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        this.fuelreqsService
            .getOrdersByLocation({
                startDateTime: this.startDate,
                endDateTime: this.endDate,
                icao: '',
                fboId: this.sharedService.currentUser.fboId,
            })
            .subscribe((data: any) => {
                this.totalOrders = 0;
                if (data) {
                    if (data.totalOrders) {
                        this.totalOrders = data.totalOrders;
                    }
                    if (data.icao) {
                        this.icao = data.icao;
                    }
                }
            }, (error: any) => {
            });
    }
}
