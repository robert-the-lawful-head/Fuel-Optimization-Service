import { Component, Input, OnInit } from '@angular/core';

// Services
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-statistics-total-aircraft',
    templateUrl: './statistics-total-aircraft.component.html',
    styleUrls: ['./statistics-total-aircraft.component.scss'],
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
            .subscribe((data: any) => {
                this.totalAircraft = data;
            }, () => {
            });
    }
}
