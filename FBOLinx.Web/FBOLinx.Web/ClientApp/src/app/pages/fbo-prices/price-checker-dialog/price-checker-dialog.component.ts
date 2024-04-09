import { Component, ViewChild, HostListener } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { SharedService } from 'src/app/layouts/shared-service';
import { PriceCheckerComponent } from '../../../shared/components/price-checker/price-checker.component';

// Services

@Component({
    selector: 'app-price-checker-dialog',
    styleUrls: ['./price-checker-dialog.component.scss'],
    templateUrl: './price-checker-dialog.component.html',
})
export class PriceCheckerDialogComponent {
    @ViewChild('priceChecker') private priceChecker: PriceCheckerComponent;
    public getScreenWidth: any;
    public getScreenHeight: any;

    constructor(
        private sharedService: SharedService,
        public dialogRef: MatDialogRef<PriceCheckerComponent>,
    ) {

    }

    get isCsr() {
        this.getScreenWidth = window.innerWidth;
        this.getScreenHeight = window.innerHeight;

        if (this.getScreenWidth <= 543)
            return true;
        else
            return this.sharedService.currentUser.role === 5;
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
