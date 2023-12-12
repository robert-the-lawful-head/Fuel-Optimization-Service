import { Component, Inject, OnInit, SimpleChanges } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FlightWatchHistorical } from 'src/app/models/flight-watch-historical';

import { SharedService } from '../../../layouts/shared-service';
import { CustomersListType } from '../../../models/customer';
// Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';

export interface NewCustomerAircraftDialogData {
    tailNumber: string;
    customers: CustomersListType[];
}

@Component({
    providers: [SharedService],
    selector: 'app-aircraft-assign-modal',
    styleUrls: ['./aircraft-assign-modal.component.scss'],
    templateUrl: './aircraft-assign-modal.component.html',
})
export class AircraftAssignModalComponent implements OnInit {
    // Public Members
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public selectedAircraft: any;
    public loading: boolean;
    public selectedCompany: number | string;
    public filteredCostumers: any[] = [];
    public isCustomerEditable: boolean = false;
    public customerFilter: string = '';
    constructor(
        public dialogRef: MatDialogRef<AircraftAssignModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewCustomerAircraftDialogData,
        private aircraftsService: AircraftsService,
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        this.aircraftsService
            .getAll()
            .subscribe((data: any) => (this.aircraftTypes = data));
        this.aircraftsService
            .getAircraftSizes()
            .subscribe((data: any) => (this.aircraftSizes = data));
        this.filteredCostumers = this.data.customers;
        this.selectedCompany = this.data.customers[0].companyId;
    }
    public onClose() {
        this.dialogRef.close();
    }
    createCustomer(){
        this.isCustomerEditable = !this.isCustomerEditable;
        this.selectedCompany =  this.customerFilter;
    }
    onFilter(event: any) {
        this.isCustomerEditable = false;
        this.filteredCostumers = this.data.customers.filter((c) => c.company.toLowerCase().includes(event.filter.toLowerCase()));
        this.customerFilter = event.filter;

    }
    public onSubmit() {
        this.loading = true;
        const payload: any = {
            aircraftId: this.selectedAircraft.aircraftId,
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            size: this.selectedAircraft.size,
            tailNumber: this.data.tailNumber,
        };
        if (typeof this.selectedCompany === 'string') {
            payload.customer = this.selectedCompany;
            this.customerAircraftsService
                .createAircraftWithCustomer(payload)
                .subscribe((result: any) => {
                    this.data.customers.push(
                       {
                            customerInfoByGroupID : result.customerInfoByGroup.oid,
                            companyId : result.customerInfoByGroup.customerId,
                            company : result.customerInfoByGroup.company
                       }
                    );

                    this.dialogRef.close({
                        aircraftType:
                            this.selectedAircraft.make +
                            ' / ' +
                            this.selectedAircraft.model,
                        company: result.customerInfoByGroup.company,
                        companyId: result.customerInfoByGroup.customerId,
                        customerInfoByGroupID: result.customerInfoByGroup.oid,
                        customerAircraftId: result.customerAircraft.oid,
                    } as Partial<FlightWatchHistorical>);
                });
        } else {
            payload.customerId = this.selectedCompany;
            this.customerAircraftsService.add(payload , this.sharedService.currentUser.oid ).subscribe((result: any) => {
                const selectedCompany = this.data.customers.find(
                    (customer) => customer.companyId === this.selectedCompany
                );
                this.dialogRef.close({
                    aircraftType:
                        this.selectedAircraft.make +
                        ' / ' +
                        this.selectedAircraft.model,
                    company: selectedCompany.company,
                    companyId: selectedCompany.companyId,
                    customerInfoByGroupID:
                        selectedCompany.customerInfoByGroupID,
                    customerAircraftId: result.oid,
                } as Partial<FlightWatchHistorical>);
            });
        }
    }
}
