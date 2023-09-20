import { Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';
import { AircraftsService } from '../../../services/aircrafts.service';

import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { CustomerAircraftsEditComponent } from '../../customer-aircrafts/customer-aircrafts-edit/customer-aircrafts-edit.component';
import { CustomersEditDialogComponent } from '../../customers/customers-edit-dialog/customers-edit-dialog.component';

import { ServiceOrder } from 'src/app/models/service-order';
import { ServiceOrderItem } from 'src/app/models/service-order-item';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';
import { CustomerInfoByGroup } from 'src/app/models/customer-info-by-group';

import * as moment from 'moment';

@Component({
    selector: 'app-service-orders-item-list',
    templateUrl: './service-orders-item-list.component.html'
})
export class ServiceOrdersItemListComponent implements OnInit {
    @Input() serviceOrder: ServiceOrder;
    @Input() showClose: boolean = true;
    @Output() serviceOrderItemsChanged: EventEmitter<ServiceOrder> = new EventEmitter();
    @Output() closeClicked: EventEmitter<boolean> = new EventEmitter();

    public customerInfoByGroup: CustomerInfoByGroup;
    public newServiceOrderItem: ServiceOrderItem;

    constructor(private router: Router,
        private serviceOrderService: ServiceOrderService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService,
        private aircraftsService: AircraftsService,
        private deleteServiceOrderItemDialog: MatDialog,
        private customerDetailsDialog: MatDialog,
        private aircraftDetailsDialog: MatDialog) {
    }

    ngOnInit() {
        this.resetNewServiceOrderItem();
        if (this.serviceOrder.serviceOrderItems == null)
            this.loadServiceOrderItems();
    }

    public addServiceOrderItemClicked() {
        
        this.serviceOrderService.createServiceOrderItem(this.newServiceOrderItem).subscribe((response: EntityResponseMessage<ServiceOrderItem>) => {
            if (!response.success)
                alert('Error creating service order item: ' + response.message);
            this.serviceOrder.serviceOrderItems.push(response.result);
            this.serviceOrderItemsChanged.emit(this.serviceOrder);
            this.resetNewServiceOrderItem();
        });
    }

    public deleteServiceOrderItemClicked(serviceOrderItem: ServiceOrderItem) {
        const dialogRef = this.deleteServiceOrderItemDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'service order item', item: serviceOrderItem },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.deleteOrderItem(serviceOrderItem);
        });
    }

    public serviceOrderItemEdited(serviceOrderItem: ServiceOrderItem) {
        this.saveServiceOrderItem(serviceOrderItem);
    }

    public serviceItemToggleChanged(event: any) {        
        event.option._value.isCompleted = event.option._selected;
        if (event.option._value.isCompleted) {
            event.option._value.completionDateTimeUtc = moment(new Date().toUTCString()).format('YYYY-MM-DD HH:mm');
            event.option._value.completedByUserId = this.sharedService.currentUser.oid;
            event.option._value.completedByName = this.sharedService.currentUser.firstName + ' ' + this.sharedService.currentUser.lastName;
        }
        this.saveServiceOrderItem(event.option._value);
        this.serviceOrderItemsChanged.emit(this.serviceOrder);
    }

    public viewCustomerDetailsClicked() {
        const dialogRef = this.customerDetailsDialog.open(
            CustomersEditDialogComponent,
            {
                data: {
                    customerInfoByGroupId: this.serviceOrder.customerInfoByGroup.oid
                },
                width: '95%',
                maxWidth: '95%'
            }
        );
    }

    public viewAircraftDetailsClicked() {
        
        const dialogRef = this.aircraftDetailsDialog.open(
            CustomerAircraftsEditComponent,
            {
                data: {
                    disableDelete: true,
                    oid: this.serviceOrder.customerAircraftId,
                    customerId: this.serviceOrder.customerInfoByGroup.customerId
                },
                width: '450px',
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.aircraftsService
                .getAircraftSizes()
                .subscribe((data: any) => {
                    if (data) {
                        //Do nothing - no adjustment needed
                    }
                });
        });
    }

    public onCloseClicked() {
        this.closeClicked.emit(true);
    }

    private loadServiceOrderItems() {
        this.serviceOrderService.getServiceOrderItems(this.serviceOrder.oid).subscribe((response: EntityResponseMessage<ServiceOrderItem[]>) => {
            if (!response.success)
                alert('Error loading service order items: ' + response.message);
            else
                this.serviceOrder.serviceOrderItems = response.result;
        });
    }

    private resetNewServiceOrderItem() {
        this.newServiceOrderItem = { oid: 0, serviceName: '', serviceOrderId: this.serviceOrder.oid, quantity: 1, isCompleted: false, completionDateTimeUtc: null };
    }

    private saveServiceOrderItem(serviceOrderItem: ServiceOrderItem) {        
        this.serviceOrderService.updateServiceOrderItem(serviceOrderItem).subscribe((response: EntityResponseMessage<ServiceOrderItem>) => {
            if (!response.success)
                alert('Error saving service order item: ' + response.message);
            else {
                this.serviceOrderItemsChanged.emit(this.serviceOrder);
            }
        });
    }

    private deleteOrderItem(serviceOrderItem: ServiceOrderItem) {
        this.serviceOrderService.deleteServiceOrderItem(serviceOrderItem.oid).subscribe((response: EntityResponseMessage<ServiceOrderItem>) => {
            if (!response.success)
                alert('Error deleting service order item: ' + response.message);
            else {
                this.serviceOrder.serviceOrderItems.splice(this.serviceOrder.serviceOrderItems.indexOf(serviceOrderItem), 1);
                this.serviceOrderItemsChanged.emit(this.serviceOrder);
            }
        });
    }
}
