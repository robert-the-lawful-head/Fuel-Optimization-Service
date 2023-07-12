import { Component, EventEmitter, Inject, Output, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import {SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { CustomeraircraftsService } from 'src/app/services/customeraircrafts.service';
import { AcukwikairportsService } from '../../../services/acukwikairports.service';
import { AircraftsService } from '../../../services/aircrafts.service';

import { ServiceOrder } from 'src/app/models/service-order';
import { CustomerInfoByGroup } from 'src/app/models/customer-info-by-group';
import { CustomerAircraft } from 'src/app/models/customer-aircraft';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';
import { ServiceOrderAppliedDateTypes } from '../../../enums/service-order-applied-date-types';
import { AircraftType } from 'src/app/models/aircraft';
import {EnumOptions} from '../../../models/enum-options';
import { share } from 'rxjs/operators';

@Component({
    selector: 'app-service-orders-dialog-new',
    templateUrl: './service-orders-dialog-new.component.html',
})
export class ServiceOrdersDialogNewComponent implements OnInit {
    public errorMessage: string;
    public warningMessage: string;
    public customerInfoByGroupDataSource: CustomerInfoByGroup[];
    public customerAircraftsDataSource: CustomerAircraft[] = [];
    public appliedDateTypes: EnumOptions.EnumOption[] = EnumOptions.serviceOrderAppliedDateTypeOptions;
    public aircraftTypes: Array<any>;
    public selectedAircraftId: number;
    public isLoadingAircraft: boolean = false;
    public filters = {
        tailNumberFilter: ''
    };

    constructor(public dialogRef: MatDialogRef<ServiceOrdersDialogNewComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ServiceOrder,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerAircraftsService: CustomeraircraftsService,
        private serviceOrderService: ServiceOrderService,
        private sharedService: SharedService,
        private acukwikAirportsService: AcukwikairportsService,
        private aircraftsService: AircraftsService) {
                    
    }

    ngOnInit() {
            //Initialize an empty aircraft
        this.onCustomerAircraftFilterChanged('');
        this.loadCustomerInfoByGroupDataSource();
        this.loadAircraftTypes();
    }    

    public onCustomerInfoByGroupChanged(selectedCustomerInfoByGroup: CustomerInfoByGroup): void {
        this.data.customerInfoByGroup = selectedCustomerInfoByGroup;
        this.onCustomerAircraftChanged(null);
        this.filters.tailNumberFilter = '';

        this.data.customerInfoByGroupId = this.data.customerInfoByGroup.oid;
        if (this.data.customerInfoByGroupId > 0) {
            this.loadCustomerAircraftsDataSource();
        }
    }

    public onCustomerFilterChanged(event) {
        if (event == '' || event == null) {
            this.data.customerInfoByGroup = null;
            this.onCustomerAircraftChanged(null);
        } else {
            this.data.customerInfoByGroup = {
                oid: 0,
                company: event,
                groupId: this.sharedService.currentUser.groupId,
                customerId: 0
            };
        }
        
        this.data.customerInfoByGroupId = 0;
        this.customerAircraftsDataSource = [];
    }

    public onCustomerAircraftChanged(customerAircraft: CustomerAircraft): void {
        if (customerAircraft == null) {
            this.onCustomerAircraftFilterChanged('');
        } else {
            this.data.customerAircraft = customerAircraft;
            this.data.customerAircraftId = this.data.customerAircraft.oid;
        }
    }

    public onCustomerAircraftFilterChanged(event) {
        this.data.customerAircraft = {
            oid: 0,
            groupId: this.sharedService.currentUser.groupId,
            customerId: 0,
            tailNumber: event,
            aircraftId: this.selectedAircraftId
        }        
        this.data.customerAircraftId = 0;        
    }

    public onAircraftTypeChanged(aircraftType: AircraftType) {
        this.selectedAircraftId = aircraftType.aircraftId;
        this.data.customerAircraft.aircraftId = this.selectedAircraftId;
    }

    public onArrivalDateTimeLocalChanged() {
        this.acukwikAirportsService.getAirportZuluTime(this.sharedService.currentUser.icao, this.data.arrivalDateTimeLocal).subscribe((response: Date) => {
            if (response)
                this.data.arrivalDateTimeUtc = response;
            if (this.data.serviceOn == ServiceOrderAppliedDateTypes.Arrival) {
                this.data.serviceDateTimeLocal = this.data.arrivalDateTimeLocal;
                this.data.serviceDateTimeUtc = this.data.arrivalDateTimeUtc;
            }
            if (this.data.departureDateTimeUtc == null || this.data.arrivalDateTimeUtc > this.data.departureDateTimeUtc) {
                this.data.departureDateTimeLocal = this.data.arrivalDateTimeLocal;
                this.onDepartureDateTimeLocalChanged();
            }
            this.setWarningMessage();
        });
    }

    public onDepartureDateTimeLocalChanged() {
        this.acukwikAirportsService.getAirportZuluTime(this.sharedService.currentUser.icao, this.data.departureDateTimeLocal).subscribe((response: Date) => {
            if (response)
                this.data.departureDateTimeUtc = response;
            if (this.data.serviceOn == ServiceOrderAppliedDateTypes.Departure) {
                this.data.serviceDateTimeLocal = this.data.departureDateTimeLocal;
                this.data.serviceDateTimeUtc = this.data.departureDateTimeUtc;
            }
            this.setWarningMessage();
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

    public displayCustomerName(customer: CustomerInfoByGroup) {
        return customer ? customer.company : customer;
    }

    public displayTailNumber(customerAircraft: CustomerAircraft) {
        return customerAircraft ? customerAircraft.tailNumber : customerAircraft;
    }

    public serviceOnChanged() {
        if (this.data.serviceOn == ServiceOrderAppliedDateTypes.Arrival && this.data.arrivalDateTimeLocal) {
            this.data.serviceDateTimeLocal = this.data.arrivalDateTimeLocal;
            this.data.serviceDateTimeUtc = this.data.arrivalDateTimeUtc;
        }
        else if (this.data.serviceOn == ServiceOrderAppliedDateTypes.Departure && this.data.departureDateTimeLocal) {
            this.data.serviceDateTimeLocal = this.data.departureDateTimeLocal;
            this.data.serviceDateTimeUtc = this.data.departureDateTimeUtc;
        }
    }

    public displayAircraft(aircraft) {
        return aircraft ? `${aircraft.make} ${aircraft.model}` : aircraft;
    }

    private setWarningMessage() {
        if (this.data.customerInfoByGroupId == 0 && this.data.customerInfoByGroup?.company != null && this.data.customerInfoByGroup?.company.length > 0) {
            this.warningMessage = '*A new customer will be created with this order.';
        }
        if (this.data.customerAircraftId == 0 && this.data.customerAircraft?.tailNumber != null && this.data.customerAircraft?.tailNumber.length > 0) {
            this.warningMessage = '*A new tail number will be added with this order.';
        }
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
        this.isLoadingAircraft = true;
        this.customerAircraftsDataSource = [];
        this.customerAircraftsService.getCustomerAircraftsByGroupAndCustomerId(this.data.groupId, this.data.fboId, this.data.customerInfoByGroup.customerId).subscribe((response: CustomerAircraft[]) => {
            this.isLoadingAircraft = false;
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

    private loadAircraftTypes() {
        this.aircraftsService
            .getAll()
            .subscribe((data: any) => (this.aircraftTypes = data));
    }
}
