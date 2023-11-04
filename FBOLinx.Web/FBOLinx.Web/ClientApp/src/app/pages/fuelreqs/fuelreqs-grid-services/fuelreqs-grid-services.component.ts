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
    @Input() public fuelerlinxTransactionId: number | null = null;
    fuelreqsServicesAndFeesGridDisplay: any;
    noServices: boolean = true;
    isLoading: boolean = true;
    serviceOrder: any;

    constructor(
        private serviceOrderService: ServiceOrderService,
        private serviceTypeService: ServiceTypeService,
        private sharedService: SharedService,
        private dialog: MatDialog,
        private snackBar: SnackBarService,
        private datePipe: DatePipe
    ) { }

    async ngOnInit() {
        if (this.fuelerlinxTransactionId != null) {
            this.serviceOrder = await this.serviceOrderService.getServiceOrderByFuelerLinxTransactionIdFboId(this.fuelerlinxTransactionId, this.sharedService.currentUser.fboId).toPromise();
            if (this.serviceOrder.result != null) {
                this.fuelreqsServicesAndFeesGridDisplay = this.serviceOrder.result.serviceOrderItems;
                this.noServices = false;
            }
        }
        this.isLoading = false;
    }

    createNewItem(serviceType: ServiceTypeResponse): void {
        let category = this.fuelreqsServicesAndFeesGridDisplay.find(elem => elem.serviceType.name == serviceType.name);

        let newItem: ServicesAndFeesGridItem = {
            oid: 0,
            isNewItem: true,
            handlerId: category.servicesAndFees[0]?.handlerId,
            serviceOfferedId: category.servicesAndFees[0]?.serviceOfferedId,
            isCustom: true,
            serviceTypeId: serviceType.oid,
            service: '',
            isActive: true,
            isEditMode: true,
            editedValue: '',
            createdDate: new Date(),
            createdByUser: '',
            createdByUserId: this.sharedService.currentUser.oid,
            isSaving: false
        };


        category.servicesAndFees.push(newItem);
    }
    add(serviceAndfee: ServicesAndFeesGridItem): void {
        serviceAndfee.service = serviceAndfee.editedValue;

        //this.servicesAndFeesService.add(this.sharedService.currentUser.fboId, serviceAndfee)
        //    .subscribe(response => {
        //        serviceAndfee.oid = response.oid;
        //        serviceAndfee.isNewItem = false;
        //        serviceAndfee.createdByUser = this.sharedService.currentUser.username;
        //        this.toogleEditModel(serviceAndfee);
        //    }, error => {
        //        this.snackBar.showErrorSnackBar(`There was an error saving ${serviceAndfee.service} ${serviceAndfee.service} please try again`);
        //        console.log(error);
        //        this.toogleEditModel(serviceAndfee);
        //    });
    }
    updateCompletedFlag(serviceAndfee: ServiceOrderItem): void {
        if (serviceAndfee.isCompleted) {
            serviceAndfee.completionDateTimeUtc = moment.utc().toDate();
            serviceAndfee.completedByUserId = this.sharedService.currentUser.oid;
            serviceAndfee.completedByName = this.sharedService.currentUser.firstName + ' ' + this.sharedService.currentUser.lastName;
        }

        this.saveServiceOrderItem(serviceAndfee);
    }

    toogleEditModel(item: ServicesAndFeesGridItem, itemList: ServicesAndFeesGridItem[] = []): void {
        item.isEditMode = !item.isEditMode;
    }

    deleteItem(serviceAndfee: ServicesAndFeesGridItem, serviceName: string): void {
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

    getInfoTooltipText(serviceAndFees: ServicesAndFeesResponse): string {
        if (!serviceAndFees.isCustom)
            return `Source: Acukwik`;
        else
            return `Source: ${serviceAndFees.createdByUser} - ${this.datePipe.transform(serviceAndFees.createdDate, 'MM/dd/yyyy')}`;
    }

    private saveServiceOrderItem(serviceOrderItem: ServiceOrderItem) {
        this.serviceOrderService.updateServiceOrderItem(serviceOrderItem).subscribe((response: EntityResponseMessage<ServiceOrderItem>) => {
            if (!response.success)
                alert('Error saving service order item: ' + response.message);
        });
    }
}
