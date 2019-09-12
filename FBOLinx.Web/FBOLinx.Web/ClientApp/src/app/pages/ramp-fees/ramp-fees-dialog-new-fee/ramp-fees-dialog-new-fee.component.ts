import { Component, Inject, Input } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { Parametri } from '../../../services/paremeters.service';
import { Subscription } from 'rxjs';

export interface NewRampFeeDialogData {
    oid: number;
    price: number;
    waived: number;
    fboId: number;
    expirationDate: string;
    categoryType: number;
    categoryMinValue: number;
    categoryMaxValue: number;
}

@Component({
    selector: 'app-ramp-fees-dialog-new-fee',
    templateUrl: './ramp-fees-dialog-new-fee.component.html',
    styleUrls: ['./ramp-fees-dialog-new-fee.component.scss']
})
/** ramp-fees-dialog-new-fee component*/
export class RampFeesDialogNewFeeComponent {

    public categoryTypes: any[] = [{ text: 'Aircraft Type', value: 2 }, { text: 'Weight Range', value: 3 }, { text: 'Wingspan', value: 4 }];
    public aircraftTypes: any[];
    subscription: Subscription;

    /** ramp-fees-dialog-new-fee ctor */
    constructor(public dialogRef: MatDialogRef<RampFeesDialogNewFeeComponent>, @Inject(MAT_DIALOG_DATA) public data: NewRampFeeDialogData,
        private aircraftsService: AircraftsService,
        private messageService: Parametri) {
        this.data.categoryType = 2;
        this.messageService.getMessage().subscribe(mymessage => this.data.expirationDate = mymessage );
       // this.data.expirationDate = this.datum;
        this.aircraftsService.getAll().subscribe((data: any) => this.aircraftTypes = data);
    }

    //Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
