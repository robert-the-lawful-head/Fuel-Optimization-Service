import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';

import { ServiceOrder } from 'src/app/models/service-order';
import { ServiceOrderItem } from 'src/app/models/service-order-item';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';
import { CustomerInfoByGroup } from 'src/app/models/customer-info-by-group';

@Component({
    selector: 'app-service-orders-item-list',
    templateUrl: './service-orders-item-list.component.html'
})
export class ServiceOrdersItemListComponent implements OnInit {
    @Input() serviceOrder: ServiceOrder;

    public customerInfoByGroup: CustomerInfoByGroup;

    constructor(private router: Router,
        private serviceOrderService: ServiceOrderService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService) {
    }

    ngOnInit() {
        if (this.serviceOrder.serviceOrderItems == null)
            this.loadServiceOrderItems();
        this.loadCustomerInfo();
    }

    public addServiceOrderItemClicked() {
        var newServiceOrderItem: ServiceOrderItem = { oid: 0, serviceName: '', serviceOrderId: this.serviceOrder.oid, quantity: 1, isCompleted: false, completionDateTimeUtc: null };
        this.serviceOrderService.createServiceOrderItem(newServiceOrderItem).subscribe((response: EntityResponseMessage<ServiceOrderItem>) => {
            if (!response.success)
                alert('Error creating service order item: ' + response.message);
            this.serviceOrder.serviceOrderItems.push(response.result);
        });
    }

    public serviceOrderItemEdited(serviceOrderItem: ServiceOrderItem) {
        this.serviceOrderService.updateServiceOrderItem(serviceOrderItem).subscribe((response: EntityResponseMessage<ServiceOrderItem>) => {
            if (!response.success)
                alert('Error updating service order item: ' + response.message);
        });
    }

    private loadServiceOrderItems() {
        this.serviceOrderService.getServiceOrderItems(this.serviceOrder.oid).subscribe((response: EntityResponseMessage<ServiceOrderItem[]>) => {
            if (!response.success)
                alert('Error loading service order items: ' + response.message);
            else
                this.serviceOrder.serviceOrderItems = response.result;
        });
    }

    private loadCustomerInfo() {
        this.customerInfoByGroupService.getCustomerInfoByGroupAndCustomerId(this.serviceOrder.customerId, this.serviceOrder.groupId).subscribe((response: CustomerInfoByGroup) => {
            this.customerInfoByGroup = response;
        });
    }
}
