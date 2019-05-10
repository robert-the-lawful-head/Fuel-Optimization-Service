import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-statistics-total-aircraft',
    templateUrl: './statistics-total-aircraft.component.html',
    styleUrls: ['./statistics-total-aircraft.component.scss']
})

/** statistics-total-aircraft component*/
export class StatisticsTotalAircraftComponent implements OnInit {

    @Input() options: any;

    //Public Members
    public totalAircraft: number;

    /** statistics-total-aircraft ctor */
    constructor(private router: Router,
        private customeraircraftService: CustomeraircraftsService,
        private sharedService: SharedService) {
        if (!this.options)
            this.options = {};
    }

    ngOnInit() {
        this.refreshData();
    }

    public redirectClicked() {
        this.router.navigate(['/default-layout/customers']);
    }

    public refreshData() {
        this.customeraircraftService.getCustomerAircraftsCountByGroupId(this.sharedService.currentUser.groupId).subscribe((data: any) => {
            this.totalAircraft = data;
        });
    }
}
