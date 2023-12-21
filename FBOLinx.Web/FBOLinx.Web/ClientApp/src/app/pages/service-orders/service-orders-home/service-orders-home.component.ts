import { Component, OnInit } from '@angular/core';
import { ServiceOrder } from 'src/app/models/service-order';

@Component({
    selector: 'app-service-orders-home',
    templateUrl: './service-orders-home.component.html'
})
export class ServiceOrdersHomeComponent implements OnInit {
    public serviceOrdersData: Array<ServiceOrder>;
    public currentOrdersData: Array<ServiceOrder>;
    public pastOrdersData: Array<ServiceOrder>;
    public breadcrumb: any[] = [
        {
            link: '/default-layout',
            title: 'Main',
        },
        {
            link: '/default-layout/fuelreqs',
            title: 'Fuel Orders',
        },
    ];

    constructor() {
    }

    ngOnInit() {

    }

}
