import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

// Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { Parametri } from '../../../services/paremeters.service';

export interface NewRampFeeDialogData {
    oid: number;
    price: number;
    waived: number;
    fboId: number;
    expirationDate: string;
    categoryType: number;
    categoryMinValue: number;
    categoryMaxValue: number;
    categoryStringValue: string;
}

@Component({
    selector: 'app-ramp-fees-dialog-new-fee',
    styleUrls: ['./ramp-fees-dialog-new-fee.component.scss'],
    templateUrl: './ramp-fees-dialog-new-fee.component.html',
})
export class RampFeesDialogNewFeeComponent {
    public categoryTypes: any[] = [
        { text: 'Aircraft Make/Model', value: 2 },
        { text: 'Weight Range', value: 3 },
        { text: 'Wingspan', value: 4 },
        { text: 'Tail Number', value: 5 },
    ];
    public aircraftTypes: any[];
    subscription: Subscription;

    //Errors
    public isWaivedNegative = false;
    public isCategoryValueNegative = false;
    public isFeeNegative = false;
    public isMaxCategoryGrater = true;

    constructor(
        public dialogRef: MatDialogRef<RampFeesDialogNewFeeComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewRampFeeDialogData,
        private aircraftsService: AircraftsService,
        private messageService: Parametri
    ) {
        this.data.categoryType = 2;
        this.messageService
            .getMessage()
            .subscribe((mymessage) => (this.data.expirationDate = mymessage));
        this.aircraftsService
            .getAll()
            .subscribe((result: any) => (this.aircraftTypes = result));
    }

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public checkForWaivedNegativeValue(value) {
        this.isWaivedNegative = false;

        if (value < 0)
            this.isWaivedNegative = true;
    }
    public getErrorData() {
    }
    public checkCategoryValidation(data): void {
        this.checkForCategoryValueNegativeValue(data);
        this.checkForCategoryMinValueGrater(data);
    }
    public checkForCategoryMinValueGrater(data): void {
        this.isMaxCategoryGrater = false;

        if (data.categoryMinValue < data.categoryMaxValue)
            this.isMaxCategoryGrater = true;
    }
    public checkForCategoryValueNegativeValue(data): void {
        this.isCategoryValueNegative = false;

        if (data.categoryMinValue < 0 || data.categoryMaxValue < 0)
            this.isCategoryValueNegative = true;
    }

    public checkForFeeNegativeValue(value) {
        this.isFeeNegative = false;

        if (value < 0)
            this.isFeeNegative = true;
    }
}
