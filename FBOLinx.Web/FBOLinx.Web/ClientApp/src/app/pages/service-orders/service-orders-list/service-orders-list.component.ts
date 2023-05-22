import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';

import { ServiceOrder } from 'src/app/models/service-order';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';

@Component({
    selector: 'app-service-orders-list',
    templateUrl: './service-orders-list.component.html'
})
export class ServiceOrdersListComponent implements OnInit {
    @Input() serviceOrdersData: Array<ServiceOrder>;

    public selectedServiceOrder: ServiceOrder;
    public inCompleteServiceOrders: Array<ServiceOrder>;
    public completeServiceOrders: Array<ServiceOrder>;

    constructor(private router: Router,
        private serviceOrderService: ServiceOrderService,
        private sharedService: SharedService) {
    }

    ngOnInit() {
        
    }

    public serviceOrderClicked(serviceOrder: ServiceOrder) {
        if (this.selectedServiceOrder == serviceOrder)
            this.selectedServiceOrder = null;
        else
            this.selectedServiceOrder = serviceOrder;
    }

    public serviceOrderItemsChanged() {
        this.arrangeServiceOrders();
    }

    public deleteServiceOrderClicked(serviceOrder: ServiceOrder) {
        this.serviceOrderService.deleteServiceOrder(serviceOrder.oid).subscribe((response: EntityResponseMessage<ServiceOrder>) => {
            if (!response.success)
                alert('Error deleting service order: ' + response.message);
            else {
                this.serviceOrdersData.splice(this.serviceOrdersData.indexOf(serviceOrder), 1);
                this.arrangeServiceOrders();
            }
        });
    }

    private arrangeServiceOrders() {
        this.inCompleteServiceOrders = this.serviceOrdersData.filter(x =>
            (x.serviceOrderItems.length == 0 || x.serviceOrderItems.filter(item => item.isCompleted).length != x.serviceOrderItems.length)
        );
        this.completeServiceOrders = this.serviceOrdersData.filter(x =>
            (x.serviceOrderItems.length > 0 && x.serviceOrderItems.filter(item => item.isCompleted).length == x.serviceOrderItems.length)
        );
    }

}
