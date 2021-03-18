import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

// Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { SharedService } from '../../../layouts/shared-service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';

import { CustomersListType } from '../../../models/customer';
import { FlightWatchHistorical } from 'src/app/models/flight-watch-historical';

export interface NewCustomerAircraftDialogData {
    tailNumber: string;
    customers: CustomersListType[];
}

@Component({
    selector: 'app-aircraft-assign-modal',
    templateUrl: './aircraft-assign-modal.component.html',
    styleUrls: [ './aircraft-assign-modal.component.scss' ],
    providers: [ SharedService ],
})
export class AircraftAssignModalComponent implements OnInit {
    // Public Members
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public selectedAircraft: any;
    public loading: boolean;
    public selectedCompany: number | string;

    constructor(
        public dialogRef: MatDialogRef<AircraftAssignModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewCustomerAircraftDialogData,
        private aircraftsService: AircraftsService,
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService,
    ) {
    }

    ngOnInit() {
        this.aircraftsService
            .getAll()
            .subscribe((data: any) => (this.aircraftTypes = data));
        this.aircraftsService
            .getAircraftSizes()
            .subscribe((data: any) => (this.aircraftSizes = data));
    }

    public onClose() {
        this.dialogRef.close();
    }

    public onSubmit() {
        this.loading = true;
        const payload: any = {
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            aircraftId: this.selectedAircraft.aircraftId,
            tailNumber: this.data.tailNumber,
            size: this.selectedAircraft.size,
        };
        if (typeof this.selectedCompany === 'string') {
            payload.customer = this.selectedCompany;
            this.customerAircraftsService.createAircraftWithCustomer(payload)
                .subscribe((customerInfoByGroup: any) => {
                    this.dialogRef.close({
                        customerInfoByGroupID: customerInfoByGroup.oid,
                        companyId: customerInfoByGroup.customerId,
                        company: customerInfoByGroup.company,
                        aircraftType: this.selectedAircraft.make + ' / ' + this.selectedAircraft.model,
                    } as Partial<FlightWatchHistorical>);
                });
        } else {
            payload.customerId = this.selectedCompany;
            this.customerAircraftsService.add(payload)
                .subscribe(() => {
                    const selectedCompany = this.data.customers.find(customer => customer.companyId === this.selectedCompany);
                    this.dialogRef.close({
                        customerInfoByGroupID: selectedCompany.customerInfoByGroupID,
                        companyId: selectedCompany.companyId,
                        company: selectedCompany.company,
                        aircraftType: this.selectedAircraft.make + ' / ' + this.selectedAircraft.model,
                    } as Partial<FlightWatchHistorical>);
                });
        }
    }
}
