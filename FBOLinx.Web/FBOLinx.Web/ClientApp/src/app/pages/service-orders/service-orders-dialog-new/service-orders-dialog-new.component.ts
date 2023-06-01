import { Component, EventEmitter, Inject, Output, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import {SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { CustomeraircraftsService } from 'src/app/services/customeraircrafts.service';
import { AcukwikairportsService } from '../../../services/acukwikairports.service' ;

import { ServiceOrder } from 'src/app/models/service-order';
import { CustomerInfoByGroup } from 'src/app/models/customer-info-by-group';
import { CustomerAircraft } from 'src/app/models/customer-aircraft';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';
import { share } from 'rxjs/operators';

@Component({
    selector: 'app-service-orders-dialog-new',
    templateUrl: './service-orders-dialog-new.component.html',
})
export class ServiceOrdersDialogNewComponent implements OnInit {
    public errorMessage: string;
    public customerInfoByGroupDataSource: CustomerInfoByGroup[];
    public customerAircraftsDataSource: CustomerAircraft[] = [];

    constructor(public dialogRef: MatDialogRef<ServiceOrdersDialogNewComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ServiceOrder,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerAircraftsService: CustomeraircraftsService,
        private serviceOrderService: ServiceOrderService,
        private sharedService: SharedService,
        private acukwikAirportsService: AcukwikairportsService) {


    }

    ngOnInit() {
        this.loadCustomerInfoByGroupDataSource();
    }    

    public onCustomerInfoByGroupChanged(): void {        
        this.data.customerAircraft = null;
        this.onCustomerAircraftChanged();

        this.data.customerInfoByGroupId = this.data.customerInfoByGroup.oid;
        if (this.data.customerInfoByGroupId > 0) {
            this.loadCustomerAircraftsDataSource();
        }
    }

    public onCustomerAircraftChanged(): void {
        if (this.data.customerAircraft != null)
            this.data.customerAircraftId = this.data.customerAircraft.oid;
        else
           this.data.customerAircraftId = 0;
    }

    public onArrivalDateTimeLocalChanged() {
        this.acukwikAirportsService.getAirportZuluTime(this.sharedService.currentUser.icao, this.data.arrivalDateTimeLocal).subscribe((response: Date) => {
            if (response)
                this.data.arrivalDateTimeUtc = response;
        });
    }

    public onDepartureDateTimeLocalChanged() {
        this.acukwikAirportsService.getAirportZuluTime(this.sharedService.currentUser.icao, this.data.departureDateTimeLocal).subscribe((response: Date) => {
            if (response)
                this.data.departureDateTimeUtc = response;
        });
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

    private loadCustomerInfoByGroupDataSource() {
        this.customerInfoByGroupService.getCustomerInfoByGroupListByGroupId(this.data.groupId).subscribe((response: CustomerInfoByGroup[]) => {
            this.customerInfoByGroupDataSource = response.sort((n1, n2) => {
                if (n1.company > n2.company) {
                    return 1;
                }
                if (n1.company < n2.company) {
                    return -1;
                }
                return 0;
            });
        });
    }

    private loadCustomerAircraftsDataSource() {
        this.customerAircraftsDataSource = [];
        this.customerAircraftsService.getCustomerAircraftsByGroupAndCustomerId(this.data.groupId, this.data.fboId, this.data.customerInfoByGroup.customerId).subscribe((response: CustomerAircraft[]) => {
            this.customerAircraftsDataSource = response.sort((n1, n2) => {
                if (n1.tailNumber > n2.tailNumber) {
                    return 1;
                }
                if (n1.tailNumber < n2.tailNumber) {
                    return -1;
                }
                return 0;
            });
        })
    }
}
