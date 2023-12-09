import { Component, EventEmitter, Inject, Output, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NgxUiLoaderService } from 'ngx-ui-loader';

import {SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { CustomeraircraftsService } from 'src/app/services/customeraircrafts.service';
import { AcukwikairportsService } from '../../../services/acukwikairports.service';
import { AircraftsService } from '../../../services/aircrafts.service';
import { FuelreqsService } from 'src/app/services/fuelreqs.service';

import { ServiceOrder } from 'src/app/models/service-order';
import { CustomerInfoByGroup } from 'src/app/models/customer-info-by-group';
import { CustomerAircraft } from 'src/app/models/customer-aircraft';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';
import { ServiceOrderAppliedDateTypes } from '../../../enums/service-order-applied-date-types';
import { AircraftType } from 'src/app/models/aircraft';
import {EnumOptions} from '../../../models/enum-options';
import { share } from 'rxjs/operators';
import { FuelReq } from '../../../models/fuelreq';

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
    public dialogLoader: string = 'dialogLoader';
    public filters = {
        tailNumberFilter: ''
    };

    constructor(public dialogRef: MatDialogRef<ServiceOrdersDialogNewComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ServiceOrder,
        private NgxUiLoader: NgxUiLoaderService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerAircraftsService: CustomeraircraftsService,
        private serviceOrderService: ServiceOrderService,
        private sharedService: SharedService,
        private acukwikAirportsService: AcukwikairportsService,
        private aircraftsService: AircraftsService,
        private fuelreqsService: FuelreqsService) {
                    
    }

    ngOnInit() {
            //Initialize an empty aircraft
        this.onCustomerAircraftFilterChanged('');
        this.loadCustomerAircraftsDataSource();
        this.loadCustomerInfoByGroupDataSource();
        this.loadAircraftTypes();
    }    

    public onCustomerInfoByGroupChanged(selectedCustomerInfoByGroup: CustomerInfoByGroup): void {
        this.data.customerInfoByGroup = selectedCustomerInfoByGroup;

        this.data.customerInfoByGroupId = this.data.customerInfoByGroup.oid;
        if (this.data.customerInfoByGroupId > 0) {
            this.loadCustomerAircraftsDataSource();
        }
        this.setWarningMessage();
    }

    public onCustomerFilterChanged(event) {
        if (event == '' || event == null) {
            this.data.customerInfoByGroup = null;
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
            this.loadCustomerInfoByGroupDataSource();
        }
        this.setWarningMessage();
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
        this.NgxUiLoader.startLoader(this.dialogLoader);
        var data = this.data;

        var newFuelOrder: FuelReq = {
            oid: 0,
            fboId: this.sharedService.currentUser.fboId,
            customerId: 0,
            customerInfoByGroupId: 0,
            arrivalDateTimeLocal: data.arrivalDateTimeLocal,
            departureDateTimeLocal: data.departureDateTimeLocal,
            eta: data.arrivalDateTimeLocal,
            etd: data.departureDateTimeLocal,
            dateCreated: null,
            icao: '',
            customerAircraftId: data.customerAircraftId,
            timeStandard: 'L',
            cancelled: false,
            quotedVolume: 0,
            quotedPpg: 0,
            notes: '',
            actualVolume: 0,
            actualPpg: 0,
            source: '',
            sourceId: 0,
            dispatchNotes: '',
            archived: false,
            email: '',
            phoneNumber: '',
            fuelOn: data.serviceOn.toString(),
            customerName: '',
            customerNotes: '',
            paymentMethod: '',
            timeZone: '',
            isConfirmed: false,
            tailNumber: '',
            fboName: '',
            pricingTemplateName: '',
            serviceOrder: null,
            customer: null,
            customerAircraft: null
        };

        this.fuelreqsService.add(newFuelOrder).subscribe((response: any) => {
            if (response != null) {
                newFuelOrder.oid = response.oid;
                newFuelOrder.customerName = response.customerName;
                newFuelOrder.tailNumber = response.tailNumber;

                if (data.serviceOrderItems.length > 0) {
                    data.associatedFuelOrderId = response.oid;

                    this.serviceOrderService.createServiceOrder(data).subscribe((response: EntityResponseMessage<ServiceOrder>) => {
                        if (!response.success)
                            alert('Error creating service order: ' + response.message);
                        else {
                            data.serviceOrderItems = response.result.serviceOrderItems;
                            this.dialogRef.close(newFuelOrder);
                        }
                    });
                }
                else {
                    this.dialogRef.close(newFuelOrder);
                }
            }
        });
        //this.serviceOrderService.createServiceOrder(this.data).subscribe((response: EntityResponseMessage<ServiceOrder>) => {
        //    this.NgxUiLoader.stopLoader(this.dialogLoader);
        //    if (!response.success)
        //        alert('Error creating service order: ' + response.message);
        //    else
        //        this.dialogRef.close(response.result);
        //});
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
        if (this.data.customerInfoByGroupId == 0) {
            this.errorMessage = '*A valid customer selection is required.';
        } else {
            this.errorMessage = '';
        }
        if (this.data.customerAircraftId == 0 && this.data.customerAircraft?.tailNumber != null && this.data.customerAircraft?.tailNumber.length > 0) {
            this.warningMessage = '*A new tail number will be added with this order.';
        } else {
            this.warningMessage = '';
        }
    }

    private loadCustomerInfoByGroupDataSource() {
        this.NgxUiLoader.startLoader(this.dialogLoader);
        if (this.data.customerAircraft != null && this.data.customerAircraft.oid > 0)
            this.loadCustomerInfoByGroupDataSourceByTail();
        else {
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
                this.NgxUiLoader.stopLoader(this.dialogLoader);
            });
        }        
    }

    private loadCustomerInfoByGroupDataSourceByTail() {
        this.aircraftsService.getCustomersByTail(this.sharedService.currentUser.groupId, this.data.customerAircraft.tailNumber).subscribe((response: CustomerInfoByGroup[]) => {
            this.customerInfoByGroupDataSource = response.sort((n1, n2) => {
                if (n1.company > n2.company) {
                    return 1;
                }
                if (n1.company < n2.company) {
                    return -1;
                }
                return 0;
            });
            this.NgxUiLoader.stopLoader(this.dialogLoader);
        });
    }

    private loadCustomerAircraftsDataSource() {
        this.customerAircraftsDataSource = [];
        this.NgxUiLoader.startLoader(this.dialogLoader);

        if (this.data.customerInfoByGroup && this.data.customerInfoByGroup.customerId > 0) {
            this.loadCustomerAircraftDataSourceByCustomerId();
        } else {
            this.customerAircraftsService.getAircraftsListByGroupAndFbo(this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId).subscribe((response: CustomerAircraft[]) => {
                this.customerAircraftsDataSource = response.sort((n1, n2) => {
                    if (n1.tailNumber > n2.tailNumber) {
                        return 1;
                    }
                    if (n1.tailNumber < n2.tailNumber) {
                        return -1;
                    }
                    return 0;
                });
                this.NgxUiLoader.stopLoader(this.dialogLoader);
            });
        }        
    }

    private loadCustomerAircraftDataSourceByCustomerId() {
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
            this.NgxUiLoader.stopLoader(this.dialogLoader);
        })
    }

    private loadAircraftTypes() {
        this.aircraftsService
            .getAll()
            .subscribe((data: any) => (this.aircraftTypes = data));
    }
}
