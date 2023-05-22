import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';
import { ServiceOrder } from 'src/app/models/service-order';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';

@Component({
    selector: 'app-service-orders-dialog-new',
    templateUrl: './service-orders-dialog-new.component.html',
})
export class ServiceOrdersDialogNewComponent {

    constructor(public dialogRef: MatDialogRef<ServiceOrdersDialogNewComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ServiceOrder,
        private serviceOrderService: ServiceOrderService) {

    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public onSaveChanges(): void {
        this.serviceOrderService.createServiceOrder(this.data).subscribe((response: EntityResponseMessage<ServiceOrder>) => {
            if (!response.success)
                alert('Error creating service order: ' + response.message);
            else
                this.dialogRef.close(response.result);
        });
    }
}
