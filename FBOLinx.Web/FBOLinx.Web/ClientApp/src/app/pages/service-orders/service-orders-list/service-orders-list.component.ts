import { Component, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';

import { ServiceOrdersDialogNewComponent } from '../service-orders-dialog-new/service-orders-dialog-new.component';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

import { ServiceOrder } from 'src/app/models/service-order';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';

@Component({
    selector: 'app-service-orders-list',
    templateUrl: './service-orders-list.component.html'
})
export class ServiceOrdersListComponent implements OnInit {
    @Input() serviceOrdersData: Array<ServiceOrder>;
    @Input() allowAddingNew: boolean = true;

    public selectedServiceOrder: ServiceOrder;
    public inCompleteServiceOrders: Array<ServiceOrder>;
    public completeServiceOrders: Array<ServiceOrder>;
    public globalFilter: string = '';

    constructor(private router: Router,
        private serviceOrderService: ServiceOrderService,
        private sharedService: SharedService,
        private newServiceOrderDialog: MatDialog,
        private deleteServiceOrderDialog: MatDialog) {
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
            isCompleted: false,
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
        this.selectedServiceOrder = serviceOrder;
    }

    public serviceOrderItemsChanged(serviceOrder: ServiceOrder) {
        this.calculateCompletions(serviceOrder);
        this.arrangeServiceOrders();
        this.saveOrder(serviceOrder);
    }

    public deleteServiceOrderClicked(serviceOrder: ServiceOrder) {
        const dialogRef = this.deleteServiceOrderDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'service order', item: serviceOrder },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.deleteOrder(serviceOrder);
        });        
    }

    public serviceOrderToggleChanged(event: any) {
        var serviceOrder: ServiceOrder = event.option._value;
        for (const item of serviceOrder.serviceOrderItems) {
            item.isCompleted = event.option._selected;
        }
        this.calculateCompletions(serviceOrder);
        this.arrangeServiceOrders();
        this.saveOrder(serviceOrder);
    };

    public filterChanged(filterValue: string) {
        this.globalFilter = filterValue;
        this.arrangeServiceOrders();
    }

    private calculateCompletions(serviceOrder: ServiceOrder) {        
        serviceOrder.numberOfCompletedItems = serviceOrder.serviceOrderItems.filter(x => x.isCompleted).length;
        serviceOrder.isCompleted = serviceOrder.numberOfCompletedItems == serviceOrder.serviceOrderItems.length;
    }

    private arrangeServiceOrders() {
        var filter = this.globalFilter.toUpperCase();
        this.inCompleteServiceOrders = this.serviceOrdersData.filter(x =>
            (x.serviceOrderItems == null || x.serviceOrderItems.length == 0 || x.serviceOrderItems.filter(item => item.isCompleted).length != x.serviceOrderItems.length)
            && (filter == '' || x.customerAircraft?.tailNumber?.toUpperCase().indexOf(filter) > -1 || x.customerInfoByGroup?.company?.toUpperCase().indexOf(filter) > -1)
        );

        this.completeServiceOrders = this.serviceOrdersData.filter(x =>
            (x.serviceOrderItems == null || x.serviceOrderItems.length > 0 && x.serviceOrderItems.filter(item => item.isCompleted).length == x.serviceOrderItems.length)
            && (filter == '' || x.customerAircraft?.tailNumber?.toUpperCase().indexOf(filter) > -1 || x.customerInfoByGroup?.company?.toUpperCase().indexOf(filter) > -1)
        );
    }

    private saveOrder(serviceOrder: ServiceOrder) {
        this.serviceOrderService.updateServiceOrder(serviceOrder).subscribe((response: EntityResponseMessage<ServiceOrder>) => {
            if (!response.success)
                alert('Error saving service order: ' + response.message);
        });
    }

    private deleteOrder(serviceOrder: ServiceOrder) {
        this.serviceOrderService.deleteServiceOrder(serviceOrder.oid).subscribe((response: EntityResponseMessage<ServiceOrder>) => {
            if (!response.success)
                alert('Error deleting service order: ' + response.message);
            else {
                this.serviceOrdersData.splice(this.serviceOrdersData.indexOf(serviceOrder), 1);
                this.arrangeServiceOrders();
            }
        });
    }
}
