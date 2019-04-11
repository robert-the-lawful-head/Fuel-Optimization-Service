import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import * as moment from 'moment';

@Component({
    selector: 'app-statistics-orders-by-location',
    templateUrl: './statistics-orders-by-location.component.html',
    styleUrls: ['./statistics-orders-by-location.component.scss']
})
/** statistics-orders-by-location component*/
export class StatisticsOrdersByLocationComponent {
    @Input() options: any;

    //Public Members
    public totalOrders: number;
    public icao: string;

    /** statistics-total-aircraft ctor */
    constructor(private fuelreqsService: FuelreqsService,
        private sharedService: SharedService) {
        if (!this.options)
            this.options = {};
    }

    ngOnInit() {
        let startDate = moment().add(-1, 'M').format('MM/DD/YYYY');
        let endDate = moment().format('MM/DD/YYYY');
        this.fuelreqsService.getOrdersByLocation({ StartDateTime: startDate, EndDateTime: endDate, ICAO: '' }).subscribe((data: any) => {
            this.totalOrders = data.totalOrders;
            this.icao = data.icao;
        });
    }
}
