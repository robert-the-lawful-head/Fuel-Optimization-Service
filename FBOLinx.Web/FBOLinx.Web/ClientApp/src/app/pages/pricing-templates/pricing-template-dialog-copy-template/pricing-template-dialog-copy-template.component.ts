import { Component, Inject } from '@angular/core';
import {
    MAT_DIALOG_DATA,
    MatDialog,
    MatDialogRef,
} from '@angular/material/dialog';
import { Router } from '@angular/router';

// Services
import { PricetiersService } from '../../../services/pricetiers.service';

export interface CopyPricingTemplateDialogData {
    currentPricingTemplateId: number;
    name: string;
}

@Component({
    selector: 'copy-pricing-templates-dialog-new-template',
    styleUrls: ['./pricing-template-dialog-copy-template.component.scss'],
    templateUrl: './pricing-template-dialog-copy-template.component.html',
})
export class PricingTemplatesDialogCopyTemplateComponent {
    constructor(
        public dialogRef: MatDialogRef<PricingTemplatesDialogCopyTemplateComponent>,
        public closeConfirmationDialog: MatDialog,
        public priceTiersService: PricetiersService,
        public router: Router,
        @Inject(MAT_DIALOG_DATA) public data: CopyPricingTemplateDialogData
    ) {
        // Prevent modal close on outside click
        dialogRef.disableClose = true;
    }

    ConfirmCopy() {
        this.dialogRef.close(this.data);
    }

    public onCancelClick(): void {
        this.dialogRef.close('cancel');
    }
}
