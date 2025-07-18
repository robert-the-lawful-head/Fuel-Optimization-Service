import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatLegacyDialog as MatDialog, MatLegacyDialogConfig as MatDialogConfig } from '@angular/material/legacy-dialog';

import { SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';

import { ServiceOrdersDialogNewComponent } from '../service-orders-dialog-new/service-orders-dialog-new.component';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

import { ServiceOrder } from 'src/app/models/service-order';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';
import { MatSidenav } from '@angular/material/sidenav';

import * as moment from 'moment';
import { ServiceOrderAppliedDateTypes } from '../../../enums/service-order-applied-date-types';
import { AccountType } from 'src/app/enums/user-role';
import { MatLegacySelectionList as MatSelectionList } from '@angular/material/legacy-list';

@Component({
    selector: 'app-service-orders-list',
    templateUrl: './service-orders-list.component.html'
})
export class ServiceOrdersListComponent implements OnInit {
    @Input() serviceOrdersData: Array<ServiceOrder>;
    @Input() allowAddingNew: boolean = true;

    @ViewChild('serviceOrderDrawer') serviceOrderDrawer: MatSidenav;
    @ViewChild('incompletedServiceOrders') selectionList: MatSelectionList;

    public selectedServiceOrder: ServiceOrder;
    public inCompleteServiceOrders: Array<ServiceOrder>;
    public completeServiceOrders: Array<ServiceOrder>;
    public globalFilter: string = '';
    public filterStartDate: Date = new Date(moment().add(-1, 'M').format('YYYY-MM-DD'));
    public filterEndDate: Date = new Date(moment().add(1, 'M').format('YYYY-MM-DD'));
    public sortType: string = 'arrivalDateTimeLocal';
    public isFreemiumAccount: boolean = true;

    constructor(
        private serviceOrderService: ServiceOrderService,
        private sharedService: SharedService,
        private newServiceOrderDialog: MatDialog,
        private deleteServiceOrderDialog: MatDialog) {
    }

    ngOnInit() {
        if (!this.serviceOrdersData)
            this.loadServiceOrders();
        else
            this.arrangeServiceOrders();

        this.isFreemiumAccount = this.sharedService.currentUser.accountType == AccountType.Freemium;
    }

    ngAfterViewInit() {
        this.selectionList?.deselectAll();
    }
    public addServiceOrderClicked() {
        var newServiceOrder: ServiceOrder = {
            oid: 0,
            fboId: this.sharedService.currentUser.fboId,
            serviceOrderItems: [],
            arrivalDateTimeUtc: null,
            arrivalDateTimeLocal: null,
            departureDateTimeUtc: null,
            departureDateTimeLocal: null,
            groupId: this.sharedService.currentUser.groupId,
            customerInfoByGroupId: 0,
            customerAircraftId: 0,
            associatedFuelOrderId: 0,
            serviceOn: ServiceOrderAppliedDateTypes.Arrival,
            numberOfCompletedItems: 0,
            isCompleted: false,
            customerInfoByGroup: null,
            customerAircraft: null,
            numberOfTotalServices: 0,
            isActive: false
        };
        const config: MatDialogConfig = {
            disableClose: true,
            data: newServiceOrder,
            autoFocus: false,
            maxWidth: '510px'
        };

        const dialogRef = this.newServiceOrderDialog.open(ServiceOrdersDialogNewComponent, config);

        dialogRef.afterClosed().subscribe((result: ServiceOrder) => {
            if (!result)
                return;
            this.serviceOrdersData.push(result);
            this.arrangeServiceOrders();
            this.serviceOrderClicked(result);
        });
    }

    public serviceOrderClicked(serviceOrder: ServiceOrder) {
        this.selectedServiceOrder = serviceOrder;
        this.serviceOrderDrawer.open();
    }

    public serviceOrderItemsChanged(serviceOrder: ServiceOrder) {
        this.calculateCompletions(serviceOrder);
        this.arrangeServiceOrders();
        this.sharedService.emitChange('service-orders-changed');
    }

    public deleteServiceOrderClicked(serviceOrder: ServiceOrder) {
        const dialogRef = this.deleteServiceOrderDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'service order', item: serviceOrder, includeThis: true },
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
        if(this.isFreemiumAccount){
            this.selectionList.deselectAll();
            return;
        }
        var serviceOrder: ServiceOrder = event.option._value;

        for (const item of serviceOrder.serviceOrderItems) {
            item.isCompleted = event.option._selected;
            if (item.isCompleted) {
                    item.completionDateTimeUtc = moment(new Date().toUTCString()).toDate();
                    item.completedByUserId = this.sharedService.currentUser.oid;
                    item.completedByName = this.sharedService.currentUser.firstName + ' ' + this.sharedService.currentUser.lastName;
            }
        }
        this.calculateCompletions(serviceOrder);
        this.arrangeServiceOrders();
        this.saveOrder(serviceOrder);
    };

    public filterChanged(filterValue: string) {
        this.globalFilter = filterValue;
        this.arrangeServiceOrders();
    }

    public serviceOrderItemsCloseClicked() {
        this.serviceOrderDrawer.close();
        this.selectedServiceOrder = null;
    }

    public applyDateFilterChange() {
        this.loadServiceOrders();
    }

    public sortTypeChanged() {
        this.sortOrders();
    }
    private loadServiceOrders() {
        this.serviceOrdersData = null;
        this.serviceOrderService.getServiceOrdersForFbo(this.sharedService.currentUser.fboId, this.filterStartDate, this.filterEndDate).subscribe((response: EntityResponseMessage<Array<ServiceOrder>>) => {
            if (!response.success)
                alert('Error getting service orders: ' + response.message);
            else {
                this.serviceOrdersData = response.result;
                this.arrangeServiceOrders();
            }
        });
    }

    private calculateCompletions(serviceOrder: ServiceOrder) {
        if(serviceOrder.serviceOrderItems.length == 0)
        {
            serviceOrder.numberOfCompletedItems = 0;
            serviceOrder.isCompleted = !serviceOrder.isCompleted;
            return;
        }
        serviceOrder.numberOfCompletedItems = serviceOrder.serviceOrderItems.filter(x => x.isCompleted).length;
        serviceOrder.isCompleted = serviceOrder.numberOfCompletedItems == serviceOrder.serviceOrderItems.length;
    }

    private arrangeServiceOrders() {
        var filter = this.globalFilter.toUpperCase();
        this.inCompleteServiceOrders = this.serviceOrdersData.filter(x =>
            (!x.isCompleted || x.serviceOrderItems == null || x.serviceOrderItems.filter(item => item.isCompleted).length != x.serviceOrderItems.length)
            && (filter == '' || x.customerAircraft?.tailNumber?.toUpperCase().indexOf(filter) > -1 || x.customerInfoByGroup?.company?.toUpperCase().indexOf(filter) > -1)
        );

        this.completeServiceOrders = this.serviceOrdersData.filter(x =>
            (x.isCompleted ||x.serviceOrderItems == null || x.serviceOrderItems.length > 0 && x.serviceOrderItems.filter(item => item.isCompleted).length == x.serviceOrderItems.length)
            && (filter == '' || x.customerAircraft?.tailNumber?.toUpperCase().indexOf(filter) > -1 || x.customerInfoByGroup?.company?.toUpperCase().indexOf(filter) > -1)
        );

        this.sortOrders();
    }

    private saveOrder(serviceOrder: ServiceOrder) {
        this.serviceOrderService.updateServiceOrder(serviceOrder).subscribe((response: EntityResponseMessage<ServiceOrder>) => {
            if (!response.success)
                alert('Error saving service order: ' + response.message);
            this.sharedService.emitChange('service-orders-changed');
        });
    }

    private deleteOrder(serviceOrder: ServiceOrder) {
        this.serviceOrderService.deleteServiceOrder(serviceOrder.oid).subscribe((response: EntityResponseMessage<ServiceOrder>) => {
            if (!response.success)
                alert('Error deleting service order: ' + response.message);
            else {
                this.serviceOrdersData.splice(this.serviceOrdersData.indexOf(serviceOrder), 1);
                this.arrangeServiceOrders();
                this.sharedService.emitChange('service-orders-changed');
            }
        });
    }

    private sortOrders() {
        this.customSort(this.inCompleteServiceOrders);
        this.customSort(this.completeServiceOrders);
    }

    private customSort(arr: ServiceOrder[]): void{
        arr.sort((a, b) => {
            if(this.sortType == "customerInfoByGroup.company")
                return b.customerInfoByGroup.company.localeCompare(a.customerInfoByGroup.company);

            return b[this.sortType].localeCompare(a[this.sortType])
            }
        );
    }
}
