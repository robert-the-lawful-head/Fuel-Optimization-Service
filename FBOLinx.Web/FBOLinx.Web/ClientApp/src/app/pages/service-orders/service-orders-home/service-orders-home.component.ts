import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';

import { ServiceOrder } from 'src/app/models/service-order';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';

@Component({
    selector: 'app-service-orders-home',
    templateUrl: './service-orders-home.component.html'
})
export class ServiceOrdersHomeComponent implements OnInit {

    public breadcrumb: any[];
    public serviceOrdersData: Array<ServiceOrder>;

    constructor(private router: Router,
        private serviceOrderService: ServiceOrderService,
        private sharedService: SharedService) {
    }

    ngOnInit() {
        this.loadServiceOrders();        
    }    

    private loadServiceOrders() {
        this.serviceOrderService.getServiceOrdersForFbo(this.sharedService.currentUser.fboId).subscribe((response: EntityResponseMessage<ServiceOrder[]>) => {
            if (!response.success)
                alert('Error loading service orders: ' + response.message);
            else
                this.serviceOrdersData = response.result;
        });
    }
}
