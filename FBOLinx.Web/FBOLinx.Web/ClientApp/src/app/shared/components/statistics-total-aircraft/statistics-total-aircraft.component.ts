import { Component, Input, OnInit } from '@angular/core';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';

@Component({
    selector: 'app-statistics-total-aircraft',
    styleUrls: ['./statistics-total-aircraft.component.scss'],
    templateUrl: './statistics-total-aircraft.component.html',
})

// statistics-total-aircraft component
export class StatisticsTotalAircraftComponent implements OnInit {
    @Input() options: any = {
        useCard: true,
    };
    @Input() startDate: any;
    @Input() endDate: any;

    // Public Members
    public totalAircraft: number;

    constructor(
        private customeraircraftService: CustomeraircraftsService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        this.customeraircraftService
            .getCustomerAircraftsCountByGroupId(
                this.sharedService.currentUser.groupId
            )
            .subscribe(
                (data: any) => {
                    this.totalAircraft = data;
                },
                () => {}
            );
    }
}
