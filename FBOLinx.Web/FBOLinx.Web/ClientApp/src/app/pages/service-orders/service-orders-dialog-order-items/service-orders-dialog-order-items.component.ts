import { Component, EventEmitter, Inject, Output, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { CustomeraircraftsService } from 'src/app/services/customeraircrafts.service';

import { ServiceOrder } from 'src/app/models/service-order';
import { CustomerInfoByGroup } from 'src/app/models/customer-info-by-group';
import { CustomerAircraft } from 'src/app/models/customer-aircraft';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';
import { share } from 'rxjs/operators';
import { AccountType } from 'src/app/enums/user-role';

@Component({
    selector: 'app-service-orders-dialog-order-items',
    templateUrl: './service-orders-dialog-order-items.component.html',
})
export class ServiceOrdersDialogOrderItemsComponent implements OnInit {
    public errorMessage: string;
    public customerInfoByGroupDataSource: CustomerInfoByGroup[];
    public customerAircraftsDataSource: CustomerAircraft[] = [];
    public selectedServiceOrder: ServiceOrder;
    public isFreemiumAccount: boolean = true;

    constructor(public dialogRef: MatDialogRef<ServiceOrdersDialogOrderItemsComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ServiceOrder,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerAircraftsService: CustomeraircraftsService,
        private serviceOrderService: ServiceOrderService,
        private sharedService: SharedService) {
    }

    ngOnInit() {
        this.loadServiceOrder();
        this.isFreemiumAccount =  this.sharedService.currentUser.accountType == AccountType.Freemium;
    }

    private loadServiceOrder(): void {
        this.serviceOrderService.getServiceOrderById(this.data.oid).subscribe(
            (response: EntityResponseMessage<ServiceOrder>) => {
                if (!response.success)
                    this.errorMessage = response.message;
                else
                    this.selectedServiceOrder = response.result;
            },
            (error: any) => {
                this.errorMessage = <any>error;
            }
        );
    }
}
