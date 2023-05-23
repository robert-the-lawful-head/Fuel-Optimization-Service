import { Component, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';

import { ServiceOrdersDialogNewComponent } from '../service-orders-dialog-new/service-orders-dialog-new.component';

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
        private sharedService: SharedService,
        private newServiceOrderDialog: MatDialog) {
    }

    ngOnInit() {
        this.arrangeServiceOrders();
    }

    public addServiceOrderClicked() {
        var localServiceDate = new Date();
        var newServiceOrder: ServiceOrder = {
            oid: 0,
            fboId: this.sharedService.currentUser.fboId,
            serviceOrderItems: [],
            serviceDateTimeUtc: new Date(Date.UTC(localServiceDate.getUTCFullYear(), localServiceDate.getUTCMonth(),
                localServiceDate.getUTCDate(), localServiceDate.getUTCHours(), localServiceDate.getUTCMinutes(), localServiceDate.getUTCSeconds())),
            serviceDateTimeLocal: localServiceDate,
            groupId: this.sharedService.currentUser.groupId,
            customerInfoByGroupId: 0,
            customerAircraftId: 0,
            associatedFuelOrderId: 0,
            numberOfCompletedItems: 0,
            customerInfoByGroup: null,
            customerAircraft: null
        };
        const config: MatDialogConfig = {
            disableClose: true,
            data: newServiceOrder
        };

        const dialogRef = this.newServiceOrderDialog.open(ServiceOrdersDialogNewComponent, config);

        dialogRef.afterClosed().subscribe((result: ServiceOrder) => {
            if (!result)
                return;
            this.serviceOrdersData.push(result);
            this.arrangeServiceOrders();
        });
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
