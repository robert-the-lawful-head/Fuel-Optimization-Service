import { Component, Inject, OnInit } from '@angular/core';
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
    }

    public onClose() {
        this.dialogRef.close();
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
                .subscribe((customerInfoByGroup: any) => {
                    this.dialogRef.close({
                        aircraftType:
                            this.selectedAircraft.make +
                            ' / ' +
                            this.selectedAircraft.model,
                        company: customerInfoByGroup.company,
                        companyId: customerInfoByGroup.customerId,
                        customerInfoByGroupID: customerInfoByGroup.oid,
                    } as Partial<FlightWatchHistorical>);
                });
        } else {

            payload.customerId = this.selectedCompany;
            this.customerAircraftsService.add(payload , this.sharedService.currentUser.oid ,this.selectedCompany).subscribe(() => {
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
                } as Partial<FlightWatchHistorical>);
            });
        }
    }
}
