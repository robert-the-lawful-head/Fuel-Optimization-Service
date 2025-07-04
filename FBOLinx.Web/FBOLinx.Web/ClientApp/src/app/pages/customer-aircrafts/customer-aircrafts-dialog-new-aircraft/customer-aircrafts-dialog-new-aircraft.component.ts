import { Component, Inject, OnInit } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

// Services
import { AircraftsService } from '../../../services/aircrafts.service';

export interface NewCustomerAircraftDialogData {
    gropupId: number;
    customerId: number;
    aircraftId: number;
    tailNumber: string;
    size: number;
    basedPageLocation: string;
    networkCode: string;
    addedFrom: number;
    selectedAircraft: any;
}

@Component({
    selector: 'app-customer-aircrafts-dialog-new-aircraft',
    styleUrls: ['./customer-aircrafts-dialog-new-aircraft.component.scss'],
    templateUrl: './customer-aircrafts-dialog-new-aircraft.component.html',
})
export class CustomerAircraftsDialogNewAircraftComponent implements OnInit {
    // Public Members
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;

    constructor(
        public dialogRef: MatDialogRef<CustomerAircraftsDialogNewAircraftComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewCustomerAircraftDialogData,
        private aircraftsService: AircraftsService
    ) {}

    ngOnInit() {
        this.aircraftsService
            .getAll()
            .subscribe((data: any) => (this.aircraftTypes = data));
        this.aircraftsService
            .getAircraftSizes()
            .subscribe((data: any) => (this.aircraftSizes = data));
    }

    // Public Methods
    public onAircraftTypeChanged(selectedAircraft) {
        this.data.selectedAircraft = selectedAircraft;
        this.data.aircraftId = this.data.selectedAircraft.aircraftId;
        this.data.size = this.data.selectedAircraft.size;
    }

    public displayAircraft(aircraft) {
        return aircraft ? `${aircraft.make} ${aircraft.model}` : aircraft;
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
