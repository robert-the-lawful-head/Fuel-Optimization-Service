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
        this.refreshData();
    }

    public refreshData() {
        let startDate = this.sharedService.dashboardSettings.filterStartDate;
        let endDate = this.sharedService.dashboardSettings.filterEndDate;
        this.fuelreqsService.getOrdersByLocation({ StartDateTime: startDate, EndDateTime: endDate, ICAO: '', FboId: this.sharedService.currentUser.fboId }).subscribe((data: any) => {
            this.totalOrders = 0;
            if (data.totalOrdersByMonth) {
                for (let orders of data.totalOrdersByMonth) {
                    this.totalOrders = orders.count;
                }
            }
            this.icao = data.icao;
        });
    }
}
