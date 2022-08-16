import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { forEach } from 'lodash';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { PriceCheckerComponent } from '../../../shared/components/price-checker/price-checker.component';



@Component({
    selector: 'app-price-checker-dialog',
    styleUrls: ['./price-checker-dialog.component.scss'],
    templateUrl: './price-checker-dialog.component.html',
})
export class PriceCheckerDialogComponent {
    @ViewChild('priceChecker') private priceChecker: PriceCheckerComponent;

    constructor(
        public dialogRef: MatDialogRef<PriceCheckerComponent>,
    ) {

    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
