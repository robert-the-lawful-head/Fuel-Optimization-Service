import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';

import { ServiceOrder } from 'src/app/models/service-order';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';

import * as moment from 'moment';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/fuelreqs',
        title: 'Fuel Orders',
    },
];

@Component({
    selector: 'app-service-orders-home',
    templateUrl: './service-orders-home.component.html'
})
export class ServiceOrdersHomeComponent implements OnInit {
    public serviceOrdersData: Array<ServiceOrder>;
    public currentOrdersData: Array<ServiceOrder>;
    public pastOrdersData: Array<ServiceOrder>;
    public breadcrumb: any[] = BREADCRUMBS;

    constructor(private router: Router,
        private serviceOrderService: ServiceOrderService,
        private sharedService: SharedService) {
    }

    ngOnInit() {
        
    }

}
