import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import * as moment from 'moment';

@Component({
    selector: 'app-statistics-total-orders',
    templateUrl: './statistics-total-orders.component.html',
    styleUrls: ['./statistics-total-orders.component.scss']
})
/** statistics-total-orders component*/
export class StatisticsTotalOrdersComponent implements OnInit {

    @Input() options: any;

    //Public Members
    public totalOrders: number;

    /** statistics-total-orders ctor */
    constructor(private fuelreqsService: FuelreqsService,
        private sharedService: SharedService) {
        if (!this.options)
            this.options = {};
    }

    ngOnInit() {
        let startDate = moment().add(-6, 'M').format('MM/DD/YYYY');
        let endDate = moment().format('MM/DD/YYYY');
        this.fuelreqsService.getForFboCount(this.sharedService.currentUser.fboId).subscribe((data: any) => {
            this.totalOrders = data;
        });
    }
}
