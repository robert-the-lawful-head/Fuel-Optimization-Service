import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SharedService } from 'src/app/layouts/shared-service';
import { FbosServicesAndFeesResponse, ServiceTypeResponse, ServicesAndFees, ServicesAndFeesResponse } from 'src/app/models/services-and-fees/services-and-fees';
import { ServicesAndFeesService } from 'src/app/services/servicesandfees.service';
import { DeleteConfirmationComponent } from 'src/app/shared/components/delete-confirmation/delete-confirmation.component';
import { DatePipe } from '@angular/common';
import { ServiceTypeService } from 'src/app/services/serviceTypes.service';
import { AccountType } from 'src/app/enums/user-role';
import { SnackBarService } from 'src/app/services/utils/snackBar.service';
import { ItemInputComponent } from '../../services-and-fees/item-input/item-input.component';
import { ServiceOrderService } from '../../../services/serviceorder.service';
import { ServiceOrder } from '../../../models/service-order';
import { Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { EntityResponseMessage } from '../../../models/entity-response-message';
import * as moment from 'moment';
import { ServiceOrderItem } from '../../../models/service-order-item';
import { ServiceOrderAppliedDateTypes } from '../../../enums/service-order-applied-date-types';
import { FuelreqsService } from '../../../services/fuelreqs.service';

interface ServicesAndFeesGridItem extends ServicesAndFeesResponse {
    isEditMode: boolean,
    isNewItem: boolean,
    editedValue: string,
    isSaving: boolean
}
@Component({
    selector: 'app-fuelreqs-grid-services',
    templateUrl: './fuelreqs-grid-services.component.html',
    styleUrls: [ './fuelreqs-grid-services.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class FuelreqsGridServicesComponent implements OnInit {
    @Input() public serviceOrderId: number | null = null;
    @Input() public associatedFuelOrderId: number | null = null;
    @Input() public fuelerlinxTransactionId: number | null = null;
    @Input() public servicesAndFees: string[];
    public newServiceOrderItem: ServiceOrderItem;
    fuelreqsServicesAndFeesGridDisplay: ServiceOrderItem[] = [];
    isLoading: boolean = true;
    serviceOrder: any;

    constructor(
        private serviceOrderService: ServiceOrderService,
        private serviceTypeService: ServiceTypeService,
        private sharedService: SharedService,
        private dialog: MatDialog,
        private snackBar: SnackBarService,
        private datePipe: DatePipe,
        private fuelReqsService: FuelreqsService
    ) { }

    async ngOnInit() {
        if (this.serviceOrderId != null && this.serviceOrderId > 0) {
            this.serviceOrder = await this.serviceOrderService.getServiceOrderById(this.serviceOrderId).toPromise();
            if (this.serviceOrder.result != null) {
                this.fuelreqsServicesAndFeesGridDisplay = this.serviceOrder.result.serviceOrderItems;
            }
        }

        this.resetNewServiceOrderItem();

        this.fuelreqsServicesAndFeesGridDisplay.push(this.newServiceOrderItem);
        this.isLoading = false;
    }

    onAddServiceChanged(serviceOrderItem: ServiceOrderItem, changed: any): void {
        serviceOrderItem.serviceName = changed;
    }

    add(serviceOrderItem: ServiceOrderItem): void {
        serviceOrderItem.isAdding = true;
        if (serviceOrderItem.serviceOrderId == 0) {
            var fuelOrderId = 0;
            if (this.fuelerlinxTransactionId > 0)
                fuelOrderId = this.fuelerlinxTransactionId;
            else
                fuelOrderId = this.associatedFuelOrderId;

            this.fuelReqsService.get(fuelOrderId).subscribe((fuelReq: any) => {
                if (fuelReq != undefined) {
                    var newServiceOrder: ServiceOrder = {
                        oid: 0,
                        fboId: this.sharedService.currentUser.fboId,
                        serviceOrderItems: [],
                        arrivalDateTimeUtc: moment(fuelReq.eta).toDate(),
                        arrivalDateTimeLocal: null,
                        departureDateTimeUtc: moment(fuelReq.etd).toDate(),
                        departureDateTimeLocal: null,
                        groupId: this.sharedService.currentUser.groupId,
                        customerInfoByGroupId: 0,
                        customerAircraftId: fuelReq.customerAircraftId,
                        associatedFuelOrderId: this.associatedFuelOrderId,
                        FuelerLinxTransactionId: this.fuelerlinxTransactionId,
                        serviceOn: fuelReq.FuelOn == "Departure" ? ServiceOrderAppliedDateTypes.Departure : ServiceOrderAppliedDateTypes.Arrival,
                        serviceDateTimeUtc: fuelReq.FuelOn == "Departure" ? moment(fuelReq.etd).toDate() : moment(fuelReq.eta).toDate(),
                        numberOfCompletedItems: 0,
                        isCompleted: false,
                        customerInfoByGroup: null,
                        customerAircraft: null
                    }

                    this.serviceOrderService.createServiceOrder(newServiceOrder).subscribe((response: EntityResponseMessage<ServiceOrder>) => {
                        if (!response.success)
                            alert('Error creating service order: ' + response.message);
                        else {
                            serviceOrderItem.serviceOrderId = response.result.oid;
                            this.serviceOrderService.createServiceOrderItem(serviceOrderItem).subscribe((responseItem: EntityResponseMessage<ServiceOrderItem>) => {
                                if (!responseItem.success)
                                    alert('Error saving service order item: ' + responseItem.message);
                                else {
                                    serviceOrderItem.isAddMode = false;
                                    serviceOrderItem.isAdding = false;

                                    this.resetNewServiceOrderItem();

                                    this.fuelreqsServicesAndFeesGridDisplay.push(this.newServiceOrderItem);
                                }
                            });
                        }
                    });
                }
            });

        }
        else {
            this.serviceOrderService.createServiceOrderItem(serviceOrderItem).subscribe((response: EntityResponseMessage<ServiceOrderItem>) => {
                if (!response.success)
                    alert('Error saving service order item: ' + response.message);
                else {
                    serviceOrderItem.isAddMode = false;
                    serviceOrderItem.isAdding = false;

                    this.resetNewServiceOrderItem();

                    this.fuelreqsServicesAndFeesGridDisplay.push(this.newServiceOrderItem);
                }
            });
        }
        
    }

    cancelEdit(service: ServiceOrderItem):void{
        service.isEditMode = false;
    }

    updateCompletedFlag(serviceAndfee: ServiceOrderItem): void {
        if (serviceAndfee.isCompleted) {
            serviceAndfee.completionDateTimeUtc = moment.utc().toDate();
            serviceAndfee.completedByUserId = this.sharedService.currentUser.oid;
            serviceAndfee.completedByName = this.sharedService.currentUser.firstName + ' ' + this.sharedService.currentUser.lastName;
        }

        this.saveServiceOrderItem(serviceAndfee);
    }

    updateServiceName(serviceAndfee: ServiceOrderItem): void {
        this.saveServiceOrderItem(serviceAndfee);
    }

    toogleEditModel(item: ServiceOrderItem, itemList: ServiceOrderItem[] = []): void {
        item.isEditMode = !item.isEditMode;
    }

    deleteItem(serviceAndfee: ServiceOrderItem, serviceName: string): void {
        const dialogRef = this.dialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: serviceName + ' from this order', item: serviceAndfee },
            }
        );

        dialogRef.afterClosed().subscribe((serviceAndfee) => {
            if (!serviceAndfee) {
                return;
            }
            this.serviceOrderService.deleteServiceOrderItem(serviceAndfee.item.oid).subscribe((response: EntityResponseMessage<ServiceOrder>) => {
                if (!response.success)
                    alert('Error deleting service order item: ' + response.message);
                else {
                    this.fuelreqsServicesAndFeesGridDisplay.splice(this.fuelreqsServicesAndFeesGridDisplay.indexOf(serviceAndfee), 1);
                }
            });
        });
    }

    getInfoTooltipText(serviceAndFees: ServiceOrderItem): string {
        //if (!serviceAndFees.isCustom)
        //    return `Source: Acukwik`;
        //else
        //    return `Source: ${serviceAndFees.createdByUser} - ${this.datePipe.transform(serviceAndFees.createdDate, 'MM/dd/yyyy')}`;
        return '';
    }

    private saveServiceOrderItem(serviceOrderItem: ServiceOrderItem) {
        this.serviceOrderService.updateServiceOrderItem(serviceOrderItem).subscribe((response: EntityResponseMessage<ServiceOrderItem>) => {
            if (!response.success)
                alert('Error saving service order item: ' + response.message);
        });
    }

    private resetNewServiceOrderItem() {
        this.newServiceOrderItem = { oid: 0, serviceName: '', serviceOrderId: (this.serviceOrderId == undefined || this.serviceOrderId == null ? 0 : this.serviceOrderId), quantity: 1, isCompleted: false, completionDateTimeUtc: null, isEditMode: false, isAddMode: true, isAdding: false };
    }
}
