import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

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
    templateUrl: './customer-aircrafts-dialog-new-aircraft.component.html',
    styleUrls: ['./customer-aircrafts-dialog-new-aircraft.component.scss'],
})
export class CustomerAircraftsDialogNewAircraftComponent implements OnInit {
    // Public Members
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;

    constructor(
        public dialogRef: MatDialogRef<
            CustomerAircraftsDialogNewAircraftComponent
        >,
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
    public onAircraftTypeChanged() {
        this.data.aircraftId = this.data.selectedAircraft.aircraftId;
        this.data.size = this.data.selectedAircraft.size;
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
